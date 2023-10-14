float a;
float w;
float h;
int borderSize;
sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	float2 position = float2(coords.x * w, coords.y * h);

	if (position.x > borderSize && position.x < w - borderSize && position.y > borderSize && position.y < h - borderSize)
	{
		color.rgba = 0;
	}
	else
	{
		color.rgba *= a;
	}

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}