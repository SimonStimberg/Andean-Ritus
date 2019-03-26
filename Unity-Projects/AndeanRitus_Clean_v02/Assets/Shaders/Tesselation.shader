﻿SubShader 
{
${Tags}
${Blending}
${Culling}
${ZTest}
${ZWrite}

	LOD ${LOD}
	
	CGPROGRAM
	#include "Tessellation.cginc"
	#pragma target 4.6
	#pragma surface surf ${LightingFunctionName} ${VertexShaderDecl} vertex:Displacement tessellate:tessEdge tessphong:_Phong
	#pragma glsl
	#pragma debug

	
	//Tessellation Edge Function
float4 tessEdge(appdata v0, appdata v1, appdata v2)
{
	float quality = 20-(_Subdivision * 2);
	return UnityEdgeLengthBasedTess(v0.vertex, v1.vertex, v2.vertex, quality);
}

//Tessellation Vertex Function
void Displacement (inout appdata v)
{
	float d = tex2Dlod(_DisplacementTex, float4(v.texcoord.xy * (_DisplacementTex_ST.xy * float2(_WeaveScale*25, _WeaveScale*25)) + _DisplacementTex_ST.zw, 0, 0)).r * _Displacement*0.1;
	v.vertex.xyz += v.normal * (d * 0.5);
}

${ShaderFunctions}
${ShaderPropertyUsages}

	struct Input 
	{
${ShaderInputs}
	};

	void vert (inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input,o);
${VertexShaderBody}
	}
  
	void surf (Input IN, inout ${SurfaceOutputStructureName} o) 
	{
${PixelShaderBody}
	}
	ENDCG
}