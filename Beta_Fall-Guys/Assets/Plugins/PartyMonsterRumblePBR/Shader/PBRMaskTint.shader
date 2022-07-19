// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PBRMaskTint"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		_SAM("SAM", 2D) = "white" {}
		_Mask01("Mask01", 2D) = "white" {}
		_Mask02("Mask02", 2D) = "white" {}
		_Mask03("Mask03", 2D) = "white" {}
		_Color01("Color01", Color) = (0.7205882,0.08477508,0.08477508,0)
		_Color02("Color02", Color) = (0.02649222,0.3602941,0.09785674,0)
		_Color03("Color03", Color) = (0.07628676,0.2567445,0.6102941,0)
		_Color04("Color04", Color) = (0.6207737,0.1119702,0.8014706,0)
		_Color05("Color05", Color) = (0.9056604,0.5051349,0.09825563,0)
		_Color06("Color06", Color) = (1,0.7848822,0,0)
		_Color07("Color07", Color) = (0.6207737,0.1119702,0.8014706,0)
		_Color08("Color08", Color) = (0.9056604,0.5051349,0.09825563,0)
		_Color01Power("Color01Power", Range( 0 , 6)) = 1
		_Color02Power("Color02Power", Range( 0 , 6)) = 1
		_Color03Power("Color03Power", Range( 0 , 6)) = 1
		_Color04Power("Color04Power", Range( 0 , 6)) = 1
		_Color05Power("Color05Power", Range( 0 , 6)) = 1
		_Color06Power("Color06Power", Range( 0 , 6)) = 1
		_Color07Power("Color07Power", Range( 0 , 6)) = 1
		_Color08Power("Color08Power", Range( 0 , 6)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _Mask01;
		uniform float4 _Mask01_ST;
		uniform float4 _Color01;
		uniform float _Color01Power;
		uniform float4 _Color02;
		uniform float _Color02Power;
		uniform float4 _Color03;
		uniform float _Color03Power;
		uniform sampler2D _Mask02;
		uniform float4 _Mask02_ST;
		uniform float4 _Color04;
		uniform float _Color04Power;
		uniform float4 _Color05;
		uniform float _Color05Power;
		uniform float4 _Color06;
		uniform float _Color06Power;
		uniform sampler2D _Mask03;
		uniform float4 _Mask03_ST;
		uniform float4 _Color07;
		uniform float _Color07Power;
		uniform float4 _Color08;
		uniform float _Color08Power;
		uniform sampler2D _SAM;
		uniform float4 _SAM_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode16 = tex2D( _Albedo, uv_Albedo );
			float2 uv_Mask01 = i.uv_texcoord * _Mask01_ST.xy + _Mask01_ST.zw;
			float4 tex2DNode13 = tex2D( _Mask01, uv_Mask01 );
			float4 temp_cast_0 = (tex2DNode13.r).xxxx;
			float4 temp_cast_1 = (tex2DNode13.g).xxxx;
			float4 temp_cast_2 = (tex2DNode13.b).xxxx;
			float2 uv_Mask02 = i.uv_texcoord * _Mask02_ST.xy + _Mask02_ST.zw;
			float4 tex2DNode41 = tex2D( _Mask02, uv_Mask02 );
			float4 temp_cast_3 = (tex2DNode41.r).xxxx;
			float4 temp_cast_4 = (tex2DNode41.g).xxxx;
			float4 temp_cast_5 = (tex2DNode41.b).xxxx;
			float2 uv_Mask03 = i.uv_texcoord * _Mask03_ST.xy + _Mask03_ST.zw;
			float4 tex2DNode58 = tex2D( _Mask03, uv_Mask03 );
			float4 temp_cast_6 = (tex2DNode58.r).xxxx;
			float4 temp_cast_7 = (tex2DNode58.g).xxxx;
			float4 blendOpSrc22 = tex2DNode16;
			float4 blendOpDest22 = ( ( min( temp_cast_0 , _Color01 ) * _Color01Power ) + ( min( temp_cast_1 , _Color02 ) * _Color02Power ) + ( min( temp_cast_2 , _Color03 ) * _Color03Power ) + ( min( temp_cast_3 , _Color04 ) * _Color04Power ) + ( min( temp_cast_4 , _Color05 ) * _Color05Power ) + ( min( temp_cast_5 , _Color06 ) * _Color06Power ) + ( min( temp_cast_6 , _Color07 ) * _Color07Power ) + ( min( temp_cast_7 , _Color08 ) * _Color08Power ) );
			float4 lerpResult4 = lerp( tex2DNode16 , ( saturate( ( blendOpSrc22 * blendOpDest22 ) )) , ( tex2DNode13.r + tex2DNode13.g + tex2DNode13.b + tex2DNode41.r + tex2DNode41.g + tex2DNode41.b + tex2DNode58.r + tex2DNode58.g ));
			o.Albedo = lerpResult4.rgb;
			float2 uv_SAM = i.uv_texcoord * _SAM_ST.xy + _SAM_ST.zw;
			float4 tex2DNode67 = tex2D( _SAM, uv_SAM );
			o.Metallic = tex2DNode67.b;
			o.Smoothness = tex2DNode67.r;
			o.Occlusion = tex2DNode67.g;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
0;0;1920;1018;2767.048;979.9222;2.029305;True;True
Node;AmplifyShaderEditor.ColorNode;9;-1941.449,-45.75774;Float;False;Property;_Color01;Color01;5;0;Create;True;0;0;False;0;0.7205882,0.08477508,0.08477508,0;0.7205882,0.08477508,0.08477508,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;50;-2113.338,1275.471;Float;False;Property;_Color06;Color06;10;0;Create;True;0;0;False;0;1,0.7848822,0,0;0.7735849,0.3949614,0.04013884,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;33;-2107.914,834.2791;Float;False;Property;_Color04;Color04;8;0;Create;True;0;0;False;0;0.6207737,0.1119702,0.8014706,0;0.6207737,0.1119702,0.8014706,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-1908.554,183.2922;Float;False;Property;_Color02;Color02;6;0;Create;True;0;0;False;0;0.02649222,0.3602941,0.09785674,0;0.02649222,0.3602941,0.09785674,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;43;-2112.565,1064.268;Float;False;Property;_Color05;Color05;9;0;Create;True;0;0;False;0;0.9056604,0.5051349,0.09825563,0;0.7735849,0.3949614,0.04013884,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-2355.11,599.1314;Float;True;Property;_Mask02;Mask02;3;0;Create;True;0;0;False;0;dea1ff9608588c14ab9d2dd4887ac6db;dea1ff9608588c14ab9d2dd4887ac6db;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-2073.799,-520.4126;Float;True;Property;_Mask01;Mask01;2;0;Create;True;0;0;False;0;dea1ff9608588c14ab9d2dd4887ac6db;dea1ff9608588c14ab9d2dd4887ac6db;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-1944.708,458.9244;Float;False;Property;_Color03;Color03;7;0;Create;True;0;0;False;0;0.07628676,0.2567445,0.6102941,0;0.07628676,0.2567445,0.6102941,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;60;-2020.343,1709.483;Float;False;Property;_Color08;Color08;12;0;Create;True;0;0;False;0;0.9056604,0.5051349,0.09825563,0;0.7735849,0.3949614,0.04013884,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;59;-2012.842,1473.191;Float;False;Property;_Color07;Color07;11;0;Create;True;0;0;False;0;0.6207737,0.1119702,0.8014706,0;0.6207737,0.1119702,0.8014706,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;58;-2413.589,1480.533;Float;True;Property;_Mask03;Mask03;4;0;Create;True;0;0;False;0;dea1ff9608588c14ab9d2dd4887ac6db;dea1ff9608588c14ab9d2dd4887ac6db;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMinOpNode;61;-1616.742,1483.63;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;64;-1389.684,1925.047;Float;False;Property;_Color08Power;Color08Power;20;0;Create;True;0;0;False;0;1;1;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-1384.228,1641.679;Float;False;Property;_Color07Power;Color07Power;19;0;Create;True;0;0;False;0;1;1;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1373.947,342.9876;Float;False;Property;_Color02Power;Color02Power;14;0;Create;True;0;0;False;0;1;1;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;18;-1598.141,416.7289;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;62;-1627.409,1757.293;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;17;-1577.399,193.755;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;49;-1626.588,1186.971;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1381.273,142.3805;Float;False;Property;_Color01Power;Color01Power;13;0;Create;True;0;0;False;0;1;1;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;34;-1626.32,655.0611;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1400.679,536.0121;Float;False;Property;_Color03Power;Color03Power;15;0;Create;True;0;0;False;0;1;1;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1395.471,1308.597;Float;False;Property;_Color06Power;Color06Power;18;0;Create;True;0;0;False;0;1;1;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1399.262,1096.478;Float;False;Property;_Color05Power;Color05Power;17;0;Create;True;0;0;False;0;1;1;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1393.806,813.1097;Float;False;Property;_Color04Power;Color04Power;16;0;Create;True;0;0;False;0;1;1;0;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;42;-1636.987,928.7234;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;15;-1607.751,-89.10741;Float;True;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-1087.436,1774.902;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1099.345,1245.142;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1097.013,946.3328;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1098.774,23.16727;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1099.408,728.6387;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1092.607,418.6484;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-1089.831,1557.208;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1105.004,261.0664;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;16;-1042.803,-639.5633;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;2f06be5cc89f84f48bfe39a1f88242c4;2f06be5cc89f84f48bfe39a1f88242c4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-867.4047,60.58863;Float;True;8;8;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;22;-622.9166,-39.07136;Float;False;Multiply;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-1187.253,-356.6879;Float;True;8;8;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;4;-204.1115,-328.2731;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;67;-258.9017,-8.104975;Float;True;Property;_SAM;SAM;1;0;Create;True;0;0;False;0;2f06be5cc89f84f48bfe39a1f88242c4;2f06be5cc89f84f48bfe39a1f88242c4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;194.515,-327.4254;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PBRMaskTint;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;61;0;58;1
WireConnection;61;1;59;0
WireConnection;18;0;13;3
WireConnection;18;1;11;0
WireConnection;62;0;58;2
WireConnection;62;1;60;0
WireConnection;17;0;13;2
WireConnection;17;1;10;0
WireConnection;49;0;41;3
WireConnection;49;1;50;0
WireConnection;34;0;41;1
WireConnection;34;1;33;0
WireConnection;42;0;41;2
WireConnection;42;1;43;0
WireConnection;15;0;13;1
WireConnection;15;1;9;0
WireConnection;66;0;62;0
WireConnection;66;1;64;0
WireConnection;51;0;49;0
WireConnection;51;1;52;0
WireConnection;44;0;42;0
WireConnection;44;1;45;0
WireConnection;36;0;15;0
WireConnection;36;1;35;0
WireConnection;40;0;34;0
WireConnection;40;1;39;0
WireConnection;37;0;18;0
WireConnection;37;1;38;0
WireConnection;65;0;61;0
WireConnection;65;1;63;0
WireConnection;32;0;17;0
WireConnection;32;1;31;0
WireConnection;19;0;36;0
WireConnection;19;1;32;0
WireConnection;19;2;37;0
WireConnection;19;3;40;0
WireConnection;19;4;44;0
WireConnection;19;5;51;0
WireConnection;19;6;65;0
WireConnection;19;7;66;0
WireConnection;22;0;16;0
WireConnection;22;1;19;0
WireConnection;20;0;13;1
WireConnection;20;1;13;2
WireConnection;20;2;13;3
WireConnection;20;3;41;1
WireConnection;20;4;41;2
WireConnection;20;5;41;3
WireConnection;20;6;58;1
WireConnection;20;7;58;2
WireConnection;4;0;16;0
WireConnection;4;1;22;0
WireConnection;4;2;20;0
WireConnection;0;0;4;0
WireConnection;0;3;67;3
WireConnection;0;4;67;1
WireConnection;0;5;67;2
ASEEND*/
//CHKSM=BCC3146C9B6E0CBF20D94891CFC764CC4444E36A