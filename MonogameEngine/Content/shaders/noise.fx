#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float noiseAmount = 1;

// local stuff
sampler samp;

float hash2(float2 p)
{
    // procedural white noise	
    return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
}

float4 main(float2 coord : TEXCOORD) : COLOR
{
    float4 color = tex2D(samp, coord);

    float noiseFactor = hash2(coord);

    // because the images are premultiplied, you have to 0 out r,g,b if alpha = 0
    
    color.r += (noiseFactor - .5) * noiseAmount * color.a;
    color.g += (noiseFactor - .5) * noiseAmount * color.a;
    color.b += (noiseFactor - .5) * noiseAmount * color.a;

    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL main();
    }
}