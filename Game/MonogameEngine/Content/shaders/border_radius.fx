float a;
bool isBorder;
float w;
float h;
int borderRadius;
int borderSize;
sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	float2 corner1 = float2(2 * borderRadius, 2 * borderRadius);
	float2 corner2 = float2(w - 2 * borderRadius, 2 * borderRadius);
	float2 corner3 = float2(w - 2 * borderRadius, h - 2 * borderRadius);
	float2 corner4 = float2(2 * borderRadius, h - 2 * borderRadius);

	float2 position = float2(coords.x * w, coords.y * h);

	float r1 = length(corner1 - position);
	float r2 = length(corner2 - position);
	float r3 = length(corner3 - position);
	float r4 = length(corner4 - position);

	bool r1Pass = r1 < 2 * borderRadius;
	bool r2Pass = r2 < 2 * borderRadius;
	bool r3Pass = r3 < 2 * borderRadius;
	bool r4Pass = r4 < 2 * borderRadius;

	if (position.x > 2 * borderRadius && position.x < w - 2 * borderRadius || position.y > 2 * borderRadius && position.y < h - 2 * borderRadius || r1Pass || r2Pass || r3Pass || r4Pass)
	{
		color.rgba *= a;

		if (isBorder == true)
		{
			int borderRadius2 = borderRadius + borderSize;
			int borderRadius3 = 2 * borderRadius + 2 * borderSize;
			float w2 = w - borderSize;
			float h2 = h - borderSize;

			float2 corner12 = float2(borderRadius3, borderRadius3);
			float2 corner22 = float2(w2 - borderRadius3, borderRadius3);
			float2 corner32 = float2(w2 - borderRadius3, h2 - borderRadius3);
			float2 corner42 = float2(borderRadius3, h2 - borderRadius3);

			float2 position2 = float2(coords.x * w2, coords.y * h2);

			float r12 = length(corner12 - position2);
			float r22 = length(corner22 - position2);
			float r32 = length(corner32 - position2);
			float r42 = length(corner42 - position2);

			bool r1Pass2 = r12 < 2 * borderRadius;
			bool r2Pass2 = r22 < 2 * borderRadius;
			bool r3Pass2 = r32 < 2 * borderRadius;
			bool r4Pass2 = r42 < 2 * borderRadius;

			bool zone1 = position2.y > 2 * borderSize && position2.y < borderSize + 2 * borderRadius;
			bool zone2 = position2.x > 2 * borderSize && position2.x < borderSize + 2 * borderRadius;
			bool zone3 = position2.y > h - (borderSize + 2 * borderRadius) && position2.y < h - 3 * borderSize;
			bool zone4 = position2.x > w - (borderSize + 2 * borderRadius) && position2.x < w - 3 * borderSize;
			bool zone5 = position2.y > 2 * borderSize && position2.y < h - 4 * borderSize && position2.x > 2 * borderSize && position2.x < w - 4 * borderSize;

			bool check1 = position2.x > 2 * borderRadius2;
			bool check2 = position2.x < w2 - 2 * borderRadius2;
			bool check3 = position2.y > 2 * borderRadius2;
			bool check4 = position2.y < h2 - 2 * borderRadius2;

			if (zone1 == true)
			{
				if (r1Pass2 || r2Pass2 || (check1 && check2))
				{
					color.rgba = 0;
				}
			}
			else if (zone2 == true)
			{
				if (r1Pass2 || r4Pass2 || (check3 && check4))
				{
					color.rgba = 0;
				}
			}
			else if (zone3 == true)
			{
				if (r3Pass2 || r4Pass2 || (check1 && check2))
				{
					color.rgba = 0;
				}
			}
			else if (zone4 == true)
			{
				if (r2Pass2 || r3Pass2 || (check3 && check4))
				{
					color.rgba = 0;
				}
			}
			else if (zone5 == true)
			{
				color.rgba = 0;
			}
		}
	}
	else
	{
		color.rgba = 0;
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