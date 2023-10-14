#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// local stuff
sampler s0;

float time = 0;
float r = 0;
float g = 0;
float b = 0;

float polygonDistance(float2 p, float radius, float angleOffset, int sideCount) {
	float a = atan2(p.y,p.x) + angleOffset;
	float b = 6.28319 / float(sideCount);
	return cos(floor(.5 + a / b) * b - a) * length(p) - radius;
}

// from https://www.shadertoy.com/view/4djSRW
#define HASHSCALE1 443.8975
float hash11(float p) // assumes p in ~0-1 range
{
	float3 p3  = frac(float3(p, p, p) * HASHSCALE1);
    p3 += dot(p3, p3.yzx + 19.19);
    return frac((p3.x + p3.y) * p3.z);
}

#define HASHSCALE3 float3(.1031, .1030, .0973)
float2 hash21(float p) // assumes p in larger integer range
{
	float3 p3 = frac(float3(p, p, p) * HASHSCALE3);
	p3 += dot(p3, p3.yzx + 19.19);
	return frac(float2((p3.x + p3.y)*p3.z, (p3.x+p3.z)*p3.y));
}

float4 main(float2 coords : TEXCOORD) : COLOR
{
	float2 uv = coords;//float2(0.5, 0.5) - (coords.xy / iResolution.xy);
    
    float accum = 0.0;
    for(int i = 0; i < 10; i++) {
        float fi = float(i);
        float thisYOffset = fmod(hash11(fi * 0.017) * (time + 19.) * 0.2, 4.0) - 2.0;
        float2 center = (hash21(fi) * 2. - 1.) * float2(1.1, 1.0) - float2(0.0, thisYOffset);
        float radius = 0.5;
        float2 offset = uv - center;
        float twistFactor = (hash11(fi * 0.0347) * 2. - 1.) * 1.9;
        float rotation = 0.1 + time * 0.2 + sin(time * 0.1) * 0.9 + (length(offset) / radius) * twistFactor;
        accum += pow(smoothstep(radius, 0.0, polygonDistance(uv - center, 0.1 + hash11(fi * 2.3) * 0.2, rotation, 5) + 0.1), 3.0);
    }
    for(i = 10; i < 20; i++) {
        float fi = float(i);
        float thisYOffset = fmod(hash11(fi * 0.017) * (time + 19.) * 0.2, 4.0) - 2.0;
        float2 center = (hash21(fi) * 2. - 1.) * float2(1.1, 1.0) - float2(0.0, thisYOffset);
        float radius = 0.5;
        float2 offset = uv - center;
        float twistFactor = (hash11(fi * 0.0347) * 2. - 1.) * 1.9;
        float rotation = 0.1 + time * 0.2 + sin(time * 0.1) * 0.9 + (length(offset) / radius) * twistFactor;
        accum += pow(smoothstep(radius, 0.0, polygonDistance(uv - center, 0.1 + hash11(fi * 2.3) * 0.2, rotation, 5) + 0.1), 3.0);
    }
    for(i = 20; i < 30; i++) {
        float fi = float(i);
        float thisYOffset = fmod(hash11(fi * 0.017) * (time + 19.) * 0.2, 4.0) - 2.0;
        float2 center = (hash21(fi) * 2. - 1.) * float2(1.1, 1.0) - float2(0.0, thisYOffset);
        float radius = 0.5;
        float2 offset = uv - center;
        float twistFactor = (hash11(fi * 0.0347) * 2. - 1.) * 1.9;
        float rotation = 0.1 + time * 0.2 + sin(time * 0.1) * 0.9 + (length(offset) / radius) * twistFactor;
        accum += pow(smoothstep(radius, 0.0, polygonDistance(uv - center, 0.1 + hash11(fi * 2.3) * 0.2, rotation, 5) + 0.1), 3.0);
    }
    
    float3 subColor = float3(r, g, b); //float3(0.4, 0.2, 0.8);
    float3 addColor = float3(0, 0, 0);//float3(0.3, 0.1, 0.2);
    
    accum = accum > 1 ? 1 : accum;

	float4 color = float4(float3(0, 0, 0) + accum * subColor + addColor, 1);
	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}