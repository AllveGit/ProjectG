Shader "LSJShader/Basic Outline" 
{
	Properties 
	{
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (.002, 1)) = .005
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			cull front //Shadow Rendering
			ZWrite On
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"

			#pragma vertex vert
			#pragma fragment frag

			CBUFFER_START(UnityPerMaterial)
			float4 _OutlineColor;
			float _Outline;
			CBUFFER_END

			struct VertexInput
			{
				float4 positionOS : POSITION;
				float3 normalOS : NORMAL;
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 color : COLOR;
			};

			VertexOutput vert(VertexInput input)
			{
				VertexOutput output = (VertexOutput)0;

				input.positionOS.xyz += input.normalOS.xyz * _Outline;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				output.positionCS = vertexInput.positionCS;

				output.color = _OutlineColor;
				return output;
			}

			float4 frag(VertexOutput i) : SV_Target
			{
				return i.color;
			}
			ENDHLSL
		}
	}
}
