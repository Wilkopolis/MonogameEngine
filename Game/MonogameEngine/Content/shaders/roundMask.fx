float x;         // circle center left
float y;         // circle center top
float r;         // circle radius
float pos_x;     // our position on the screen in % 
float pos_y;     // our position on the screen in %
float screen_w;  // viewport width in pixels
float screen_h;  // viewport height in pixels
int tex_w;       // width of this texture in pixels
int tex_h;       // height of this texture in pixels
float alpha;     // alpha of the texture
sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);

    // our position in % of this pixel on the entire game window
    float2 absolute_position = float2(coords.x * tex_w/screen_w + pos_x, coords.y * tex_h/screen_h + pos_y);
    float2 absolute_center = float2(.5 * tex_w/screen_w + pos_x, .5 * tex_h/screen_h + pos_y);

    if (length(absolute_position - absolute_center) > r)
    {
        color.rgba = 0;
    }

    color.rgba *= alpha;
    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}