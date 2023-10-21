sampler s0;

float RScale = 1;
float GScale = 1;
float BScale = 1;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords) * float4(RScale, GScale, BScale, 1);
	
	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}