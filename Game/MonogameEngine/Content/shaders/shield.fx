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
float r = 0;
float g = 0;
float b = 0;

float2 hash2( float2 p )
{
    // procedural white noise	
	return frac(sin(float2(dot(p,float2(127.1,311.7)),dot(p,float2(269.5,183.3))))*43758.5453);
}

float sphere(float t, float k)
{
    float d = 1.0+t*t-t*t*k*k;
    if (d <= 0.0)
        return -1.0;
    float x = (k - sqrt(d))/(1.0 + t*t);
    return asin(x*t);
}

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float PI = 3.14159265;
	float2 iResolution = float2(w, h);
	float2 fragCoord = float2(w * coords.x, h * coords.y);
    
  	float2 uv = fragCoord.xy - 0.5*iResolution.xy;
    float v = iResolution.x;
    if (v > iResolution.y)
        v = iResolution.y;
	uv /= v;
    uv *= 3.0;
    float len = length(uv);
    float k = 1.0;
    float len2;

    len2 = sphere(len*k,sqrt(2.0))/sphere(1.0*k,sqrt(2.0));
	uv = uv * len2 * 0.5 / len;
	uv = uv + 0.5;
    
    float2 pos = uv;
    float scale1 = 40.0;
    float scale2 = 20.0;
    float val = 0.0;
    
    float glow = 0.020 / (0.015 + distance(len, 1.0));
    
    val = (cos(PI*val) + 1.0) * 0.5;
    float4 col2 = float4(r, g, b, 1.0); //float4(.4, .3, .5, 1.0);

    float radius = length(coords - float2(.5,.5));

   	if (len2 < -.1)
   	{
	   	glow = glow * glow;
	   	if (glow < 0)
   			glow = 0;
   	}
    
    float4 color = step(len, 1.0) * 0.3 * col2 * val + glow * col2;

    if (len2 > 0)
    	color += float4(r / 4, g / 4, b / 4, .4);

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}

