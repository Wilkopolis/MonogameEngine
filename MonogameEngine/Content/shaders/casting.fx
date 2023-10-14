float p1;

sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	float2 a = float2(cos(1.570796325), -sin(1.570796325));
	float2 b = float2(sin(1.570796325), cos(1.570796325));

	float2 pos = coords - float2(0.5, 0.5);

	float dist = length(pos) - p1;

	float2 hack = float2(pos.x * a.x + pos.y * b.x, pos.x * a.y + pos.y * b.y);
	float angle = atan2(hack.y, hack.x) + 3.14159265;

	float adj = (sin(dist * 16 + sin(angle * 16) / 10) + 1.5) / 4 + .1;

	color.a *= adj;
	return color;
}

technique Technique1
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
};