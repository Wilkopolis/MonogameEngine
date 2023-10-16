#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler2D input : register(s0);
int w;
int h;
float Size = 8.0; // BLUR SIZE (Radius)

// void mainImage( out vec4 fragColor, in float2 fragCoord )
float4 main(float2 uv : TEXCOORD) : COLOR
{
    float Pi = 6.28318530718; // Pi*2
    
    // GAUSSIAN BLUR SETTINGS {{{
    float Directions = 32.0; // BLUR DIRECTIONS (Default 16.0 - More is better but slower)
    float Quality = 8.0; // BLUR QUALITY (Default 4.0 - More is better but slower)
    // GAUSSIAN BLUR SETTINGS }}}
   
    float2 Radius = Size / float2(w, h);
    
    // Normalized pixel coordinates (from 0 to 1)
    // float2 uv = fragCoord/iResolution.xy;
    // Pixel colour
    float4 Color = tex2D(input, uv);
    
    // Blur calculations
    for (float d = 0.0; d < Pi; d += Pi / Directions)
    {
        for (float i = 1.0 / Quality; i <= 1.0; i += 1.0 / Quality)
        {
            Color += tex2D(input, uv + float2(cos(d), sin(d)) * Radius * i);
        }
    }
    
    // Output to screen
    Color /= Quality * Directions - 15.0;
    return Color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL main();
    }
}