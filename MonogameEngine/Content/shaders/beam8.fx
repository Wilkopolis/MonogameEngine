#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float w = 0;
float h = 0;
float x = 0;
float y = 0;
float time = 0;
float r = 0;
float g = 0;
float b = 0;

float4 Strand(float2 fragCoord, float4 color, float hoffset, float hscale, float vscale, float timescale)
{
    float2 uResolution = float2(w, h);
    // rotate
    float glow = 0.3 * uResolution.y;
    float twopi = 6.28318530718;
    float p = fragCoord.x * hscale / 100.0 / uResolution.x * 1000.0 + time * timescale + hoffset;
    float q = abs(fmod(p, twopi));
    float curve = 1.0 - abs(fragCoord.y - (sin(q) * uResolution.y * 0.25 * vscale + uResolution.y / 2.0));
    float i = clamp(curve, 0.0, 1.0);
    i += clamp((glow + curve) / glow, 0.0, 1.0) * 0.8;
    
    float4 result = i * color;

    if (p > -twopi)
    {
        float factor = -twopi - p;
        float eased = 1 - pow(1 - factor, 2);
        float adjust = 1 - eased;
        result *= 1 / adjust;
    }
    else if (p <= -2 * twopi)
    {
        float factor = clamp(-2 * twopi - p, 0, 1);
        float eased = 1 - pow(1 - factor, 2);
        float adjust = 1 - eased;
        result *= adjust;
    }
    
    if (q > twopi / 4.0 && q < twopi * 3 / 4.0)
    {
        if (x == 0)
            result = float4(0,0,0,0);
    }
    else
    {
        if (x == 1)
            result = float4(0,0,0,0);
    }

    return result;
}

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float2 uResolution = float2(w, h);
    float2 fragCoord = float2(w * coords.x, h * coords.y);

    // float r = .9;
    // float g = .6;
    // float b = .4;
    
    float2 uv = fragCoord.xy / uResolution;
    
    fragCoord.x = 1 - fragCoord.x;

    float timescale = .001;
    float4 c = float4(0, 0, 0, 0);
    
    if (y == 0)
        c += Strand(fragCoord, float4(r, g, b, 1), 0.500000 + time * -8.0 * .4, 0.60, 0.8, -10.0 * timescale);
    else
        c += Strand(fragCoord, float4(r, g, b, 1), 3.141529 + time * -8.0 * .4, 0.60, 0.8, -10.0 * timescale);

    float4 fragColor = float4(c.r, c.g, c.b, c.a);
    
    if (uv.x < .3)
        fragColor *= uv.x/.3;
        
    if (uv.x > .7)
        fragColor *= (1 - uv.x)/.3;
    
    return fragColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}