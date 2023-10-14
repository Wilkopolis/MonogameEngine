#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// ms elapsed
float time;

// how intense the color should be overall
float alpha;

// target color
float colorr;
float colorg;
float colorb;

// points where we gradiate from
float2 p[12];
// radius of each point
float r[12];

// starting time offsets
static float dt[12] =
{
	42000,
	24000,
	18000,
	36000,
	12000,
	6000,
	8000,
	30000,
	16000,
	48000,
	24000,
	54000
};

static float rt[12] =
{
	1,
	.8,
	1,
	.6,
	.8,
	1.2,
	.8,
	1,
	1.4,
	.8,
	1.2,
	1
};

// local stuff
sampler s0;

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 color = tex2D(s0, uv);

	for(uint i = 0; i < 12; i++)
	{
		float2 center = float2(p[i].x, p[i].y);
		float q = length(uv - center);
		float diff = r[i] - q;
		// float diff2 = r[i] - q;


		diff *= (sin(((time * rt[i]) + dt[i]) / 1000) + .75) * .8 + .2;

		if (diff < 0)
			diff = 0;
		// if (diff2 < 0)
		// 	diff2 = 0;

		color.r += colorr * diff;
		color.g += colorg * diff;
		color.b += colorb * diff;
		color.a += alpha * diff;
	}

	//color.rgba *= alpha;

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}