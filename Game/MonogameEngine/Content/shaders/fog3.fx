#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// ms elapsed
float time;

#define saturate(a) clamp(a, 0., 1.);

#define EPS 0.0001

#define SAMPLE_COUNT 40

#define SHADOW_LENGTH 3.6
#define SHADOW_ITERATIONS 4

#define DENSITY_INTENSITY .55
#define AMBIENT_INTENSITY 24.

float3 ABSORPTION_INTENSITY = float3(.5, .8, .7) * .25;

float3 sunDirection = normalize(float3(.5, .8, .5));
float3 lightColor = float3(.3, .3, .2);
    
float3 ambientLightDir = normalize(float3(0., 1., 0.));
float3 ambientLightColor = float3(.9, .7, .3);

float3 cloudColor = float3(.92, .92, .92);    
    
float3x3 m = float3x3( 0.00,  0.80,  0.60,
              			 -0.80,  0.36, -0.48,
              			 -0.60, -0.48,  0.64);

float hash(float n)
{
    return frac(sin(n) * 43758.5453);
}

float noise(in float3 x)
{
    float3 p = floor(x);
    float3 f = frac(x);
    
    f = f * f * (3.0 - 2.0 * f);
    
    float n = p.x + p.y * 57.0 + 113.0 * p.z;
    
    float res = lerp(lerp(lerp(hash(n +   0.0), hash(n +   1.0), f.x),
                          lerp(hash(n +  57.0), hash(n +  58.0), f.x), f.y),
                     lerp(lerp(hash(n + 113.0), hash(n + 114.0), f.x),
                          lerp(hash(n + 170.0), hash(n + 171.0), f.x), f.y), f.z);
    return res;
}

float fbm(float3 p)
{
    float f;
    p = mul(m, p) * 1.5;
    f  = .5 * noise(p);
    p = mul(m, p) * 2.;
    f += .15 * noise(p);
    //p = m * p * .2;
    //f += 0.15 * noise(p);
    return f;
}

float sdPlane(float3 p, float4 n) {
  return dot(p,n.xyz) + n.w;
}

float scene(float3 p) {
	float iTime = time / 1000.0;
   return 1. - sdPlane(p, float4(0., 1., 0., 1.)) + fbm(p * .7 + iTime) * 2.8;
}

float3x3 camera(float3 ro, float3 ta) 
{
    float3 forward = normalize(ta - ro);
    float3 side = normalize(cross(forward, float3(0., 1., 0.)));
    float3 up = normalize(cross(side, forward));
    return float3x3(side, up, forward);
}

float4 rayMarchFog(float3 p, float3 dir) 
{    
   float zStep = 16. / float(SAMPLE_COUNT);
   
   float transmittance = 1.;
   
   float3 color = float3(0.0, 0.0, 0.0);    
   
   float densityScale = DENSITY_INTENSITY * zStep;
   float shadowSize = SHADOW_LENGTH / float(SHADOW_ITERATIONS);
   float3 shadowScale = ABSORPTION_INTENSITY * shadowSize;
   float3 shadowStep = sunDirection * shadowSize;    
   
   for(int i = 0; i < SAMPLE_COUNT; i++) 
   {
    	float density = scene(p);
        
        if(density > EPS) 
        {
            density = saturate(density * densityScale);
            
            // #ifdef USE_DIRECTIONAL_LIGHT
            
            {
	            float3 shadowPosition = p;
    	         float shadowDensity = 0.;
        	      for(int si = 0; si < SHADOW_ITERATIONS; si++) {
            	   float sp = scene(shadowPosition);
                	shadowDensity += sp;
                	shadowPosition += shadowStep;
            	}
	            float3 attenuation = exp(-shadowDensity * shadowScale);
    	         float3 attenuatedLight = lightColor * attenuation;
        	      color += cloudColor * attenuatedLight * transmittance * density;
            }
                
            // #endif
            
            // #ifdef USE_AMBIENT_LIGHT
            
            {
	            float shadowDensity = 0.;
    	         float3 shadowPosition = p + ambientLightDir * .05;
        	      shadowDensity += scene(p) * .05;
            	shadowPosition = p + ambientLightDir * .1;
            	shadowDensity += scene(p) * .05;
            	shadowPosition = p + ambientLightDir * .2;
            	shadowDensity += scene(p) * .1;
            	float attenuation = exp(-shadowDensity * AMBIENT_INTENSITY);
            	float3 attenuatedLight = float3(ambientLightColor * attenuation);
            	color += cloudColor * attenuatedLight * transmittance * density;
            }
            
            // #endif
            
            transmittance *= 1. - density;            
        }

        if(transmittance < EPS) {
        	break;
    	}
        
      p += dir * zStep;
   }
    
   return float4(color, 1. - transmittance);
}

float4 main(float2 coord : TEXCOORD) : COLOR
{
	float iTime = time / 1000.0;
	float2 iResolution = float2(1920, 1080);
	float2 fragCoord = float2(coord.x * iResolution.x, coord.y * iResolution.y);
	float2 iMouse = float2(960, 540);

  	float2 uv = (fragCoord.xy - iResolution.xy * .5) / min(iResolution.x, iResolution.y);
    float2 mouse = (iMouse.xy - iResolution.xy * .5) / min(iResolution.x, iResolution.y);
    
    float rot = sin(iTime * .8) * .06;
    uv = mul(float2x2(float2(cos(rot), -sin(rot)), float2(sin(rot), cos(rot))), uv);

  	float fov = 1.2;
    float3 f = float3(0.0, 0.0, iTime);
    float3 lookAt = float3(
        cos(iTime * .4) * .4 + 0.0,
        sin(iTime * .5) * .18 + 1.6,
        0.0
    ) + f + float3(mouse, 0.0) * 1.;
    float3 cameraPos = float3(
        0.0,
        2.35,
        2.
    ) + f;

  	// raymarch
  	float3 rayOrigin = cameraPos;
  	float3 rayDirection = mul(camera(rayOrigin, lookAt), normalize(float3(uv, fov)));

    float4 color = float4(0.0, 0.0, 0.0, 0.0);
    
    float4 res = rayMarchFog(rayOrigin, rayDirection);
    color += res;
    
    float3 bg = lerp(
        float3(.6, .3, .4),
        float3(.9, .6, .4),
        smoothstep(-.6, .4, uv.y)
    );
    
    color.rgb += bg;
    
    return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}