Shader "com.Neogoma/ScanEffect"
{
		Properties{
			_MainTex("Base (RGB)", 2D) = "white" {}									
			_Threeshold("Threeshold", Float) = 0.1
			_EdgeColor("Edge Color", Color) = (1,1,1,1)
			_EdgeColor2("Edge Color 2", Color) = (0,0,1,1)
			_EnableScan("EnableScan", Float) = 1
			_Range("Neighboor range search", Vector) = (1,1,1,1)

		}
			SubShader{
			Pass{
			CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform float _bwBlend;
		float4 _Range;
		float _EnableScan;
		float _Threeshold;
		float4 _EdgeColor;
		float4 _EdgeColor2;
	

		//Sobel function to find borders
			float sobel(sampler2D tex, float2 uv) {

			//Filter horizontal
			float4 hr = float4(0, 0, 0, 0);

			//Filter vertical
			float4 vt = float4(0, 0, 0, 0);

			//Calculate the horizontal matrix
			hr += tex2D(tex, (uv + float2(-1.0, -1.0) * _Range)) *  1.0;
			hr += tex2D(tex, (uv + float2(1.0, -1.0) * _Range)) * -1.0;
			hr += tex2D(tex, (uv + float2(-1.0, 0.0) * _Range)) *  2.0;
			hr += tex2D(tex, (uv + float2(1.0, 0.0) * _Range)) * -2.0;
			hr += tex2D(tex, (uv + float2(-1.0, 1.0) * _Range)) *  1.0;
			hr += tex2D(tex, (uv + float2(1.0, 1.0) * _Range)) * -1.0;

			//Calculate the vertical matrix
			vt += tex2D(tex, (uv + float2(-1.0, -1.0) * _Range)) *  1.0;
			vt += tex2D(tex, (uv + float2(0.0, -1.0) * _Range)) *  2.0;
			vt += tex2D(tex, (uv + float2(1.0, -1.0) * _Range)) *  1.0;
			vt += tex2D(tex, (uv + float2(-1.0, 1.0) * _Range)) * -1.0;
			vt += tex2D(tex, (uv + float2(0.0, 1.0) * _Range)) * -2.0;
			vt += tex2D(tex, (uv + float2(1.0, 1.0) * _Range)) * -1.0;

			return sqrt(hr * hr + vt * vt);
		}

			float4 frag(v2f_img i) : COLOR{

				
				
			if (_EnableScan>0) {
				float s = sobel(_MainTex, i.uv);
				float4 finalComputeColor = lerp(_EdgeColor, _EdgeColor2, 1 - abs(_SinTime.w));
				//If s above threeshold then use the color
				if (s > _Threeshold)
					return finalComputeColor;
				else
					return tex2D(_MainTex, i.uv);
			}
			else
				return tex2D(_MainTex, i.uv);

		}
			ENDCG
		}
		}

}
