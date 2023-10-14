#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

#define STEPS 12

// ms elapsed
float time;

// local stuff
sampler samp;

// float2 vertices1 [48];
// float2 vertices2 [48];
// float2 vertices3 [48];
// float2 vertices4 [48];
// float vertCounts [4];
// int polygons;

//Noise functions from IQ and other sources that I don't exactly remember. However, not my own work.
float noise3D(float3 p)
{
	// make some noise
	return frac(sin(dot(p, float3(12.9898,78.233,128.852))) * 43758.5453)*2.0-1.0;
}

float simplex3D(float3 p)
{
	float f3 = 1.0/3.0;
	float s = (p.x+p.y+p.z)*f3;
	int i = int(floor(p.x+s));
	int j = int(floor(p.y+s));
	int k = int(floor(p.z+s));
	
	float g3 = 1.0/6.0;
	float t = float((i+j+k))*g3;
	float x0 = float(i)-t;
	float y0 = float(j)-t;
	float z0 = float(k)-t;
	x0 = p.x-x0;
	y0 = p.y-y0;
	z0 = p.z-z0;
	
	int i1,j1,k1;
	int i2,j2,k2;
	
	if(x0>=y0)
	{
		if(y0>=z0){ i1=1; j1=0; k1=0; i2=1; j2=1; k2=0; } // X Y Z order
		else if(x0>=z0){ i1=1; j1=0; k1=0; i2=1; j2=0; k2=1; } // X Z Y order
		else { i1=0; j1=0; k1=1; i2=1; j2=0; k2=1; }  // Z X Z order
	}
	else 
	{ 
		if(y0<z0) { i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; } // Z Y X order
		else if(x0<z0) { i1=0; j1=1; k1=0; i2=0; j2=1; k2=1; } // Y Z X order
		else { i1=0; j1=1; k1=0; i2=1; j2=1; k2=0; } // Y X Z order
	}
	
	float x1 = x0 - float(i1) + g3; 
	float y1 = y0 - float(j1) + g3;
	float z1 = z0 - float(k1) + g3;
	float x2 = x0 - float(i2) + 2.0*g3; 
	float y2 = y0 - float(j2) + 2.0*g3;
	float z2 = z0 - float(k2) + 2.0*g3;
	float x3 = x0 - 1.0 + 3.0*g3; 
	float y3 = y0 - 1.0 + 3.0*g3;
	float z3 = z0 - 1.0 + 3.0*g3;	
				 
	float3 ijk0 = float3(i,j,k);
	float3 ijk1 = float3(i+i1,j+j1,k+k1);	
	float3 ijk2 = float3(i+i2,j+j2,k+k2);
	float3 ijk3 = float3(i+1,j+1,k+1);	
            
	float3 gr0 = normalize(float3(noise3D(ijk0),noise3D(ijk0*2.01),noise3D(ijk0*2.02)));
	float3 gr1 = normalize(float3(noise3D(ijk1),noise3D(ijk1*2.01),noise3D(ijk1*2.02)));
	float3 gr2 = normalize(float3(noise3D(ijk2),noise3D(ijk2*2.01),noise3D(ijk2*2.02)));
	float3 gr3 = normalize(float3(noise3D(ijk3),noise3D(ijk3*2.01),noise3D(ijk3*2.02)));
	
	float n0 = 0.0;
	float n1 = 0.0;
	float n2 = 0.0;
	float n3 = 0.0;

	float t0 = 0.5 - x0*x0 - y0*y0 - z0*z0;
	if(t0>=0.0)
	{
		t0*=t0;
		n0 = t0 * t0 * dot(gr0, float3(x0, y0, z0));
	}
	float t1 = 0.5 - x1*x1 - y1*y1 - z1*z1;
	if(t1>=0.0)
	{
		t1*=t1;
		n1 = t1 * t1 * dot(gr1, float3(x1, y1, z1));
	}
	float t2 = 0.5 - x2*x2 - y2*y2 - z2*z2;
	if(t2>=0.0)
	{
		t2 *= t2;
		n2 = t2 * t2 * dot(gr2, float3(x2, y2, z2));
	}
	float t3 = 0.5 - x3*x3 - y3*y3 - z3*z3;
	if(t3>=0.0)
	{
		t3 *= t3;
		n3 = t3 * t3 * dot(gr3, float3(x3, y3, z3));
	}
	return 96.0*(n0+n1+n2+n3);	
}

float fbm(float3 p)
{
	float f;
   f  = 0.50000*simplex3D( p ); p = p*2.01;
   f += 0.25000*simplex3D( p ); p = p*2.02; //from iq
   f += 0.12500*simplex3D( p ); p = p*2.03;
   f += 0.06250*simplex3D( p ); p = p*2.04;
   f += 0.03125*simplex3D( p );
	return f;
}

float trace(float3 ro, float3 rd){
    
    float t = 0.0;
    
    float3 p = ro+rd;
    
    for(int i = 0; i < STEPS; ++i){
        float d = fbm(p)*0.08;
        t += d;
        p += rd*d;
        
    }
    
    return t;
}

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

float4 main(float2 coord : TEXCOORD) : COLOR
{
	float4 color = tex2D(samp, coord);

	if (color.a == 0)
		return float4(0, 0, 0, 0);

	// our position in % of this pixel on the entire game window
	float xPos = coord.x;
	if (flipped == 1)
		xPos = 1 - coord.x;

	float2 absolute_position = float2(xPos * tex_w/screen_w + pos_x, coord.y * tex_h/screen_h + pos_y);

	if (absolute_position.x < x || absolute_position.x > x + w || absolute_position.y < y || absolute_position.y > y + h)
	{
		float2 iResolution = float2(100, 100);
		float iTime = time / 1000.0;

		// make some funky green smoke

		float2 q = coord; //.xy / iResolution.xy;
	    q = -1.0 + q * 2.0;
	    q.x *= iResolution.x / iResolution.y;
	    
	    float3 ro = float3(0.0, 0.0, iTime*0.05);
		float3 target = float3(0.0, 0.0, -1.0);

		float3 cw = normalize(target-ro);//z
		float3 cu = normalize( cross(cw, float3(0.0, 1.0, 0.0)) );//x
		float3 cv = normalize( cross(cu,cw) );//y

		// [a b c]   [j]
		// [d e f] x [k]
		// [g h i]   [l]
		//
		// [aj + bk + cl]
		// [dj + ek + fl]
		// [gj + gk + il]

		float3 hack = float3(
			cu.x * q.x + cv.x * q.y + cw.x * 1.5,
			cu.y * q.x + cv.y * q.y + cw.y * 1.5,
			cu.z * q.x + cv.z * q.y + cw.z * 1.5);

		float3 rd = normalize(hack);

	    float density = trace(ro, rd);
	    
	    float3 col = float3(0.62, 0.6, 0.7) + float3(density, density, density);
	    
		color.rgb = col.xyz;
		color.a = .75;

		color.rgb *= .6f;

		// now make a radial gradient from the edges

		float dist = length(abs(coord - float2(0.5, 0.5)));

		float factor = max(0, min(1, (.4 - dist) / .3));
		color.rgba *= pow(factor, 4);

		return color;
	}
	else
	{
		color.rgba = 0;
		return color;
	}
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL main();
	}
}