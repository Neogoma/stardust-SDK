Shader "Custom/animated" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("FarLayer ", 2D) = "white" {}
		_DetailTex("NearLayer ", 2D) = "white" {}
		_ScrollX("Far layer scroll Speed",Float) = 1.0
		_Scroll2X("Near layer scroll Speed",Float) = 1.0
		_Multiplier("Layer Multiplier",Float) = 1.0
	}
		SubShader
		{
			Tags{"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
			Pass
			{
				Tags{"LightMode" = "ForwardBase"}
				ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#include "UnityCG.cginc"
				#pragma multi_compile_fwdbase
				#pragma vertex vert
				#pragma fragment frag

				fixed4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _DetailTex;
				float4 _DetailTex_ST;
				float _ScrollX;
				float _Scroll2X;
				float _Multiplier;


				struct a2v
				{
					float4 vertex : POSITION;
					float4 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 pos : SV_POSITION;
					float4 uv : TEXCOORD0;
				};

				v2f vert(a2v v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv.xy = TRANSFORM_TEX(v.texcoord, _MainTex) + frac(float2(0.0, -_ScrollX) * _Time.y);
					o.uv.zw = TRANSFORM_TEX(v.texcoord, _DetailTex) + frac(float2(0.0, -_Scroll2X) * _Time.y);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 firstLayer = tex2D(_MainTex,i.uv.xy);
					fixed4 secondLayer = tex2D(_DetailTex, i.uv.xy);
					fixed4 c = lerp(firstLayer, secondLayer, secondLayer.a);
					c.rgb *= _Multiplier;
					c.rgb *= _Color.rgb;
					return c;
				}

				ENDCG
			}
		}
			FallBack "VertexLit"
}