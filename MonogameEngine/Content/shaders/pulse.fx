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
float a = 0;
float radius = 0;
float top = 1;

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
    uv *= 19 * (1 - radius);
    float2 newV = float2(uv.x, uv.y * 2.2 );
    float len = length(newV);
    float k = 1.0;
    float len2 = 1.0;

    len2 = sphere(len*k,sqrt(2.0))/sphere(1.0*k,sqrt(2.0));
    uv = uv * len2 * 0.5 / len;
    uv = uv + 0.5;
    
    float glow = 0.050 / (0.015 + distance(len, 1.0));

    if (len > 1.2)
        glow = glow / (len * len * len);
    
    float4 col2 = float4(r, g, b, 1);
    
    float4 color = glow * col2 * a;

    if (top == 1 && coords.y >= .5)
        color = float4(0, 0, 0, 0);
    else if (top == 0 && coords.y < .5)
        color = float4(0, 0, 0, 0);

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}

