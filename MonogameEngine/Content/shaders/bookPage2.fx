sampler s0;

float alpha = 1;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float hScale = 2.25;
	float vScale = .8;

	float xIn = 1 - coords.x;

	// adjusting curve to fit in 0 to 1 window
	float x = xIn * hScale;

	// initial curve
	float xFactor = (log(x * 3 + .6) + 2.75) * .4;

	// adjusting by slope 1
	xFactor -= x * .6;

	// adding curve 1
	if (xIn > .3)
		xFactor += (x - .3 * hScale) * x * .08;

	// adding curve 2
	if (xIn > .55)
		xFactor += (x - .55 * hScale) * x * .03;

	// fitting f(x) to 0 to 1 window
	xFactor = (xFactor - 1) * .5 + 1;

	// using and offset instead of a function
	float yOffset = xFactor - 1;
	yOffset *= vScale;

	float2 uv = float2(coords.x * xFactor, coords.y + yOffset);

	float4 color = tex2D(s0, uv);

	color.a *= alpha;

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}