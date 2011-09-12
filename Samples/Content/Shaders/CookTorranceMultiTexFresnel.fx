/*********************************************************************NVMH3****
File:  $Id: //sw/devtools/FXComposer1.6/SDK/MEDIA/HLSL/CookTorranceMultiTexFresnel.fx#1 $

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
	surface, and their product results in the final specular BDRF.
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
    string Script = "Technique=ps11;";
> = 0.8;

/************* TWEAKABLES **************/

float4x4 WorldIT : WorldInverseTranspose < string UIWidget="None"; >;
float4x4 WorldViewProj : WorldViewProjection < string UIWidget="None"; >;
float4x4 World : World < string UIWidget="None"; >;
float4x4 ViewInv : ViewInverse < string UIWidget="None"; >;

///////////////

float4 LightPos : Position
<
    string Object = "PointLight";
    string Space = "World";
> = {100.0f, 100.0f, -100.0f, 0.0f};

float4 LightColor
<
    string UIName =  "Light Color";
    string UIWidget = "Color";
> = {0.8f, 0.8f, 1.0f, 1.0f};

float4 AmbiColor : Ambient
<
    string UIName =  "Ambient Light Color";
    string UIWidget = "Color";
> = {0.1f, 0.1f, 0.1f, 1.0f};

float4 SurfColor : DIFFUSE
<
    string UIName =  "Surface Color";
    string UIWidget = "Color";
> = {1.0f, 1.0f, 1.0f, 1.0f};

float Ks
<
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 1.0;
    float UIStep = 0.05;
    string UIName =  "Hilight Strength";
> = 0.5;

float Kr
<
    string UIWidget = "slider";
    float UIMin = 0.0;
    float UIMax = 1.0;
    float UIStep = 0.05;
    string UIName =  "Fresnel Strength";
> = 0.6;

float FresExp
<
    string UIWidget = "slider";
    float UIMin = 3.0;
    float UIMax = 5.0;
    float UIStep = 0.1;
    string UIName =  "Fresnel Expon";
> = 5.0;

//////////////

texture halfAngleMap
<
    string ResourceName = "ctHalf.dds";
    string ResourceType = "2D";
    string UIName =  "Map with dot-half-angle factors";
>;

texture normalAngleMap
<
    string ResourceName = "ctNorm.dds";
    string ResourceType = "2D";
    string UIName =  "Map with dot-normal factors";
>;

texture colorTexture : DIFFUSE
<
    string ResourceName = "default_color.dds";
    string ResourceType = "2D";
>;

texture cubeMap : ENVIRONMENT
<
    string ResourceName = "default_reflection.dds";
    string ResourceType = "Cube";
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

sampler2D nmapSampler = sampler_state
{
	Texture = <normalAngleMap>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = None;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D cmapSampler = sampler_state
{
	Texture = <colorTexture>;
	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
};

samplerCUBE emapSampler = sampler_state
{
    Texture = <cubeMap>;
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
    float4 specCol	: COLOR1;
};

/*********** vertex shader ******/

vertexOutput cookTorrMultVS(appdata IN) {
    vertexOutput OUT;
    float3 worldNormal = mul(IN.Normal, WorldIT).xyz;
    worldNormal = normalize(worldNormal);

    //build float4
	float4 Po = float4(IN.Position.xyz,1.0);

    //compute worldspace position
    float3 worldSpacePos = mul(Po, World).xyz;
    float3 LightVec = normalize(LightPos - worldSpacePos);
    float ldn = dot(LightVec,worldNormal);
    float diffComp = max(0,ldn);
    float4 diffContrib = SurfColor * diffComp * LightColor;

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
    OUT.specCol=float4(0,0,0,1);

    // transform into homogeneous-clip space
    OUT.HPosition = mul(Po, WorldViewProj);
    return OUT;
}

/********* pixel shader ********/

float4 cookTorrMultPS_t(vertexOutput IN) : COLOR {
    float4 nspec = tex2D(hmapSampler,IN.TexCoord0.xy) *
				   tex2D(nmapSampler,IN.TexCoord1.xy) *
				   Ks * LightColor;
    float4 ndiff = tex2D(cmapSampler,IN.TexCoord2.xy) * IN.diffCol;
    return (ndiff + nspec);
}

/********* fresnel term shaders ******************/

vertexOutput fresVS(appdata IN) {
    vertexOutput OUT;
    float3 worldNormal = mul(IN.Normal, WorldIT).xyz;
    worldNormal = normalize(worldNormal);
    float4 Po = float4(IN.Position.xyz,1);
    //compute worldspace position
    float3 worldSpacePos = mul(Po, World).xyz;
    float3 EyePos = ViewInv[3].xyz;
    float3 vertToEye = normalize(worldSpacePos - EyePos);
    float fake = abs(dot(vertToEye,worldNormal));
    fake = Kr * (1.0 - pow(fake,FresExp));
    OUT.TexCoord0.xyz = reflect(vertToEye,worldNormal);
    OUT.TexCoord0.w = 1.0;
    OUT.TexCoord1 = IN.UV;
    OUT.TexCoord2 = IN.UV;
    OUT.diffCol = SurfColor * AmbiColor;
    OUT.specCol = float4(fake.xxx,1.0);
    OUT.HPosition = mul(Po, WorldViewProj);
    return OUT;
}

float4 fresPS_t(vertexOutput IN) : COLOR
{
    float4 env = texCUBE(emapSampler,IN.TexCoord0.xyz);
    return (IN.diffCol * tex2D(cmapSampler,IN.TexCoord1.xy) + IN.specCol * env);
}

/*************/

////////////////

technique ps11 <
	string Script = "Pass=envPass; Pass=p0;";
> {
    // shared lighting: ambient, environment, and Z
    pass envPass  <
		string Script = "Draw=geometry;";
    > {		
	VertexShader = compile vs_1_1 fresVS();
	ZEnable = true;
	ZWriteEnable = true;
	CullMode = None;
	PixelShader = compile ps_1_1 fresPS_t();
    }
    // pass for each lamp (repeat for multiple lamps)
    pass p0  <
		string Script = "Draw=geometry;";
    > {		
	VertexShader = compile vs_1_1 cookTorrMultVS();
	ZEnable = true;
	ZWriteEnable = false;
	ZFunc = LessEqual;
	CullMode = None;
	AlphaBlendEnable = true;
	SrcBlend = One;
	DestBlend = One;
	PixelShader = compile ps_1_1 cookTorrMultPS_t();
    }
}

/***************************** eof ***/
