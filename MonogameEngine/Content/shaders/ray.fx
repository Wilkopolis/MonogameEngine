float p1;
float p2;
float q1;
float q2;

// local stuff
sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	float2 pt1 = float2(p1, p2);
	float2 pt2 = float2(q1, q2);

    float2 v1 = pt2 - pt1;
    float2 v2 = pt1 - coords;
    float2 v3 = float2(v1.y,-v1.x);
    float dist = abs(dot(v2,normalize(v3)));
		
	color.rgba *= min(dist * 2, 1);
	// color.rgba *= .5f;

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}