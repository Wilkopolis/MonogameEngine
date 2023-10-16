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
int flipped = 0;
sampler s0;

float4 PixelShaderFunction(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = tex2D(s0, coords);

	// our position in % of this pixel on the entire game window
	float xPos = coords.x;
	if (flipped == 1)
		xPos = 1 - coords.x;

	float2 absolute_position = float2(xPos * tex_w + pos_x, coords.y * tex_h + pos_y);

	if (absolute_position.x < x || absolute_position.x > x + w || absolute_position.y < y || absolute_position.y > y + h)
	{
		color.rgba = 0;
	}

	return color;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}