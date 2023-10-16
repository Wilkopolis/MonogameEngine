#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float p1;
sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	float2 a = float2(cos(1.570796325), -sin(1.570796325));
	float2 b = float2(sin(1.570796325), cos(1.570796325));

	float2 pos = coords - float2(0.5, 0.5);

	float2 hack = float2(pos.x * a.x + pos.y * b.x, pos.x * a.y + pos.y * b.y);
	float angle = atan2(hack.y, hack.x) + 3.14159265;

	color.a *= step(p1, angle);
	return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}