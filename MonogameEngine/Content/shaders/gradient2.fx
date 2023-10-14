// position in the text where the gradient is centered
float x1;
float y1;
// how far before the gradient should hit 0
float radius;
// color 1
float a1;
// color 2
float a2;

// local stuff
sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	float2 center = float2(x1, y1);
	float r = length(coords - center);
	float diff =  1 - (radius - r);
	if (diff < 0)
		diff = 0;

	float da = a1 - a2;
	float a = da * diff + a2;
		
	color.rgba *= a;

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}