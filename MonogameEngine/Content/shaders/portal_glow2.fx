

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

float r = 0;
float g = 0;
float b = 0;
float a = 0;
float w = 0;
float h = 0;
float time = 0;
float tileSize = 1.0;

sampler samp;

float waterHighlight(float2 p, float time, float foaminess)
{
    float2 i = p;
    float c = 0.0;
    float foaminess_factor = lerp(1.0, 2.0, foaminess);
    float inten = .005 * foaminess_factor;

    for (int n = 0; n < MAX_ITER; n++) 
    {
        float t = time * (1.0 - (3.5 / float(n+1)));
        i = p + float2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
        c += 1.0/length(float2(p.x / (sin(i.x+t)),p.y / (cos(i.y+t))));
    }
    c = 0.2 + c / (inten * float(MAX_ITER));
    c = 1.17-pow(c, 1.4);
    c = pow(abs(c), 20.0);
    return c / sqrt(foaminess_factor);
}

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float2 uResolution = float2(w, h);
    float2 fragCoord = float2(w * coords.x, h * coords.y);

    float4 ogColor = tex2D(samp, coords);

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
            float2 textureCoordinates = (tileNumber + float2((x + 0.5)/textureSamplesCount, (y + 0.5)/textureSamplesCount)) * tileSize / uResolution.xy;
            textureCoordinates.y = 1.0 - textureCoordinates.y;
            textureCoordinates = clamp(textureCoordinates, 0.0 + textureEdgeOffset, 1.0 - textureEdgeOffset);
            
            float2 uv = textureCoordinates;

            float2 uv_square = float2(uv.x * uResolution.x / uResolution.y, uv.y);
            float dist_center = pow(2.0*length(uv - 0.5), 2.0);

            float foaminess = smoothstep(0.4, 1.8, dist_center);
            float clearness = 0.1 + 0.9*smoothstep(0.1, 0.5, dist_center);

            float2 p = fmod(uv_square*TAU*TILING_FACTOR, TAU)-250.0;

            float c = waterHighlight(p, time, foaminess);

            float3 water_color = float3(r, g, b);
            float3 color = float3(c, c, c);
            color = clamp(color + water_color, 0.0, 1.0);

            color = lerp(water_color, color, clearness);
    
            accumulator += float4(color.x, color.y, color.z, 1.0);
       }
    }
    
    float total = textureSamplesCount * textureSamplesCount;
    float4 portalColor = accumulator / float4(total, total, total, total);

    #if defined(USE_TILE_BORDER) || defined(USE_ROUNDED_CORNERS)
        float2 pixelNumber = floor(fragCoord - (tileNumber * tileSize));
        pixelNumber = mod(pixelNumber + borderSize, tileSize);
        
    #if defined(USE_TILE_BORDER)
        float pixelBorder = step(min(pixelNumber.x, pixelNumber.y), borderSize) * step(borderSize * 2.0 + 1.0, tileSize);
    #else
        float pixelBorder = step(pixelNumber.x, borderSize) * step(pixelNumber.y, borderSize) * step(borderSize * 2.0 + 1.0, tileSize);
    #endif
        portalColor *= pow(portalColor, float4(pixelBorder));
    #endif

    // portalColor.a = a;

    // a is how much of the portal color we use. multiply by the original alpha
    float4 fragColor = (ogColor * (1 - a) + portalColor * a) * ogColor.a;

    return fragColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}