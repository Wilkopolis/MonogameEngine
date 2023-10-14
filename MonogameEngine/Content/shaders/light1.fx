#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;
float r = 0;
float g = 0;
float b = 0;
float a = 0;
float radius = 1;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float dist = .5 - length(float2(.5, .5) - coords);
    dist *= 2;
    dist *= radius;

    dist = 1 - pow(2, -4 * dist);
    
    return float4(r * dist, g * dist, b * dist, a * dist);
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}

