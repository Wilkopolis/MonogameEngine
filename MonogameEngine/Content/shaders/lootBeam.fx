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
float time = 0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float2 uv = coords;
    float4 fragColor;
    
    float4 zapColor = float4(r, g, b, 1);
    float dist = (.5 - length(.5 - uv.x)) * 2;
    float factor = log(dist) + 1;
    
    factor = clamp(factor, 0, 1);
        
    factor = factor * pow(factor + .25, 4);
    
    factor = clamp(factor, 0, 3);
    
    factor *= .5 - abs(.5 - uv.y);
    
    float offset = fmod(time, 6.28318530);
    
    fragColor = factor * zapColor;
    
    fragColor += factor * zapColor * .05 * (1 + sin(offset * 1.2));
    fragColor += factor * zapColor * .15 * (1 + sin(offset * 2));
    fragColor += factor * zapColor * .05 * (1 + sin(offset * 1.4));
    fragColor += factor * zapColor * .1 * (1 + sin(offset * 1.7));
    
    return fragColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}

