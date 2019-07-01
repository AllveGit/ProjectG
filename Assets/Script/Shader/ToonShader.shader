Shader "LSJShader/ToonShader"
{
    Properties
    {
		_MainTex("Albedo (RGB)", 2D) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

		cull front

		//1Pass
        CGPROGRAM
       
        #pragma surface surf Nolight vertex:vert noshadow noambient

        #pragma target 3.0

		void vert(inout appdata_full v)
		{
			v.vertex.xyz = v.vertex.xyz + v.normal.xyz * 0.09f;
		}

        struct Input
        {
			float4 color : COLOR;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            //o.Albedo = c.rgb;
            //o.Alpha = c.a;
        }

		float4 LightingNolight(SurfaceOutput s, float3 lightDir, float atten)
		{
			return float4(0, 0, 0, 1);
		}
        ENDCG

		cull back
		//2Pass
		CGPROGRAM

		#pragma surface surf Lambert

		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
    }
    FallBack "Diffuse"
}
