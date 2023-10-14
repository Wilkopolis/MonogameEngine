float x;		 // rect left
float y;		 // rect top
float w;		 // rect width
float h;		 // rect height
float pos_x;	 // our position on the screen in %	
float pos_y;	 // our position on the screen in %
float screen_w;  // viewport width in pixels
float screen_h;	 // viewport height in pixels
int tex_w;	 	 // width of this texture in pixels
int tex_h;       // height of this texture in pixels
float alpha;	 // alpha of the texture
// BORDER SHIT
// int borderSize;

sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	// float2 position = float2(coords.x * tex_w, coords.y * tex_h);
	// if (position.x > borderSize && position.x < tex_w - borderSize && position.y > borderSize && position.y < tex_h - borderSize)
	// {
	// 	color.rgba = 0;
	// }

	// our position in % of this pixel on the entire game window
	float2 absolute_position = float2(coords.x * tex_w/screen_w + pos_x, coords.y * tex_h/screen_h + pos_y);

	if (absolute_position.x < x || absolute_position.x > x + w || absolute_position.y < y || absolute_position.y > y + h)
	{
		color.rgba = 0;
	}
	// else
	// {
	// 	color.rgba *= a;
	// }

	return color * alpha;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}