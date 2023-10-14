#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;

float w = 0;
float h = 0;
float time = 0;

// Mikael Lemercier & Fabrice Neyret , June, 2013

#define DENS 2.           // tau.rho
#define rad .3            // sphere radius
#define H   .05           // skin layer thickness (for linear density)
#define ANIM true         // true/false
#define PI 3.14159

static float4 skyColor =     float4(0,0,0,0);
static float3 sunColor = 10.*float3(.7,.58,.72);   // NB: is Energy 

// --- noise functions from https://www.shadertoy.com/view/XslGRr
// Created by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

static  float3x3 m = float3x3( 0.00,  0.80,  0.60,
              -0.80,  0.36, -0.48,
              -0.60, -0.48,  0.64 );

static float Rz=0.0;  // 1/2 ray length inside object

float hash( float n )
{
    return frac(sin(n)*43758.5453);
}

float noise( in float3 x )
{
    float3 p = floor(x);
    float3 f = frac(x);

    f = f*f*(3.0-2.0*f);

    float n = p.x + p.y*57.0 + 113.0*p.z;

    float res = lerp(lerp(lerp( hash(n+  0.0), hash(n+  1.0),f.x),
                          lerp( hash(n+ 57.0), hash(n+ 58.0),f.x),f.y),
                     lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
                          lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
    return res;
}

float fbm( float3 p )
{
    float f;
    f = 0.5000*noise( p ); 
    p = mul(m, p) * 2.02;
    f += 0.2500*noise( p ); 
    p = mul(m, p) * 2.03;
    f += 0.1250*noise( p ); 
    p = mul(m, p) * 2.01;
    f += 0.0625*noise( p );
    return f;
}
// --- End of: Created by inigo quilez --------------------

float3 noise3( float3 p )
{
	if (ANIM) p += time;
    float fx = noise(p);
    float fy = noise(p+float3(1345.67,0,45.67));
    float fz = noise(p+float3(0,4567.8,-123.4));
    return float3(fx,fy,fz);
}
float3 fbm3( float3 p )
{
	if (ANIM) p += time;
    float fx = fbm(p);
    float fy = fbm(p+float3(1345.67,0,45.67));
    float fz = fbm(p+float3(0,4567.8,-123.4));
return float3(fx,fy,fz);
}
float3 perturb3(float3 p, float scaleX, float scaleI)
{
    scaleX *= 2.;
	return scaleI*scaleX*fbm3(p/scaleX); // usually, to be added to p
}

float constantDensityTransmittance(float NDotL,float NDotO)
{
    return NDotL/(DENS*(NDotL+NDotO));
}

float linearDensityTransmittance(float NDotL,float NDotO,float LDotO)
{
	return sqrt(PI/2.) / sqrt(DENS/H* NDotO/NDotL*(NDotL+NDotO) ) ; 
}

float4 intersectSphere(float3 rpos, float3 rdir)
{
    float3 op = float3(0.0, 0.0, 0.0) - rpos;
    //float rad = 0.3;
  
    float eps = 1e-5;
    float b = dot(op, rdir);
    float det = b*b - dot(op, op) + rad*rad;
      
    if (det > 0.0)
    {
        det = sqrt(det);
        float t = b - det;
        if (t > eps)
        {
            float4 P = float4(normalize(rpos+rdir*t), t);
            Rz = rad*P.z;   // 1/2 ray length inside object
            
            // skin layer counts less
            float dH = 1.+H*(H-2.*rad)/(Rz*Rz);
            if (dH>0.) // core region
                Rz *= .5*(1.+sqrt(dH));
            else
                Rz *= .5*rad*(1.-sqrt(1.-Rz*Rz/(rad*rad)))/H;
                
            return P;
        }
    }
  
    return float4(0.0, 0.0, 0.0, 0.0);
}

bool computeNormal(in float3 cameraPos, in float3 cameraDir, out float3 normal)
{
    cameraPos = cameraPos+perturb3(cameraDir,.06,1.5);
    float4 intersect = intersectSphere(cameraPos,cameraDir);
    if ( intersect.w > 0.)
    {
        normal = intersect.xyz;
        //normal = normalize(normal+perturb3(normal,.3,30.));
        return true;
    }
    return false;
}
float computeTransmittance( in float3 cameraPos, in float3 cameraDir, in float3 lightDir )
{
    float3 normal;
    if ( computeNormal(cameraPos,cameraDir,normal))
    {
        float NDotL = clamp(dot(normal,lightDir),0.,1.);
        float NDotO = clamp(dot(normal,cameraPos),0.,1.);
        float LDotO = clamp(dot(lightDir,cameraPos),0.,1.);
      
#if LINEAR_DENSITY
        float transmittance = linearDensityTransmittance(NDotL,NDotO,LDotO)*.5;
#else
        float transmittance = constantDensityTransmittance(NDotL,NDotO);
#endif
        return transmittance;
    }

    return -1.;
}

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float2 iResolution = float2(w, h);
	float2 fragCoord = float2(w * coords.x, h * coords.y);

    float4 fragColor;
    //camera
    float3 cameraPos = float3(0.0,0.0,1.0);      
    float3 cameraTarget = float3(0.0, 0.0, 0.0);
    float3 ww = normalize( cameraPos - cameraTarget );
    float3 uu = normalize(cross( float3(0.0,1.0,0.0), ww ));
    float3 vv = normalize(cross(ww,uu));
    float2 q = fragCoord.xy / iResolution.xy;
    float2 p = -1.0 + 2.0*q;
    p.x *= iResolution.x/ iResolution.y;
    float3 cameraDir = normalize( p.x*uu + p.y*vv - 1.5*ww );
 
    //light
    float theta = (iResolution.x / 2.1 / iResolution.x * 2. - 1.)*PI;
    float phi = (iResolution.y * .25 / iResolution.y - .5)*PI;
    float3 lightDir =float3(sin(theta)*cos(phi),sin(phi),cos(theta)*cos(phi));
  
	// shade object
    float transmittance = computeTransmittance( cameraPos, cameraDir, lightDir );
	
	// display: special cases
	if (transmittance<0.) 		    fragColor = skyColor;
	else if (transmittance>1.) 		    fragColor = float4(1.,0.,0.,1.); 		
	else
	{ // display: object
		Rz = 1.-exp(-8.*DENS*Rz);
	    float alpha = Rz;
    	float3 frag = float3(transmittance,transmittance,transmittance);
   		fragColor = float4(frag*sunColor * .5,alpha) + (1.-alpha)*skyColor;
	}
	
	fragColor.rgb *= .4;
	fragColor.a *= .8;
    return fragColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}

