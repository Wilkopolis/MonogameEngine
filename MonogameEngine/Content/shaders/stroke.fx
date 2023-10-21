#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0
#endif

sampler s0;
float r = 0;
float g = 0;
float b = 0;
float a = 0;
// how many pixels
float outlineSize = 0;
// dimensions
int w = 0;
int h = 0;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    const float4 target = float4(0.0, 0.0, 0.0, 0.0);
    const float TAU = 6.28318530;
    const float steps = 32.0;
    
    float2 uv = coords;
    
    // Correct aspect ratio
    float2 aspect = 1.0 / float2(w, h);
    
    float4 fragColor = float4(0, 0, 0, 0);
    for (float i = 0.0; i < TAU; i += TAU / steps)
    {
		// Sample image in a circular pattern
        float2 offset = float2(sin(i), cos(i)) * aspect * outlineSize;
        float4 col = tex2D(s0, uv + offset);
		
		// Mix outline with background
        float alpha = smoothstep(0.5, 0.7, distance(col.rgba, target));
        fragColor = lerp(fragColor, float4(r, g, b, a), alpha);
    }
	
    // Overlay original color
    float4 mat = tex2D(s0, uv);
    float factor = smoothstep(0.5, 0.7, distance(mat.rgba, target));
    fragColor = lerp(fragColor, mat, factor);
    return fragColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}
