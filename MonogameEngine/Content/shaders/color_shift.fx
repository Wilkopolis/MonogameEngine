sampler s0;

float RScale = 1;
float GScale = 1;
float BScale = 1;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	color.r *= RScale;
	color.g *= GScale;
	color.b *= BScale;
	
	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}