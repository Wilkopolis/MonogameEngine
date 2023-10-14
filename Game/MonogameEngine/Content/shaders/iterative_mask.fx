#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define STEPS 12

float2 vertices [48];
int vertCount = 0;

float distanceFromLine(float4 seg1, float2 p)  
{
   if ((seg1.x-seg1.z)*(p.x-seg1.z)+(seg1.y-seg1.w)*(p.y-seg1.w) <= 0)
      return sqrt((p.x - seg1.z) * (p.x - seg1.z) + (p.y - seg1.w) * (p.y - seg1.w));

   if ((seg1.z-seg1.x)*(p.x-seg1.x)+(seg1.w-seg1.y)*(p.y-seg1.y) <= 0)
      return sqrt((p.x - seg1.x) * (p.x - seg1.x) + (p.y - seg1.y) * (p.y - seg1.y));

 	return abs((seg1.w - seg1.y)*p.x - (seg1.z - seg1.x)*p.y + seg1.z*seg1.y - seg1.w*seg1.x) /
   	sqrt((seg1.y - seg1.w) * (seg1.y - seg1.w) + (seg1.x - seg1.z) * (seg1.x - seg1.z));
}

float distanceFromNearestEdge(float2 coord)
{
	float minDist = 1;
	for (int i = 0; i < vertCount; i++)
	{
		// check if we've reached the end
		int indexA = i;
		int indexB = i + 1;
		if (indexB == vertCount)
			indexB = 0;

		float2 a = vertices[indexA];
		float2 b = vertices[indexB];

        minDist = min(minDist, distanceFromLine(float4(a.x, a.y, b.x, b.y), coord));
	}
	return minDist;
}

bool isPointInPolygon(float2 coord)
{
	bool inside = false;
	for (int i = 0; i < vertCount; i++)
	{
		// check if we've reached the end
		int indexA = i;
		int indexB = i + 1;
		if (indexB == vertCount)
			indexB = 0;

		float2 a = vertices[indexA];
		float2 b = vertices[indexB];

        if ((a.y > coord.y) != (b.y > coord.y) && coord.x < (b.x - a.x) * (coord.y - a.y) / (b.y - a.y) + a.x)
        {
            inside = !inside;
        }
	}
	return inside;
}

// local stuff
sampler samp;

float4 main(float2 coord : TEXCOORD) : COLOR
{
	float4 color = tex2D(samp, coord);    
    
	// inside of polygon
	if (isPointInPolygon(coord)) 
	{
		// distance from nearest segment
		float dist = distanceFromNearestEdge(coord);

		float factor = max(0, min(1, (.06 - dist) / .06));

		color.rgba = 1 - pow(factor, 2.8);
	}
	else
	{
		color.a = 0;
	}

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}