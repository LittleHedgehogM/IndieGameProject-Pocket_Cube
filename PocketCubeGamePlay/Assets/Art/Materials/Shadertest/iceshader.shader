// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aqin/iceshader"
{
	Properties
	{
		_Basecolor("Base color", Color) = (1,1,1,0.1254902)
		_Reflect02("Reflect02", Color) = (0.6509434,0.6076939,0.4339622,1)
		_ReflectOffset001("Reflect Off set001", Range( 0 , 10)) = 4.393466
		_ReflectPow("ReflectPow", Range( 0 , 1)) = 0.5
		_ReflectOffset002("Reflect Off set002", Range( 0 , 10)) = 2
		_TextureSample3("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float3 worldNormal;
			INTERNAL_DATA
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _Basecolor;
		uniform float4 _Reflect02;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform float _ReflectOffset001;
		uniform float _ReflectOffset002;
		uniform float _ReflectPow;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			o.Albedo = _Basecolor.rgb;
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float4 tex2DNode15 = tex2D( _TextureSample3, uv_TextureSample3 );
			float4 appendResult18 = (float4(tex2DNode15.r , tex2DNode15.g , 0.95 , 0.0));
			float3 normalizeResult20 = normalize( (WorldNormalVector( i , appendResult18.xyz )) );
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult21 = dot( reflect( normalizeResult20 , float3( 0,0,0 ) ) , ase_worldlightDir );
			float temp_output_22_0 = saturate( dotResult21 );
			float clampResult30 = clamp( ( round( ( pow( temp_output_22_0 , _ReflectOffset001 ) * _ReflectOffset002 ) ) - 1.0 ) , -0.25 , 1.0 );
			float4 lerpResult31 = lerp( float4( 0,0,0,0 ) , _Reflect02 , clampResult30);
			float4 temp_output_5_0 = ( ( lerpResult31 * saturate( round( temp_output_22_0 ) ) ) * _ReflectPow );
			o.Emission = temp_output_5_0.rgb;
			float temp_output_1_4 = _Basecolor.a;
			o.Alpha = temp_output_1_4;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

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
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
Node;AmplifyShaderEditor.CommentaryNode;2;-706.0278,-471.0947;Inherit;False;2456.841;703.4816;reflect;19;37;36;31;30;28;26;25;24;22;21;12;11;10;9;8;7;6;5;4;Reflect02;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;3;-2346.895,-468.1417;Inherit;False;1601.529;532.0251;;5;34;20;19;18;15;Reflection;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.WireNode;7;195.9975,93.01481;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-163.6044,129.8435;Inherit;False;Property;_ReflectOffset002;Reflect Off set002;5;0;Create;True;0;0;0;False;0;False;2;1.67;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;10;-379.1253,-88.07149;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-1527.706,-337.2158;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;19;-1375.018,-338.4128;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;20;-1154.358,-330.0277;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;21;-120.72,-56.07062;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;22;4.873456,-56.07062;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-167.2755,55.95139;Inherit;False;Property;_ReflectOffset001;Reflect Off set001;3;0;Create;True;0;0;0;False;0;False;4.393466;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;31;1236.873,-248.0719;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1663.73,-173.8669;Inherit;False;Constant;_Float2;Float 1;12;0;Create;True;0;0;0;False;0;False;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;932.8712,-248.0719;Inherit;False;Property;_Reflect02;Reflect02;1;0;Create;True;0;0;0;False;0;False;0.6509434,0.6076939,0.4339622,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;37;-427.1247,7.929481;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;15;-1944.289,-368.7186;Inherit;True;Property;_TextureSample3;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;9fb015b33574ef74eae24849a3c3e563;9138feb8dc43c494395caba74376a3a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;1725.964,-426.9053;Inherit;False;Property;_Basecolor;Base color;0;0;Create;True;0;0;0;False;0;False;1,1,1,0.1254902;1,1,1,0.1568628;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;30;1088.98,-86.26845;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-0.25;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;1315.874,-5.07164;Inherit;False;Property;_ReflectPow;ReflectPow;4;0;Create;True;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;12;946.5779,147.4731;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;6;1093.448,124.6655;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2413.822,-406.5721;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Aqin/iceshader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;False;Transparent;;AlphaTest;ForwardOnly;12;all;True;True;True;True;0;False;;False;1;False;;255;False;;255;False;;5;False;;1;False;;1;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0.1;1,1,1,0;VertexScale;True;False;Cylindrical;False;True;Relative;0;;2;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;2018.798,150.7185;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;1390.874,-236.0719;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;1621.876,-228.0719;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;2167.863,-116.0822;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;1967.686,-174.2005;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RoundOpNode;26;587.178,-52.84731;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;11;163.8735,-56.07062;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;378.7125,13.85494;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;766.3116,-69.45856;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;46;1805.209,249.6625;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;55;1324.952,489.6448;Inherit;False;919.9573;567.8873;Comment;5;60;59;58;57;56;Custom Outline;1,0.6029412,0.7097364,1;0;0
Node;AmplifyShaderEditor.ColorNode;58;1704.808,602.5631;Inherit;False;Property;_Color0;Color 0;8;0;Create;True;0;0;0;False;0;False;0,0,0,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;1777.142,801.8881;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;1524.592,866.2772;Inherit;False;-1;;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;1413.768,786.3751;Float;False;Property;_OutlineWidth;Outline Width;7;0;Create;True;0;0;0;False;0;False;0.02;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.OutlineNode;60;2022.443,623.0607;Inherit;False;1;False;None;1;7;Off;True;True;True;True;0;False;;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
WireConnection;7;0;22;0
WireConnection;10;0;20;0
WireConnection;18;0;15;1
WireConnection;18;1;15;2
WireConnection;18;2;34;0
WireConnection;19;0;18;0
WireConnection;20;0;19;0
WireConnection;21;0;10;0
WireConnection;21;1;37;0
WireConnection;22;0;21;0
WireConnection;31;1;36;0
WireConnection;31;2;30;0
WireConnection;30;0;28;0
WireConnection;12;0;7;0
WireConnection;6;0;12;0
WireConnection;0;0;1;0
WireConnection;0;2;5;0
WireConnection;0;9;1;4
WireConnection;47;0;46;0
WireConnection;47;1;1;4
WireConnection;4;0;31;0
WireConnection;4;1;6;0
WireConnection;5;0;4;0
WireConnection;5;1;8;0
WireConnection;53;0;54;0
WireConnection;53;1;47;0
WireConnection;54;0;5;0
WireConnection;54;1;36;4
WireConnection;26;0;25;0
WireConnection;11;0;22;0
WireConnection;11;1;24;0
WireConnection;25;0;11;0
WireConnection;25;1;9;0
WireConnection;28;0;26;0
WireConnection;46;0;5;0
WireConnection;56;0;57;0
WireConnection;56;1;59;0
WireConnection;60;0;58;0
WireConnection;60;1;56;0
ASEEND*/
//CHKSM=0C181E2A31DD18B190FBAFCA960DF73BEE0F963D