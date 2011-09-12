/*********************************************************************NVMH3****
File:  $Id: //sw/devtools/FXComposer1.6/SDK/MEDIA/HLSL/post_posterize.fx#1 $

Copyright NVIDIA Corporation 2004
TO THE MAXIMUM EXTENT PERMITTED BY APPLICABLE LAW, THIS SOFTWARE IS PROVIDED
*AS IS* AND NVIDIA AND ITS SUPPLIERS DISCLAIM ALL WARRANTIES, EITHER EXPRESS
OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, IMPLIED WARRANTIES OF MERCHANTABILITY
AND FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL NVIDIA OR ITS SUPPLIERS
BE LIABLE FOR ANY SPECIAL, INCIDENTAL, INDIRECT, OR CONSEQUENTIAL DAMAGES
WHATSOEVER (INCLUDING, WITHOUT LIMITATION, DAMAGES FOR LOSS OF BUSINESS PROFITS,
BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION, OR ANY OTHER PECUNIARY LOSS)
ARISING OUT OF THE USE OF OR INABILITY TO USE THIS SOFTWARE, EVEN IF NVIDIA HAS
BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.

Reduce color space

******************************************************************************/

float Script : STANDARDSGLOBAL <
	string UIWidget = "none";
	string ScriptClass = "scene";
	string ScriptOrder = "postprocess";
	string ScriptOutput = "color";
	string Script = "Technique=poster;";
> = 0.8; // version #

#include "Quad.fxh"

float4 ClearColor : DIFFUSE = {0.3,0.3,0.3,1.0};
float ClearDepth
<
	string UIWidget = "none";
> = 1.0;

float nColors <
	string UIWidget = "slider";
	string UIName = "# colors";
	float UIMin = 2.0;
	float UIMax = 255.0;
	float UIStep = 1.0;
> = 16;

float gamma <
	string UIWidget = "slider";
	string UIName = "Gamma";
	float UIMin = 0.1;
	float UIMax = 10.0;
	float UIStep = 0.1;
> = 1.0;

///////////////////////////////////////////////////////////
///////////////////////////// Render-to-Texture Data //////
///////////////////////////////////////////////////////////

DECLARE_QUAD_TEX(SceneTexture,SceneSampler,"A8R8G8B8")
DECLARE_QUAD_DEPTH_BUFFER(DepthBuffer, "D24S8")

////////////////////////////////////////////////////////////
/////////////////////////////////////// Shader /////////////
////////////////////////////////////////////////////////////

QUAD_REAL4 posterPS(QuadVertexOutput IN) : COLOR
{   
	QUAD_REAL4 texCol = tex2D(SceneSampler, IN.UV);
	QUAD_REAL3 tc = texCol.xyz;
	tc = pow(tc, gamma);
	tc = tc * nColors;
	tc = floor(tc);
	tc = tc / nColors;
	tc = pow(tc,1.0/gamma);
	return float4(tc,texCol.w);
}  

////////////////////////////////////////////////////////////
/////////////////////////////////////// techniques /////////
////////////////////////////////////////////////////////////

technique poster <
	string Script =
			"RenderColorTarget0=SceneTexture;"
	        "RenderDepthStencilTarget=DepthBuffer;"
	        "ClearSetColor=ClearColor;"
	        "ClearSetDepth=ClearDepth;"
   			"Clear=Color;"
			"Clear=Depth;"
			"ScriptExternal=color;"
        	"Pass=p0;";
> {
    pass p0 <
    	string Script = "RenderColorTarget0=;"
	        			"RenderDepthStencilTarget=;"
						"Draw=Buffer;";
    > {
		cullmode = none;
		ZEnable = false;
		ZWriteEnable = false;
		VertexShader = compile vs_2_0 ScreenQuadVS();
		PixelShader = compile ps_2_0 posterPS();
    }
}

////////////// eof ///
