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
float outlineSize = 0;
float a = 0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float2 uv = coords;

    const float4 target = float4(0.0, 0.0, 0.0, 0.0); // Find transparent
    const float threshold = 0.4; // Controls target color range
    const float softness = 0.1; // Controls linear falloff

    const float steps = 32.0;
    const float total = steps / 6.28318530;
    
    // Apply linear color key
    float4 mat = tex2D(s0, uv);
    float diff = distance(mat.a, target.a) - threshold;
    float factor = clamp(diff / softness, 0.0, 1.0);

    float4 fragColor = float4(0.0, 0.0, 0.0, 0.0);
    
    for (float i = 0.0; i < steps; i++) {
        // Sample image in a circular pattern
        float j = i / total;
        float4 col = tex2D(s0, uv + float2(sin(j), cos(j)) * outlineSize);
        
        // Apply linear color key
        float diff2 = distance(col.a, target.a) - threshold;
        fragColor = lerp(fragColor, float4(r, g, b, 1.0), clamp(diff2 / softness, 0.0, 1.0));
    }
    
    fragColor = lerp(fragColor, mat, factor);
    fragColor.rgba *= a;
    return fragColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}
