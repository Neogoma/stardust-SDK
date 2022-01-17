    Shader "com.Neogoma/Hidden"
    {
		//These are all the properties that our deaaaar designer Deng can modify on the fly
        Properties
        {
			//DENG: Use this to select which stencer value to use
			[IntRange] _StencilRef ("Stencil Reference Value", Range(0,255)) = 0
			_Color ("Tint", Color) = (0, 0, 0, 1)
            _Albedo ("Albedo", 2D) = "white" {}
            [NoScaleOffset]            
			_Metallic("Metallic", Range(0,1)) = 0.0
			_MetallicGlossMap ("Metallic", 2D) = "black" {}
			_Glossiness("Smoothness", Range(0,1)) = 0.5
			_Emission("Emission", 2D) = "black" {}
			[HDR] _Emission ("Emission", color) = (0,0,0)

            [NoScaleOffset]
            _Normal ("Normal", 2D) = "bump" {}
        }
     
        SubShader
        {
			//FOR DENG: If you want to use your sexy shaders of the death who kills copy from here
            Tags
            {
                "Queue" = "Geometry"
                "RenderType" = "Opaque"
            }
			LOD 200
			
			Stencil{
					Ref [_StencilRef]
					Comp Equal
			}
			//FOR DENG: STOP COPYING FROM HERE

            CGINCLUDE
            #define _GLOSSYENV 1
            ENDCG
         
            CGPROGRAM
            #pragma target 3.0
            #include "UnityPBSLighting.cginc"
            #pragma surface surf Standard
            #pragma exclude_renderers gles
     
            struct Input
            {
                float2 uv_Albedo;
            };
     
	 		fixed4 _Color;
            sampler2D _Albedo;
            sampler2D _Normal;
            sampler2D _MetallicGlossMap;
			half _Glossiness;
			half _Metallic;
			half3 _Emission;

			//STANDARD SHADER APPLICATIONS
            void surf (Input IN, inout SurfaceOutputStandard o)
            {
                fixed4 albedo = tex2D(_Albedo, IN.uv_Albedo);
                fixed3 normal = UnpackScaleNormal(tex2D(_Normal, IN.uv_Albedo), 1);
				albedo *=_Color;
                o.Albedo = albedo.rgb;
				o.Alpha=albedo.a;
                o.Normal = normal;
                o.Smoothness = _Glossiness;
                o.Metallic =_Metallic;
				o.Emission = _Emission;

                
            }
            ENDCG
        }
     
        FallBack "Diffuse"
    }
