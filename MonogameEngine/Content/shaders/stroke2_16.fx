#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0;

int w = 1;
int h = 1;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float2 pixel = float2(coords.x * w, coords.y * h);

    int radius = 16;

    int hits = 0;
    float steps = 0.0;
    for (int i = -radius; i < radius; i++)
    {
        for (int j = -radius; j < radius; j++)
        {
            if (sqrt(i*i + j*j) > radius)
                continue;

            float2 p = float2(pixel.x + i, pixel.y + j);
            float2 uv = float2(p.x / w, p.y / h);

            if (uv.x < 0 || uv.y < 0 || uv.x > 1 || uv.y > 1)
                continue;

            steps++;

            float4 color = tex2D(s0, uv);
            if (color.a > 0)
                hits++;
        }
    }

    if (hits > 0)
        return float4(1,1,1,1);
    else
        return float4(0,0,0,1);
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}
