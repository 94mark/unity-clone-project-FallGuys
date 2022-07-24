// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Effect/Effect Blend"
{
	Properties
	{
		_TintColor ("Main Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_CutTex ("Cutout (A)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_MainRotation("Main Rotation", Float) = 0.0
		_CutRotation("Cut Rotation",Float) = 0.0
		_UVScrollX("Main UV X Scroll",Float) = 0.0
		_UVScrollY("Main UV Y Scroll",Float) = 0.0
		_UVCutScrollX("Cut UV X Scroll",Float) = 0.0
		_UVCutScrollY("Cut UV Y Scroll",FLoat) = 0.0
		_UVMirrorX("UV Mirror X", Range(0.0,1.0)) = 0.0
		_UVMirrorY("UV Mirror Y", Range(0.0,1.0)) = 0.0
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_EmissionGain ("Emission Gain", Range(0, 1)) = 0.0
	}
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog
			#pragma shader_feature Enable_AlphaMask
			#pragma shader_feature Enable_UVRotation
			#pragma shader_feature Enable_UVScroll
			#pragma shader_feature Enable_UVMirror
			#pragma shader_feature Enable_Bloom
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _CutTex;
			float4 _MainTex_ST;
			float4 _CutTex_ST;
			fixed4 _TintColor;
			float _Cutoff;
			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
			struct v2f
			{
				half4 vertex : SV_POSITION;
				half2 uv_MainTex: TEXCOORD0;
#ifdef Enable_AlphaMask
				half2 uv_CutOut : TEXCOORD1;
				UNITY_FOG_COORDS(2)
#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD3;
#endif
#else
				UNITY_FOG_COORDS(1)
#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD2;
#endif
#endif
				fixed4 color : COLOR;
			};
#ifdef Enable_UVRotation
			float _MainRotation;
			float _CutRotation;
#endif
#ifdef Enable_UVScroll
			float _UVScrollX;
			float _UVScrollY;
			float _UVCutScrollX;
			float _UVCutScrollY;
#endif
#ifdef Enable_UVMirror
			float _UVMirrorX;
			float _UVMirrorY;
#endif

			v2f vert (appdata v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = UnityObjectToClipPos(v.vertex);
#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
#endif
				o.color = v.color;

#ifdef Enable_UVMirror
				float2 t = v.uv;
				float2 len = {_UVMirrorX,_UVMirrorY};
				float2 mirrorTextCoords = abs(t - len);
#else
				float2 mirrorTextCoords = v.uv;
#endif

#ifdef Enable_UVScroll
				float2 scroll = float2(_UVScrollX, _UVScrollY) * _Time.y;
				o.uv_MainTex = (mirrorTextCoords.xy + scroll) * _MainTex_ST.xy + _MainTex_ST.zw;
#else
				o.uv_MainTex = mirrorTextCoords.xy * _MainTex_ST.xy + _MainTex_ST.zw;
#endif


#ifdef Enable_UVRotation
				o.uv_MainTex.xy -= 0.5;

				float s = sin(radians(_MainRotation));
				float c = cos(radians(_MainRotation));

				float2x2 rotationMatrix = float2x2(c, -s, s, c);

				o.uv_MainTex.xy = mul(o.uv_MainTex.xy, rotationMatrix);
				o.uv_MainTex.xy += 0.5;
#endif


#ifdef Enable_AlphaMask
#ifdef Enable_UVScroll
				scroll = float2(_UVCutScrollX, _UVCutScrollY) * _Time.y;
				o.uv_CutOut = (v.uv + scroll) * _CutTex_ST.xy + _CutTex_ST.zw;
#else
				o.uv_CutOut = v.uv * _CutTex_ST.xy + _CutTex_ST.zw;
#endif
#ifdef Enable_UVRotation
				o.uv_CutOut.xy -= 0.5;

				s = sin(radians(_CutRotation));
				c = cos(radians(_CutRotation));

				rotationMatrix = float2x2(c, -s, s, c);

				o.uv_CutOut.xy = mul(o.uv_CutOut.xy, rotationMatrix);
				o.uv_CutOut.xy += 0.5;
#endif
#endif

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _InvFade;
			float _EmissionGain;

			half4 frag(v2f i) : SV_Target
			{
#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
#endif

#ifdef Enable_Bloom
				fixed4 c = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.uv_MainTex) * (exp(_EmissionGain * 5.0f));
#else
				fixed4 c = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.uv_MainTex);
#endif
#ifdef Enable_AlphaMask
				fixed4 ca = tex2D(_CutTex, i.uv_CutOut);
				c.a *= ca.a;
				c = ca.a >= _Cutoff ? c : 0;
#endif
				UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0)); // fog towards black due to our blend mode
				return c;
			}
			ENDCG
		}
	}
	Fallback "Transparent/VertexLit"
	CustomEditor "ShaderMaterialsEditor"
}