#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define TAU 6.28318530718

#define TILING_FACTOR 1.0
#define MAX_ITER 8

float w = 0;
float h = 0;
float tileSize = 1.0;
sampler s0;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 uResolution = float2(w, h);
    float2 fragCoord = float2(w * coords.x, h * coords.y);

    const float minTileSize = 1.0;
    const float maxTileSize = 32.0;
    const float textureSamplesCount = 3.0;
    const float textureEdgeOffset = 0.005;
    const float borderSize = 1.0;

    float2 tileNumber = floor(fragCoord / tileSize);
    
    float4 accumulator = float4(0, 0, 0, 0);
    for (float y = 0.0; y < textureSamplesCount; ++y)
    {
        for (float x = 0.0; x < textureSamplesCount; ++x)
        {
            float2 textureCoordinates = (tileNumber + float2((x + 0.5) / textureSamplesCount, (y + 0.5) / textureSamplesCount)) * tileSize / uResolution.xy;
            textureCoordinates.y = textureCoordinates.y;
            textureCoordinates = clamp(textureCoordinates, 0.0 + textureEdgeOffset, 1.0 - textureEdgeOffset);
            
            float2 uv = textureCoordinates;
            float4 color = tex2D(s0, uv);
    
            accumulator += float4(color.x, color.y, color.z, color.a);
        }
    }
    
    float total = textureSamplesCount * textureSamplesCount;
    float4 fragColor = accumulator / float4(total, total, total, total);

#if defined(USE_TILE_BORDER) || defined(USE_ROUNDED_CORNERS)
        float2 pixelNumber = floor(fragCoord - (tileNumber * tileSize));
        pixelNumber = mod(pixelNumber + borderSize, tileSize);
        
#if defined(USE_TILE_BORDER)
        float pixelBorder = step(min(pixelNumber.x, pixelNumber.y), borderSize) * step(borderSize * 2.0 + 1.0, tileSize);
#else
        float pixelBorder = step(pixelNumber.x, borderSize) * step(pixelNumber.y, borderSize) * step(borderSize * 2.0 + 1.0, tileSize);
#endif
        fragColor *= pow(fragColor, float4(pixelBorder));
#endif

    return fragColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}

