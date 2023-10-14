float time;
float offsetX;
float offsetY;

sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);
	
	float2 start = float2(1920, 1920);
	float2 end = float2(3840, 0);

	float2 pos = float2(offsetX + coords.x * 96, offsetY + coords.y * 96);

	float2 dir = float2(10, 10);

	start += dir * time;
	end += dir * time;

	float a = length(start - end);
	float b = length(start - pos);
	float c = length(end - pos);
	float s = (a + b + c)/2;
	float d = 2 * sqrt(s*(s-a)*(s-b)*(s-c)) / a;

	color.a *= .65 + max(0, sin(d / 28)) * .3;

	return color;
}

technique Technique1
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
};