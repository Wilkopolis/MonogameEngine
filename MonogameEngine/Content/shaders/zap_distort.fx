#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_5_0
#define PS_SHADERMODEL ps_5_0
#endif

sampler s0;

float p1 = 0;
float p2 = 0;
float p3 = 0;
float p4 = 0;
float p5 = 0;

float q1 = 0;
float q2 = 0;
float q3 = 0;
float q4 = 0;
float q5 = 0;

float offset = 0;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float pi = 3.14159265359 * 2;

    float yFactor = 0;

    float x = coords.x + offset;

    yFactor = sin(p1 * x * pi) * q1 + sin(p2 * x * pi) * q2 + sin(p3 * x * pi) * q3 + sin(p4 * x * pi) * q4 + sin(p5 * x * pi) * q5;

    if (coords.x < .3)
        yFactor *= (coords.x - .2) / .1;
    else if (coords.x > .7)
        yFactor *= (1 - coords.x - .2) / .1;

    float2 uv = float2(coords.x, coords.y + .2 * yFactor);

    float4 color = tex2D(s0, uv);

    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
    }
}