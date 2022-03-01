Shader "Neogoma/PointCloud" {
	Properties{
		point_size("Point Size", Float) = 5
	}
		SubShader{
		Pass {
			LOD 200

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct VertexInput {
				float4 v : POSITION;
				float4 color : COLOR;
			};

			struct VertexOutput {
				float4 pos : SV_POSITION;
				float size : PSIZE;
				float4 col : COLOR;
			};

			float point_size;

			VertexOutput vert(VertexInput v) {

				VertexOutput o;
				o.pos = UnityObjectToClipPos(v.v);
				o.size = point_size;
				o.col = v.color;

				return o;
			}

			float4 frag(VertexOutput o) : COLOR {
				return o.col;
			}

			ENDCG
			}
	}

}