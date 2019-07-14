Shader "LSJShader/GlitchShader"
{
    Properties
    {
		_GlitchAmount("GlitchAmount", Range(0,1)) = 1
		_GlitchTex("GlitchTex", 2D) = "white" {}
		_CameraOpaqueTexture("_CameraOpaqueTexture", 2D) = "white" {}
		_GlitchColor1("GlitchColor1", Color) = (1,1,1,1)
		_GlitchColor2("GlitchColor2", Color) = (1,1,1,1)
		_GlitchColor3("GlitchColor3", Color) = (1,1,1,1)

		_GlitchCutAmountX("GlitchCutAmountX", Range(0.1, 10)) = 1
		_GlitchCutAmountY("GlitchCutAmountY", Range(0.1, 10)) = 1
	}

		SubShader
		{

			Pass
			{
				Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
				LOD 200

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"


				UNITY_INSTANCING_BUFFER_START(Props)

				UNITY_INSTANCING_BUFFER_END(Props)


				sampler2D _CameraOpaqueTexture;
				sampler2D _GlitchTex;

				fixed4 _Color;

				float _GlitchAmount;
				float3 _GlitchColor1;
				float3 _GlitchColor2;
				float3 _GlitchColor3;
				float _GlitchCutAmountX;
				float _GlitchCutAmountY;

				struct VERTEXINPUT
				{
				    float4 vertex : POSITION;
				    float2 uv : TEXCOORD0;
				};

				struct v2f
				{
				    float2 uv_GlitchTex : TEXCOORD0;
				    float4 screenPos : SV_POSITION;
				};

				v2f vert (VERTEXINPUT IN)
				{
				    v2f o;
				    o.screenPos = UnityObjectToClipPos(IN.vertex);
				    o.uv_GlitchTex = IN.uv;
				    return o;
				}

				fixed4 frag (v2f IN) : SV_Target
				{
					float3 screenUV = IN.screenPos.xyz / IN.screenPos.w;

					float GlitchUV = tex2D(_GlitchTex, float2(IN.uv_GlitchTex.x * _GlitchCutAmountX + (_Time.y * 100),
						IN.uv_GlitchTex.y * _GlitchCutAmountY + sin(_Time.y * 100)));

					float UV = GlitchUV * _GlitchAmount;
					float GlitchAmountFinal = saturate(_GlitchAmount * 10);

					fixed3 r = tex2D(_CameraOpaqueTexture, float2(screenUV.x + UV, screenUV.y)).r * lerp(fixed3(1, 0, 0), _GlitchColor1, GlitchAmountFinal);
					fixed3 g = tex2D(_CameraOpaqueTexture, float2(screenUV.x - UV, screenUV.y)).g * lerp(fixed3(0, 1, 0), _GlitchColor2, GlitchAmountFinal);
					fixed3 b = tex2D(_CameraOpaqueTexture, float2(screenUV.x, screenUV.y + UV)).b * lerp(fixed3(0, 0, 1), _GlitchColor3, GlitchAmountFinal);

					float3 GlitchFinal = r + g + b;

					float3 EffectFinal = GlitchFinal;

					fixed4 col = fixed4(EffectFinal, 1);
				    return col;
				}
			ENDCG
        }
    }
}
