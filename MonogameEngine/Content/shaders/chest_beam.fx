#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float r = 0;
float g = 0;
float b = 0;
float w = 0;
float h = 0;
float time = 0;

float4 Strand(float2 fragCoord, float4 color, float hoffset, float hscale, float vscale, float timescale)
{
    float2 uResolution = float2(w, h);
    float2 uv = fragCoord.xy / uResolution;
    
    float glow = 0.06 * uResolution.y;
    float twopi = 6.28318530718;
    float curve = 1.0 - abs(fragCoord.y - (sin(fmod(fragCoord.x * hscale / 100.0 / uResolution.x * 1000.0 + time * timescale + hoffset, twopi)) * uResolution.y * 0.25 * vscale + uResolution.y / 2.0));
    float i = clamp(curve, 0.0, 1.0);
    
    i *= clamp(uv.x * uv.x * uv.x * uv.x, 0, 1);
        
    i += clamp((glow + curve) / glow, 0.0, 1.0) * 0.2;
    
    float4 result = i * color;
                
    return result;
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float2 uResolution = float2(w, h);
    float2 fragCoord = float2(w * coords.x, h * coords.y);
    float2 uv = fragCoord.xy / uResolution;

    float4 c = float4(0, 0, 0, 0);
    c += Strand(fragCoord, float4(r, g, b, 1), 0.7934 + 1.0 + 0 * -21.0, 0.10, 0.16, -10.0 * .01);
    c += Strand(fragCoord, float4(r, g, b, 1), 0.645 + 1.0 + 0 * -22.0, 0.20, 0.2, -10.3 * .01);
    c += Strand(fragCoord, float4(r, g, b, 1), 0.735 + 1.0 + 0 * -23.0, 0.30, 0.19, -8.0 * .01);
    c += Strand(fragCoord, float4(r, g, b, 1), 0.9245 + 1.0 + 0 * -24.0, 0.40, 0.14, -12.0 * .01);
    c += Strand(fragCoord, float4(r, g, b, 1), 0.7234 + 1.0 + 0 * -25.0, 0.50, 0.23, -14.0 * .01);
    c += Strand(fragCoord, float4(r, g, b, 1), 0.84525 + 1.0 + 0 * -26.0, 0.60, 0.18, -9.0 * .01);
    c += Strand(fragCoord, float4(r, g, b, 1), 0.69525 + 1.0 + 0 * -28.0, 0.70, 0.18, -13.0 * .01);
    c += Strand(fragCoord, float4(r, g, b, 1), 0.96525 + 1.0 + 0 * -29.0, 0.80, 0.18, -15.0 * .01);
    
    float4 fragColor = float4(c.r, c.g, c.b, c.a);
    
    if (uv.x < .1)
        fragColor *= uv.x / .1;
    if (uv.x > .5)
        fragColor *= clamp((.25 - uv.x + .7) / .4, 0, 1);

    //fragColor.a *= .8;

    return fragColor;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}

