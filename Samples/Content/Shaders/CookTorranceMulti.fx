/*********************************************************************NVMH3****
File:  $Id: //sw/devtools/FXComposer1.6/SDK/MEDIA/HLSL/CookTorranceMulti.fx#1 $

Copyright NVIDIA Corporation 2002
TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, THIS SOFTWARE IS PROVIDED
*AS IS* AND NVIDIA AND ITS SUPPLIERS DISCLAIM ALL WARRANTIES, EITHER EXPRESS
OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL NVIDIA OR ITS SUPPLIERS
BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT, OR CONSEQUENTIAL DAMAGES
WHATSOEVER (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS,
BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION, OR ANY OTHER PECUNIARY LOSS)
ARISING OUT OF THE USE OF OR INABILITY TO USE THIS SOFTWARE, EVEN IF NVIDIA HAS
BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.


Comments:
	Shading via multitexture. Two textures are interpolated over the
	surface, and their product results in the final specular BDRF. This
	texture lookup is driven by vertex-shaded factors, so DX8 is more
	than capable of performing the math.
	    The initial textures supplied approximate a Cook-Torrance model
	using one set of possible parameters, but different textures can
	be used to emulate a wide variety of isotropic BRDF models.
	    In this example lambertian diffuse is still supplied, but it is
	not required.

******************************************************************************/

float Script : STANDARDSGLOBAL <
    string UIWidget = "none";
    string ScriptClass = "object";
    string ScriptOrder = "standard";
    string ScriptOutput = "color";
    string Script = "Technique=Technique?Untextured:Textured;";
> = 0.8;

/************* UN-TWEAKABLES ***********/

float4x4 WorldIT : WorldInverseTranspose < string UIWidget="None"; >;
float4x4 WorldViewProj : WorldViewProjection < string UIWidget="None"; >;
float4x4 World : World < string UIWidget="None"; >;
float4x4 ViewInv : ViewInverse < string UIWidget="None"; >;

/************* TWEAKABLES **************/

float4 LightPos : Position <
    string Object = "PointLight";
    string Space = "World";
    string UIName = "Lamp Position";
> = {100.0f, 100.0f, -100.0f, 0.0f};

float4 LightColor <
    string UIName =  "Lamp";
    string UIWidget = "Color";
> = {1.0f, 1.0f, 1.0f, 1.0f};

float4 AmbiColor : Ambient
<
    string UIName =  "Ambient";
> = {0.1f, 0.1f, 0.1f, 1.0f};

float4 SurfColor <
    string UIName =  "Surface";
    string UIWidget = "Color";
> = {0.5f, 0.2f, 0.1f, 1.0f};

//////////////////////////////
// Textures //////////////////
//////////////////////////////

texture halfAngleMap
<
    string ResourceName = "ctHalf.dds";
    string ResourceType = "2D";
    string UIName =  "Map with dot-half-angle factors";
>;

sampler2D hmapSampler = sampler_state
{
	Texture = <halfAngleMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = None;
	AddressU = Clamp;
	AddressV = Clamp;
};

texture normalAngleMap
<
    string ResourceName = "ctNorm.dds";
    string ResourceType = "2D";
    string UIName =  "Map with dot-normal factors";
>;

sampler2D nmapSampler = sampler_state
{
	Texture = <normalAngleMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = None;
	AddressU = Clamp;
	AddressV = Clamp;
};

texture colorTexture : DIFFUSE
<
    string ResourceName = "default_color.dds";
    string ResourceType = "2D";
>;

sampler2D cmapSampler = sampler_state
{
	Texture = <colorTexture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

/************* DATA STRUCTS **************/

/* data from application vertex buffer */
struct appdata {
    float3 Position	: POSITION;
    float4 UV		: TEXCOORD0;
    float4 Normal	: NORMAL;
};

/* data passed from vertex shader to pixel shader */
struct vertexOutput {
    float4 HPosition	: POSITION;
    float4 TexCoord0	: TEXCOORD0; // dot prods against half-angle
    float4 TexCoord1	: TEXCOORD1; // dot-prods against normal
    float4 TexCoord2	: TEXCOORD2; // plain-ol' UV
    float4 diffCol	: COLOR0;
};

/*********** vertex shader ******/

vertexOutput cookTorrMultVS(appdata IN) {
    vertexOutput OUT;
    float3 worldNormal = mul(IN.Normal, WorldIT).xyz;
    worldNormal = normalize(worldNormal);

    //build float4
    float4 tempPos;
    tempPos.xyz = IN.Position.xyz;
    tempPos.w   = 1.0;

    //compute worldspace position
    float3 worldSpacePos = mul(tempPos, World).xyz;
    float3 LightVec = normalize(LightPos - worldSpacePos);
    float ldn = dot(LightVec,worldNormal);
    float diffComp = max(0,ldn);
    float4 diffContrib = SurfColor * ( diffComp * LightColor + AmbiColor);

    OUT.diffCol = diffContrib;
    OUT.diffCol.w = 1.0;
    OUT.TexCoord2 = IN.UV;

    float3 EyePos = ViewInv[3].xyz;
    float3 vertToEye = normalize(EyePos - worldSpacePos);
    float3 halfAngle = normalize(vertToEye + LightVec);
    float4 halfIndices = float4(0.5+dot(LightVec,halfAngle)/2.0,
			   1.0 - (0.5+dot(worldNormal,halfAngle)/2.0),0.0,1.0);
    float4 normIndices = float4(0.5+dot(LightVec,worldNormal)/2.0,
			   1.0 - (0.5+dot(worldNormal,vertToEye)/2.0),0.0,1.0);
    OUT.TexCoord0 = halfIndices;
    OUT.TexCoord1 = normIndices;

    // transform into homogeneous-clip space
    OUT.HPosition = mul(tempPos, WorldViewProj);
    return OUT;
}

/********* pixel shader ********/

float4 cookTorrMultPS(vertexOutput IN) : COLOR {
    float4 nspec = tex2D(hmapSampler,IN.TexCoord0.xy) *
		   tex2D(nmapSampler,IN.TexCoord1.xy) * LightColor;
    return (IN.diffCol + nspec);
}

float4 cookTorrMultPS_t(vertexOutput IN) : COLOR {
    float4 nspec = tex2D(hmapSampler,IN.TexCoord0.xy) *
		   tex2D(nmapSampler,IN.TexCoord1.xy) * LightColor;
    float4 ndiff = tex2D(cmapSampler,IN.TexCoord2.xy) * IN.diffCol;
    return (ndiff + nspec);
}

/*************/

technique Untextured <
	string Script = "Pass=p0;";
> {
    pass p0  <
		string Script = "Draw=geometry;";
    > {		
		VertexShader = compile vs_1_1 cookTorrMultVS();
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = None;
		PixelShader = compile ps_1_1 cookTorrMultPS();
    }
}

technique Textured <
	string Script = "Pass=p0;";
> {
    pass p0  <
		string Script = "Draw=geometry;";
    > {		
		VertexShader = compile vs_1_1 cookTorrMultVS();
		ZEnable = true;
		ZWriteEnable = true;
		CullMode = None;
		PixelShader = compile ps_1_1 cookTorrMultPS_t();
    }
}

/***************************** eof ***/
