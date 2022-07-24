// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effect/distortion_mask" {
Properties {
	_DisplacementTex ("Displacement Tex(RG)", 2D) = "white" {}
	_MainTex("Mask(A) ", 2D) = "white" {}
	_StrengthX  ("Disp Strength X", Float) = 1
	_StrengthY  ("Disp Strength Y", Float) = -1
	_DisplacementScrollSpeedX("Disp Scroll Speed X", Float) = 0
	_DisplacementScrollSpeedY("Disp Scroll Speed Y", Float) = 0
}

Category {
	Tags { "Queue"="Transparent+3000" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	AlphaTest Greater .01
	Cull Off Lighting Off ZWrite Off

	SubShader {
		GrabPass {							
			Name "BASE"
			Tags { "LightMode" = "Always" }
 		}

		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord: TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float4 uvgrab : TEXCOORD0;
				float2 uvmain : TEXCOORD1;
			};

			half _DisplacementScrollSpeedX;
			half _DisplacementScrollSpeedY;
			half _StrengthX;
			half _StrengthY;
			float4 _DisplacementTex_ST;
			sampler2D _DisplacementTex;
			sampler2D _MainTex;

		v2f vert (appdata_t v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
#else
			float scale = 1.0;
#endif
			o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
			o.uvgrab.zw = o.vertex.zw;
			o.uvmain = TRANSFORM_TEX(v.texcoord, _DisplacementTex);
			o.color = v.color;
			return o;
		}

		sampler2D _GrabTexture;

		half4 frag( v2f i ) : COLOR
		{
			half2 uvOffset = half2(_Time.y*_DisplacementScrollSpeedX, _Time.y*_DisplacementScrollSpeedY);
			half4 colOffset = tex2D(_DisplacementTex, i.uvmain + uvOffset);

			i.uvgrab.x += colOffset.r * _StrengthX;
			i.uvgrab.y += colOffset.g * _StrengthY;

			half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
			col.a = i.color.a;
			fixed4 tint = tex2D(_MainTex, i.uvmain);
			col.a *= tint.a;
			col.a *= tint.r;
			return col;
		}
	ENDCG
	}
}
		// ------------------------------------------------------------------
		// Fallback for older cards and Unity non-Pro

		SubShader{
			Blend DstColor Zero
			Pass{
			Name "BASE"
			SetTexture[_MainTex]{ combine texture }
			}
		}
	}
}
