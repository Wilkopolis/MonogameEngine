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
float time = 0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float2 uv = coords;
    float4 fragColor;
    
    float4 zapColor = float4(r, g, b, 1);
    float4 hilight = float4(r * 1.5, g * 1.5, b * 1.5, 1);
    float dist = .5 - length(float2(.5, .5) - uv);
    float factor = log(dist) + 1.25;
    
    factor = clamp(factor, 0, 1);
        
    factor = pow(factor + .6, 18);
    
    factor = clamp(factor, 0, 3);
    
    factor *= .5 - abs(.5 - uv.y);
    
    float offset = fmod(time, 6.28318530);
    
    fragColor = factor * zapColor;
    
    fragColor = factor * hilight + 4 * factor * zapColor;
    
    fragColor += factor * zapColor * .05 * (1 + sin(offset * 30.2));
    fragColor += factor * zapColor * .15 * (1 + sin(offset * 20));
    fragColor += factor * zapColor * .05 * (1 + sin(offset * 33.4));
    fragColor += factor * zapColor * .1 * (1 + sin(offset * 17.7));

    return fragColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}

