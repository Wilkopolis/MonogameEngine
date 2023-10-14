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

// global alpha
float alpha;

sampler samp;

static const float PI = 3.14159265f;

float4 main(float2 coord : TEXCOORD) : COLOR
{
	float4 color = tex2D(samp, coord);
	float x = coord.x + time * .00002;
	float factor = sin(x * 2 * PI * 1.5) * .04 + .5;
	factor += sin(x * 2 * PI * 3.7) * .02;
	factor += sin((x + .6) * 2 * PI * 2.7) * .02;
	factor += sin(x * 2 * 2 * PI * .47) * .01;
	factor += sin(x * 2 * PI * 7) * .005;

	// move it up the screen
	factor -= .35;
    
    if (coord.y < factor)
    {
    	color.rgba = 0;
    }
	else
	{
		float dx = min(.05, (coord.y - factor)) / .05;
		float fac = sin(dx * PI * .5);

		color.rgba *= min(1, pow(fac, 2));
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