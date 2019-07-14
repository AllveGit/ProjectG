Shader "LSJShader/NoTexRender"
{
	Properties 
	{
	    [HideInInspector]_Color ("Base (RGB)", Color) = (1,1,1,1) 
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			cull back //Default Rendering
			HLSLPROGRAM
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/SurfaceInput.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma vertex vert
			#pragma fragment frag

			float4 _Color;

			struct VertexInput
			{
				float4 positionOS       : POSITION;
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			VertexOutput vert(VertexInput input)
			{
				VertexOutput output = (VertexOutput)0;
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				output.vertex = vertexInput.positionCS;

				return output;
			}

			float4 frag(VertexOutput input) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				return _Color;
			}
			ENDHLSL
		}
	} 
	FallBack "Diffuse"
}
