#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float ColorR = 0;
float ColorG = 0;
float ColorB = 0;
int w = 0;
int h = 0;

float radius = 0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float2 uResolution = float2(w, h);
    float2 fragCoord = float2(w * coords.x, h * coords.y);
    float2 uv = fragCoord.xy / uResolution;

    float y = abs(fragCoord.y - uResolution.y / 2); 

    float g = clamp((radius - y) / radius, 0, 1);                              
    float3 col = float3(ColorR, ColorG, ColorB);
    col *= pow(g, 3);        

    float4 fragColor = float4(col.r, col.g, col.b, 1);                          

    if (fragCoord.x < radius)
    {
        float2 dist = fragCoord - float2(radius, uResolution.y / 2);         
        float f = clamp((radius - length(dist)) / radius, 0, 1);
        fragColor.rgb = float3(ColorR, ColorG, ColorB) * pow(f, 3);         
    }
    if (fragCoord.x > uResolution.x - radius)
    {
        float2 dist = fragCoord - float2(uResolution.x - radius, uResolution.y / 2);         
        float f = clamp((radius - length(dist)) / radius, 0, 1);
        fragColor.rgb = float3(ColorR, ColorG, ColorB) * pow(f, 3);
    }

    return fragColor;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}

