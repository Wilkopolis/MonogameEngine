#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// ms elapsed
float rRatio = 1;
float gRatio = 1;
float bRatio = 1;
float alpha = 1;
float noiseAmount = 1;

// local stuff
sampler samp;

float hash2( float2 p )
{
    // procedural white noise	
	return frac(sin(dot(p,float2(127.1,311.7)))*43758.5453);
}

float4 main(float2 coord : TEXCOORD) : COLOR
{
	float4 color = tex2D(samp, coord);

	color.r *= rRatio;
	color.g *= gRatio;
	color.b *= bRatio;
	color.a *= alpha;

	float noiseFactor = hash2(coord);

	color.r += noiseFactor * noiseAmount;
	color.g += noiseFactor * noiseAmount;
	color.b += noiseFactor * noiseAmount;

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}