#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0
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

float maskOffsetX;
float maskOffsetY;
float maskWidth;
float maskHeight;
float textureWidth;
float textureHeight;

float4 main(float2 coord : TEXCOORD) : COLOR
{
    float4 diffuse = tex2D(TextureSampler, coord.xy);
    
    // screen pixel
    float2 texturePixel = float2(coord.x * textureWidth, coord.y * textureHeight);
    // mask pixel 
    float2 maskPixel = texturePixel - float2(maskOffsetX, maskOffsetY);
    // mask coords
    float2 maskCoords = float2(maskPixel.x / maskWidth, maskPixel.y / maskHeight);

    float4 mask = tex2D(MaskSampler, maskCoords.xy);

    return diffuse * mask.r;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}