// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aqin/Diffusion"
{
	Properties
	{
		[HDR]_Color("Color", Color) = (8,0,0,0)
		_transport("transport", Color) = (1,0,0,0)
		_Timescale1("Time scale", Range( 0 , 2)) = 0.5
		_interval01("interval01", Range( 0 , 1)) = 0.33
		_interval02("interval02", Range( 0 , 1)) = 0.66
		_interval03("interval03", Range( 0 , 1)) = 0.99
		_interval04("interval04", Range( 0 , 1)) = 0.99
		_intervalsub("interval sub", Range( 0 , 1)) = 0.33
		_softe("softe", Float) = 0.33
		_softetrans("softe trans", Float) = 0.33
		_widthvalue("width value", Float) = 0.06
		_Spider_BaseColor("Spider_BaseColor", 2D) = "white" {}
		_Texturepower("Texture power", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+1" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Stencil
		{
			Ref 1
			Comp Equal
			Pass Keep
			Fail Keep
		}
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color;
		uniform float _softe;
		uniform float _Timescale1;
		uniform float _interval01;
		uniform float _intervalsub;
		uniform float _widthvalue;
		uniform float _interval02;
		uniform float _interval03;
		uniform float _interval04;
		uniform sampler2D _Spider_BaseColor;
		uniform float4 _Spider_BaseColor_ST;
		uniform float _Texturepower;
		uniform float4 _transport;
		uniform float _softetrans;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float myVarName5 = saturate( length( (float2( -1,-1 ) + (i.uv_texcoord - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 ))) ) );
			float temp_output_48_0 = ( 1.0 - pow( myVarName5 , _softe ) );
			float3 temp_cast_0 = (myVarName5).xxx;
			float3 temp_output_14_0_g11 = temp_cast_0;
			float mulTime93 = _Time.y * _Timescale1;
			float temp_output_104_0 = saturate( mulTime93 );
			float temp_output_4_0_g11 = frac( ( temp_output_104_0 - ( _interval01 - _intervalsub ) ) );
			float3 temp_cast_1 = (temp_output_4_0_g11).xxx;
			float3 temp_cast_2 = (( temp_output_4_0_g11 - _widthvalue )).xxx;
			float3 temp_cast_3 = (myVarName5).xxx;
			float3 temp_output_14_0_g12 = temp_cast_3;
			float temp_output_4_0_g12 = frac( ( temp_output_104_0 - ( _interval02 - _intervalsub ) ) );
			float3 temp_cast_4 = (temp_output_4_0_g12).xxx;
			float3 temp_cast_5 = (( temp_output_4_0_g12 - _widthvalue )).xxx;
			float3 temp_cast_6 = (myVarName5).xxx;
			float3 temp_output_14_0_g10 = temp_cast_6;
			float temp_output_4_0_g10 = frac( ( temp_output_104_0 - ( _interval03 - _intervalsub ) ) );
			float3 temp_cast_7 = (temp_output_4_0_g10).xxx;
			float3 temp_cast_8 = (( temp_output_4_0_g10 - _widthvalue )).xxx;
			float3 temp_cast_9 = (myVarName5).xxx;
			float3 temp_output_14_0_g9 = temp_cast_9;
			float temp_output_4_0_g9 = frac( ( temp_output_104_0 - ( _interval04 - _intervalsub ) ) );
			float3 temp_cast_10 = (temp_output_4_0_g9).xxx;
			float3 temp_cast_11 = (( temp_output_4_0_g9 - _widthvalue )).xxx;
			float2 uv_Spider_BaseColor = i.uv_texcoord * _Spider_BaseColor_ST.xy + _Spider_BaseColor_ST.zw;
			float4 temp_cast_13 = (_Texturepower).xxxx;
			float4 temp_output_59_0 = ( temp_output_48_0 * float4( ( ( step( temp_output_14_0_g11 , temp_cast_1 ) * ( 1.0 - step( temp_output_14_0_g11 , temp_cast_2 ) ) ) + ( step( temp_output_14_0_g12 , temp_cast_4 ) * ( 1.0 - step( temp_output_14_0_g12 , temp_cast_5 ) ) ) + ( step( temp_output_14_0_g10 , temp_cast_7 ) * ( 1.0 - step( temp_output_14_0_g10 , temp_cast_8 ) ) ) + ( step( temp_output_14_0_g9 , temp_cast_10 ) * ( 1.0 - step( temp_output_14_0_g9 , temp_cast_11 ) ) ) ) , 0.0 ) * pow( tex2D( _Spider_BaseColor, uv_Spider_BaseColor ) , temp_cast_13 ) );
			o.Emission = ( _Color * temp_output_59_0 ).rgb;
			o.Alpha = ( temp_output_59_0 * _transport.a * pow( temp_output_48_0 , _softetrans ) ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19108
Node;AmplifyShaderEditor.CommentaryNode;20;-1913.878,-555.0261;Inherit;False;1087.379;398.6733;Comment;5;5;1;4;3;2;ring;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1858.464,-458.1689;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-1046.464,-463.1689;Inherit;True;myVarName;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;4;-1209.463,-459.1689;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;3;-1381.463,-458.1689;Inherit;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;2;-1641.463,-458.1689;Inherit;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;-643.4976,-145.8339;Inherit;True;5;myVarName;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-2.988934,92.90111;Inherit;True;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;69;-324.1723,571.0841;Inherit;True;Property;_Spider_BaseColor;Spider_BaseColor;11;0;Create;True;0;0;0;False;0;False;-1;43cae36c25a647a4884cad45cfc4a8fd;43cae36c25a647a4884cad45cfc4a8fd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;71;57.74329,582.1768;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.2;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;48;4.323261,-308.3441;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;47;-287.4461,-308.7299;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;78;200.4775,-304.7526;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;40.75069,-445.4951;Inherit;False;Property;_softetrans;softe trans;9;0;Create;True;0;0;0;False;0;False;0.33;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-547.3916,-314.0568;Inherit;False;Property;_softe;softe;8;0;Create;True;0;0;0;False;0;False;0.33;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;67;293.6812,429.974;Float;False;Property;_transport;transport;1;0;Create;True;0;0;0;False;0;False;1,0,0,0;1,1,1,0.6431373;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;72;-234.8527,777.2554;Inherit;False;Property;_Texturepower;Texture power;12;0;Create;True;0;0;0;False;0;False;0;0.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;61;1669.182,7.135491;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Aqin/Diffusion;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;1;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;True;1;False;;255;False;;255;False;;5;False;;1;False;;1;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Absolute;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;265.5882,38.38722;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;831.4749,10.30313;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;52;517.0212,-159.8178;Inherit;False;Property;_Color;Color;0;1;[HDR];Create;True;0;0;0;False;0;False;8,0,0,0;0.1589609,0.2015563,0.496933,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;820.3841,323.9394;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;100;-359.1752,377.4015;Inherit;False;Ring Diffion;-1;;9;c7334664b95e91246a23b7cf4f00e885;0;4;14;FLOAT3;0,0,0;False;15;FLOAT;1;False;16;FLOAT;0.33;False;17;FLOAT;0.1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;101;-352.6105,227.7831;Inherit;False;Ring Diffion;-1;;10;c7334664b95e91246a23b7cf4f00e885;0;4;14;FLOAT3;0,0,0;False;15;FLOAT;1;False;16;FLOAT;0.33;False;17;FLOAT;0.1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;102;-354.9225,-50.00558;Inherit;False;Ring Diffion;-1;;11;c7334664b95e91246a23b7cf4f00e885;0;4;14;FLOAT3;0,0,0;False;15;FLOAT;1;False;16;FLOAT;0.33;False;17;FLOAT;0.1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;103;-353.611,89.18285;Inherit;False;Ring Diffion;-1;;12;c7334664b95e91246a23b7cf4f00e885;0;4;14;FLOAT3;0,0,0;False;15;FLOAT;1;False;16;FLOAT;0.33;False;17;FLOAT;0.1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-635.2546,296.8157;Inherit;False;Property;_widthvalue;width value;10;0;Create;True;0;0;0;False;0;False;0.06;0.025;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-1335.071,218.8293;Inherit;False;Property;_interval02;interval02;4;0;Create;True;0;0;0;False;0;False;0.66;0.058;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1334.071,331.8289;Inherit;False;Property;_interval03;interval03;5;0;Create;True;0;0;0;False;0;False;0.99;0.112;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-1334.706,466.0544;Inherit;False;Property;_interval04;interval04;6;0;Create;True;0;0;0;False;0;False;0.99;0.173;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1336.581,80.17879;Inherit;False;Property;_interval01;interval01;3;0;Create;True;0;0;0;False;0;False;0.33;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-1620.852,148.6889;Inherit;False;Property;_intervalsub;interval sub;7;0;Create;True;0;0;0;False;0;False;0.33;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;80;-890.5214,60.6204;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;82;-900.5194,200.4493;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;83;-901.8198,322.6496;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;84;-887.5196,437.0492;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;93;-1254.286,-109.5696;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-1534.048,-113.681;Inherit;False;Property;_Timescale1;Time scale;2;0;Create;True;0;0;0;False;0;False;0.5;0.3;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;104;-1082.604,-107.9096;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
WireConnection;5;0;4;0
WireConnection;4;0;3;0
WireConnection;3;0;2;0
WireConnection;2;0;1;0
WireConnection;41;0;102;0
WireConnection;41;1;103;0
WireConnection;41;2;101;0
WireConnection;41;3;100;0
WireConnection;71;0;69;0
WireConnection;71;1;72;0
WireConnection;48;0;47;0
WireConnection;47;0;37;0
WireConnection;47;1;62;0
WireConnection;78;0;48;0
WireConnection;78;1;79;0
WireConnection;61;2;49;0
WireConnection;61;9;66;0
WireConnection;59;0;48;0
WireConnection;59;1;41;0
WireConnection;59;2;71;0
WireConnection;49;0;52;0
WireConnection;49;1;59;0
WireConnection;66;0;59;0
WireConnection;66;1;67;4
WireConnection;66;2;78;0
WireConnection;100;14;37;0
WireConnection;100;15;104;0
WireConnection;100;16;84;0
WireConnection;100;17;40;0
WireConnection;101;14;37;0
WireConnection;101;15;104;0
WireConnection;101;16;83;0
WireConnection;101;17;40;0
WireConnection;102;14;37;0
WireConnection;102;15;104;0
WireConnection;102;16;80;0
WireConnection;102;17;40;0
WireConnection;103;14;37;0
WireConnection;103;15;104;0
WireConnection;103;16;82;0
WireConnection;103;17;40;0
WireConnection;80;0;39;0
WireConnection;80;1;81;0
WireConnection;82;0;45;0
WireConnection;82;1;81;0
WireConnection;83;0;46;0
WireConnection;83;1;81;0
WireConnection;84;0;65;0
WireConnection;84;1;81;0
WireConnection;93;0;38;0
WireConnection;104;0;93;0
ASEEND*/
//CHKSM=F019CF5C818755D19D9F85ED0720BB0EB593D66F