#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float time;
float profile;
float width;
float height;

float r = 0;
float g = 0;
float b = 0;

sampler s0;

float2 pos(float t)
{
	t += 0.005;
	if (t > 0)
		t = 0;

	if (t < -.036)
		t = -.036;

	float2 result = float2(0, 0);

	if (profile == 1)
		result = float2(-t * 20 * width / 2000.0 + .2, -sin(t * 27 * 3.141529) * .08 * height + .5);
	else if (profile == 2)
		result = float2(-t * 20 * width / 2000.0 + .2, sin(t * 27 * 3.141529) * .08 * height + .5);
	else if (profile == 3)
		result = float2(-t * 20 * width / 2000.0 + .2, sin(t * 27 * 3.141529) * .08 * height + sin(t * 54 * 3.141529) * .03 * height + .5);
	else if (profile == 4)
		result = float2(-t * 20 * width / 2000.0 + .2, -sin(t * 27 * 3.141529) * .04 * height + .5);
	else if (profile == 5)
		result = float2(-t * 20 * width / 2000.0 + .2, sin(t * 27 * 3.141529) * .04 * height + .5);
	else
		result = float2(-t * 20 * width / 2000.0 + .2, sin(t * 27 * 3.141529) * .08 * height - sin(t * 54 * 3.141529) * .03 * height + .5);

	return result;
}

float4 main(float2 coords : TEXCOORD) : COLOR
{
    float t = time / 10000;
    
    float4 f = float4(0, 0, 0, 0);
    
	float4 c1 = float4(r * .6, g * .6, b * .6, .8);

	// float4 c1 = float4(.6 * .6, .4 * .6, .7 * .6, .8);
	
	int steps = 24;
	for (int i = 0; i < steps; i++)
    {
    	float localT = -t - i * .0002;
    	float d = .01 / length(pos(localT) - coords);
    	float4 c = float4(d - .2, d - .2, d - .2, d - .2);
	    
		f = lerp(c, f, .9);
    }
    
    f *= c1;

    return float4(f.x, f.y, f.z, f.a);    
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}