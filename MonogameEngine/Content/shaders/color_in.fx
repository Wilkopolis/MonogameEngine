#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;

float R = 1;
float G = 1;
float B = 1;

// if one, write our alpha to 1 when its > 0
float forceAlpha = 0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);
	
    color = float4(R, G, B, color.a);
	if (forceAlpha && color.a > 0)
        color.a = 1;
	
	return color;
}

technique Technique1
{
	pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}