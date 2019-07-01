Shader "LSJShader/DefaultRender"
{
	Properties 
	{
	    [HideInInspector]_MainTex ("Base (RGB)", 2D) = "white" {}
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

			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);

			struct VertexInput
			{
				float4 positionOS       : POSITION;
				float2 uv               : TEXCOORD0;
			};

			struct VertexOutput
			{
				float2 uv        : TEXCOORD0;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			VertexOutput vert(VertexInput input)
			{
				VertexOutput output = (VertexOutput)0;
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				output.vertex = vertexInput.positionCS;
				output.uv = input.uv;

				return output;
			}

			float4 frag(VertexOutput input) : SV_Target
			{
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

				return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
			}
			ENDHLSL
		}
	} 
	FallBack "Diffuse"
}
