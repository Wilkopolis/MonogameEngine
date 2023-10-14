#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler TextureSampler : register(s0);
Texture2D Mask;
sampler MaskSampler {
    Texture = ( Mask );
    MagFilter = LINEAR;
    MinFilter = LINEAR;
    Mipfilter = LINEAR;
    AddressU = CLAMP;
    AddressV = CLAMP;
};

float4 main(float2 coord : TEXCOORD) : COLOR
{
    float4 diffuse = tex2D(TextureSampler, coord.xy);
    float4 mask = tex2D(MaskSampler, coord.xy);

    return diffuse * mask.r;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}