// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effect/Toon Additive"
{
	Properties
	{
		_MainTex 	("Texture", 2D) = "white" {}
		_MainRotation	("   ╠ Rotation", Range(0, 360)) = 0.0
		_UVScrollX		("   ╠ U Speed", Float) = 0.0
		_UVScrollY		("   ╠ V Speed", Float) = 0.0
		[Toggle] _USEPOS ("   ╚ Use Posterize", Float ) = 0
		_Posterize ("      ╚ Posterize", Range(2, 50) ) = 2.0
		_CutTex 	("Mask Texture", 2D) = "white" {}
		_CutRotation ("   ╠ Mask Rotation", Range(0, 360)) = 0
        [Toggle] _USEMASK ("   ╚ Use Mask Posterize", Float ) = 0
        _MaskPosterize ("      ╚ Mask Posterize", Range(2, 50)) = 2
        _EmissionGain ("Brightness", Range(0, 1)) = 0

			[Toggle] _USESHEET("Sheet Animation", Float) = 0
			_xx("   ╠ Tiles X", float) = 1
			_yy("   ╠ Tiles Y", float) = 1
			_Speed("   ╚ FPS", float) = 30

	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			#pragma shader_feature _USEPOS_ON
			#pragma shader_feature _USEMASK_ON
			#pragma shader_feature _USESHEET_ON
			#pragma shader_feature Enable_AlphaMask
			#pragma shader_feature Enable_UVRotation
			#pragma shader_feature Enable_UVScroll
			#pragma shader_feature Enable_Bloom
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
#ifdef Enable_AlphaMask
			uniform sampler2D _CutTex;
			uniform float4 _CutTex_ST;
#endif

#ifdef Enable_UVRotation
			uniform half _MainRotation;
			uniform half _CutRotation;
#endif

#ifdef Enable_UVScroll
			uniform half _UVScrollX;
			uniform half _UVScrollY;
#endif

#if _USEPOS_ON
			uniform half _Posterize;
#endif

#if _USEMASK_ON
			uniform half _MaskPosterize;
#endif

#ifdef Enable_Bloom
			uniform half _EmissionGain;
#endif

#if _USESHEET_ON
			uniform half _xx;
			uniform half _yy;
			uniform half _Speed;
#endif


			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv_MainTex : TEXCOORD0;

				UNITY_FOG_COORDS(1)

#ifdef Enable_AlphaMask
				float2 uv_MaskTex : TEXCOORD2;
#endif

#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD3;
#endif
				float4 vertexColor : COLOR;
			};
			
			v2f vert (a2v v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.vertexColor = v.vertexColor;


#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos(o.pos);
				COMPUTE_EYEDEPTH(o.projPos.z);
#endif

				o.uv_MainTex = TRANSFORM_TEX(v.uv, _MainTex);
#ifdef Enable_AlphaMask
				o.uv_MaskTex = TRANSFORM_TEX(v.uv, _CutTex) ;
#endif
				//位移
#ifdef Enable_UVScroll
				float2 move = float2(_UVScrollX , _UVScrollY) * _Time.y * _MainTex_ST.xy ;
				o.uv_MainTex += move ;
#endif

#ifdef Enable_UVRotation
				float c = 0.0;
				float s = 0.0;
				//MainTex旋轉
				o.uv_MainTex -= 0.5 ;
				c =  cos(_MainRotation/57.30265000896); 
				s =  sin(_MainRotation/57.30265000896); 
				o.uv_MainTex = mul(o.uv_MainTex , float2x2( c , -s , s , c )) ;
				o.uv_MainTex += 0.5 ; 
#endif
#ifdef Enable_AlphaMask
#ifdef Enable_UVRotation
				//MaskTex旋轉
				o.uv_MaskTex -= 0.5 ;
				c =  cos(_CutRotation/57.30265000896); 
				s =  sin(_CutRotation/57.30265000896); 
				o.uv_MaskTex = mul(o.uv_MaskTex , float2x2( c , -s , s , c )) ;
				o.uv_MaskTex += 0.5 ; 
#endif
#endif
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
#ifdef SOFTPARTICLES_ON		
			sampler2D_float _CameraDepthTexture;
#endif

			fixed4 frag (v2f i) : SV_Target
			{

#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (sceneZ - partZ);
				i.vertexColor.a *= fade;
#endif

				//texture

#if _USESHEET_ON
				float row = floor(_Time.y * _Speed);
				fixed4 Main = tex2D(_MainTex, saturate(i.uv_MainTex)*float2(1 / _xx, 1 / _yy) + float2(frac(row / _xx), (1 - ((floor(row / _xx)) + 1) / _yy)));
#else
				fixed4 Main = tex2D(_MainTex, i.uv_MainTex);
#endif


				//Posterize and open
#if _USEPOS_ON
				Main.rgb = saturate( floor((Main.rgb) * _Posterize) / (_Posterize - 1));
#endif

#ifdef Enable_Bloom
				Main = fixed4(Main.rgb * i.vertexColor.rgb * i.vertexColor.a *  (exp(_EmissionGain * 5.0f)), 1);
#else
				Main = fixed4((Main.rgb * i.vertexColor.rgb * i.vertexColor.a), 1);
#endif


				//Masktexture
#ifdef Enable_AlphaMask
				fixed Mask = tex2D(_CutTex, i.uv_MaskTex).a;

				//Posterize and open
#if _USEMASK_ON
				Mask = saturate( floor((Mask) * _MaskPosterize) / (_MaskPosterize - 1)); 
#endif

				Main.rgb *= Mask;
#endif
				UNITY_APPLY_FOG_COLOR(i.fogCoord, Main, fixed4(0, 0, 0, 0));
				return Main;
			}
			ENDCG
		}
	}
	CustomEditor "ShaderMaterialsEditor"
}
