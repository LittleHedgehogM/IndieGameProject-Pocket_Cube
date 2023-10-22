// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aqin/Character/Less"
{
	Properties
	{
		_ReflectOff01("Reflect Off01", Color) = (0.8588235,0.5792313,0,1)
		_Reflect02("Reflect02", Color) = (0.6509434,0.6076939,0.4339622,1)
		_diffusegradient01("diffusegradient01", Color) = (1,1,1,1)
		_diffusegradient02("diffusegradient02", Color) = (0.3509552,0.6132076,0.5379074,1)
		_TextureSample1("Texture Sample 0", 2D) = "white" {}
		_BaseCellOffset002("Base Cell Offset002", Range( 0.01 , 1)) = 0.01
		_BaseCellOffset001("Base Cell Offset001", Range( -1 , 1)) = 0
		_N_BaseCellOffset001("N_Base Cell Offset001", Range( 0 , 10)) = 2.235294
		_R_BaseCellOffset001("R_Base Cell Offset001", Range( 0 , 10)) = 2.235294
		_N_BaseCellOffset002("N_Base Cell Offset002", Range( 0 , 10)) = 2.235294
		_R_BaseCellOffset002("R_Base Cell Offset002", Range( 0 , 10)) = 2.235294
		_BaseCellOffset003("Base Cell Offset003", Range( 0 , 1)) = 0.6117647
		_BaseCellOffset004("Base Cell Offset004", Range( 0 , 1)) = 0.7676471
		_ReflectOffset001("Reflect Off set001", Range( 0 , 10)) = 4.393466
		_ReflectPow("ReflectPow", Range( 0 , 1)) = 0.5
		_ReflectOffset002("Reflect Off set002", Range( 0 , 10)) = 2
		_TextureSample3("Texture Sample 0", 2D) = "white" {}
		_TextureSample2("Texture Sample 0", 2D) = "white" {}
		_HeighRatioInPut("HeighRatioInPut ", Range( 0 , 10)) = 10
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		ZTest LEqual
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
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

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform float4 _ReflectOff01;
		uniform float4 _Reflect02;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform float _R_BaseCellOffset001;
		uniform float _R_BaseCellOffset002;
		uniform float _ReflectOffset001;
		uniform float _ReflectOffset002;
		uniform float _ReflectPow;
		uniform float4 _diffusegradient01;
		uniform float4 _diffusegradient02;
		uniform sampler2D _TextureSample1;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _HeighRatioInPut;
		uniform float _N_BaseCellOffset001;
		uniform float _N_BaseCellOffset002;
		uniform float _BaseCellOffset001;
		uniform float _BaseCellOffset002;
		uniform float _BaseCellOffset003;
		uniform float _BaseCellOffset004;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float2 temp_cast_2 = (1.0).xx;
			float2 uv_TexCoord7 = i.uv_texcoord * temp_cast_2;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode50 = tex2D( _TextureSample2, uv_TextureSample2 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 temp_cast_3 = (ase_worldViewDir.z).xxx;
			float2 Offset3 = ( ( tex2DNode50.b - 1 ) * temp_cast_3.xy * _HeighRatioInPut ) + uv_TexCoord7;
			float4 lerpResult13 = lerp( _diffusegradient01 , _diffusegradient02 , tex2D( _TextureSample1, Offset3 ).r);
			float4 appendResult30 = (float4(( ( tex2DNode50.g * _N_BaseCellOffset001 ) - _N_BaseCellOffset002 ) , ( ( tex2DNode50.r * _N_BaseCellOffset001 ) - _N_BaseCellOffset002 ) , 0.95 , 0.0));
			float3 normalizeResult80 = normalize( (WorldNormalVector( i , appendResult30.xyz )) );
			float3 NewNormals86 = normalizeResult80;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult78 = dot( NewNormals86 , ase_worldlightDir );
			float NdotL84 = dotResult78;
			float4 main337 = ( lerpResult13 * saturate( ( round( ( ( ( NdotL84 + _BaseCellOffset001 ) / _BaseCellOffset002 ) + _BaseCellOffset003 ) ) + _BaseCellOffset004 ) ) );
			c.rgb = main337.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float4 tex2DNode190 = tex2D( _TextureSample3, uv_TextureSample3 );
			float4 appendResult140 = (float4(( ( tex2DNode190.g * _R_BaseCellOffset001 ) - _R_BaseCellOffset002 ) , ( ( tex2DNode190.r * _R_BaseCellOffset001 ) - _R_BaseCellOffset002 ) , 0.95 , 0.0));
			float3 normalizeResult137 = normalize( (WorldNormalVector( i , appendResult140.xyz )) );
			float3 myVarName151 = normalizeResult137;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult156 = dot( reflect( myVarName151 , float3( 0,0,0 ) ) , ase_worldlightDir );
			float temp_output_160_0 = saturate( dotResult156 );
			float clampResult172 = clamp( ( ( ( round( ( pow( temp_output_160_0 , _ReflectOffset001 ) * _ReflectOffset002 ) ) * 0.5 ) - 1.0 ) * 2.0 ) , -0.25 , 1.0 );
			float4 lerpResult175 = lerp( _ReflectOff01 , _Reflect02 , clampResult172);
			o.Emission = ( ( lerpResult175 * saturate( round( temp_output_160_0 ) ) ) * _ReflectPow ).rgb;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				vertexDataFunc( v );
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
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
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
Node;AmplifyShaderEditor.CommentaryNode;426;771.3385,1218.303;Inherit;False;854.6548;278.1097;Comment;7;423;425;420;422;424;421;419;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;260;382.7297,939.7471;Inherit;False;1254.347;253.1995;Comment;9;247;245;238;263;240;243;237;244;246;Rim erode;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;179;-831.9353,-1315.883;Inherit;False;2456.841;703.4816;reflect;23;173;164;162;168;163;160;161;156;178;170;182;181;180;172;165;174;177;176;175;155;154;152;446;Reflect02;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;117;128.8725,-567.4103;Inherit;False;1517.705;868.1461;Toon diffuse;16;89;109;107;108;91;87;92;90;88;337;128;15;111;110;14;13;Toon diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;46;-914.1636,-552.5628;Inherit;False;1024.409;657.902;Comment;6;2;3;4;12;5;7;Base;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;76;-1382.066,128.0586;Inherit;False;1503.606;511.2192;Comment;13;86;80;81;53;27;24;26;28;30;50;114;115;258;Normals;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;77;-717.0706,714.9427;Inherit;False;835.6508;341.2334;Comment;4;85;84;79;78;N dot L;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;130;-2472.802,-1312.93;Inherit;False;1601.529;532.0251;;12;190;139;141;135;151;142;137;140;136;134;133;132;Reflection;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;155;-553.0322,-836.8585;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;1286.966,-1076.86;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;181;968.54,-776.1226;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;182;70.08984,-751.7733;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;178;1148.966,-956.8597;Inherit;False;Property;_ReflectPow;ReflectPow;14;0;Create;True;0;0;0;False;0;False;0.5;0.5705882;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-289.5121,-714.9446;Inherit;False;Property;_ReflectOffset002;Reflect Off set002;15;0;Create;True;0;0;0;False;0;False;2;3.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;13;620.1099,-240.9034;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;110;290.2032,138.5789;Inherit;False;Property;_BaseCellOffset003;Base Cell Offset003;11;0;Create;True;0;0;0;False;0;False;0.6117647;0.4239594;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;295.4733,222.7102;Inherit;False;Property;_BaseCellOffset004;Base Cell Offset004;12;0;Create;True;0;0;0;False;0;False;0.7676471;0.5945997;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;154;-505.0329,-932.8596;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;161;37.96588,-900.8586;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;180;813.6704,-776.315;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;133;-1980.05,-1143.151;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;134;-2028.244,-1246.498;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;190;-2427.698,-1212.207;Inherit;True;Property;_TextureSample3;Texture Sample 0;16;0;Create;True;0;0;0;False;0;False;-1;9fb015b33574ef74eae24849a3c3e563;9fb015b33574ef74eae24849a3c3e563;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;80;-223.248,276.1733;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-739.6166,-488.3809;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-874.3007,-470.3729;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-817.6157,-152.6478;Inherit;False;Property;_HeighRatioInPut;HeighRatioInPut ;19;0;Create;True;0;0;0;False;0;False;10;1.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1292.985,474.325;Inherit;False;Property;_N_BaseCellOffset001;N_Base Cell Offset001;7;0;Create;True;0;0;0;False;0;False;2.235294;2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-1296.909,543.6647;Inherit;False;Property;_N_BaseCellOffset002;N_Base Cell Offset002;9;0;Create;True;0;0;0;False;0;False;2.235294;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;3;-472.6962,-261.028;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;4;-711.6985,-74.58756;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;258;-900.5056,492.9243;Inherit;False;TextureA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;81;-420.8722,275.3321;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;132;-1829.661,-1144.18;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;135;-1878.278,-1247.428;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;140;-1653.614,-1182.004;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;337;1449.654,-199.8908;Inherit;False;main;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;152;-761.0318,-964.8597;Inherit;False;151;myVarName;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;136;-1500.926,-1183.201;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;137;-1280.266,-1174.816;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;156;-246.6276,-900.8586;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-596.5964,268.985;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;115;-732.6226,432.3348;Inherit;False;Constant;_Float1;Float 1;12;0;Create;True;0;0;0;False;0;False;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;160;-121.0342,-900.8586;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;292.4198,-11.52141;Float;False;Property;_BaseCellOffset001;Base Cell Offset001;6;0;Create;True;0;0;0;False;0;False;0;1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;290.0996,65.57502;Float;False;Property;_BaseCellOffset002;Base Cell Offset002;5;0;Create;True;0;0;0;False;0;False;0.01;1;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;86;-77.158,272.0747;Float;False;NewNormals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;92;1175.902,-90.06375;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;78;-342.7803,801.7413;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;107;940.4765,-93.9166;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;79;-667.0704,877.1754;Inherit;True;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;84;-98.81085,799.1619;Float;True;NdotL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;237;432.3563,1011.784;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FresnelNode;238;618.3026,1014.401;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;244;1057.354,1017.268;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;245;1173.877,1010.75;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;246;921.9734,1017.558;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;243;902.0524,1098.369;Inherit;False;Property;_Float8;Float 7;17;0;Create;True;0;0;0;False;0;False;0.3611765;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;240;1491.42,1008.309;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;263;1309.6,1083.156;Inherit;False;Constant;_Float4;Float 4;25;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;247;1307.709,1007.79;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;421;1005.963,1298.538;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;423;1145.351,1410.913;Float;False;Property;_ShadowContribution;Shadow Contribution;20;0;Create;True;0;0;0;False;0;False;0.5;0.7272412;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightPos;420;797.1499,1368.223;Inherit;False;0;3;FLOAT4;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.LerpOp;425;1444.94,1260.723;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;422;1153.103,1312.821;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;424;1284.977,1315.781;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;-1336.246,276.1215;Inherit;True;Property;_TextureSample2;Texture Sample 0;18;0;Create;True;0;0;0;False;0;False;-1;4043a05d39af1aa4eb2717e1db3c205b;9138feb8dc43c494395caba74376a3a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-923.0295,307.838;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-971.2267,204.4907;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;27;-821.2607,203.5599;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-772.6457,306.8092;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-205.9466,-293.0869;Inherit;True;Property;_TextureSample1;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;df2bad5f2f660604eb5627fdab9245e1;df2bad5f2f660604eb5627fdab9245e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-1105.558,-1176.856;Inherit;False;myVarName;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-293.1832,-788.8366;Inherit;False;Property;_ReflectOffset001;Reflect Off set001;13;0;Create;True;0;0;0;False;0;False;4.393466;2.588895;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;198.966,-900.8586;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;165;326.9655,-900.8586;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;446;460.8544,-903.8993;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;168;627.4041,-901.2466;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;170;806.9636,-916.8596;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;172;963.0723,-931.0566;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-0.25;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;175;1110.965,-1092.86;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;85;-708.4781,743.5727;Inherit;False;86;NewNormals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;87;309.5573,-133.0799;Inherit;False;84;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;109;1053.29,-93.13723;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;108;816.3765,-99.4661;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;91;696.8176,-100.1431;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;567.4832,-105.9456;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;139;-2400,-1014.665;Inherit;False;Property;_R_BaseCellOffset001;R_Base Cell Offset001;8;0;Create;True;0;0;0;False;0;False;2.235294;2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;141;-2397.925,-857.3234;Inherit;False;Property;_R_BaseCellOffset002;R_Base Cell Offset002;10;0;Create;True;0;0;0;False;0;False;2.235294;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;142;-1789.638,-1018.655;Inherit;False;Constant;_Float2;Float 1;12;0;Create;True;0;0;0;False;0;False;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;173;806.9636,-1252.86;Inherit;False;Property;_ReflectOff01;Reflect Off01;0;0;Create;True;0;0;0;False;0;False;0.8588235,0.5792313,0,1;0.2030675,0.278274,0.3490566,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;174;806.9636,-1092.86;Inherit;False;Property;_Reflect02;Reflect02;1;0;Create;True;0;0;0;False;0;False;0.6509434,0.6076939,0.4339622,1;0.3163734,0.2651596,0.3867925,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;174.7075,-500.4691;Inherit;False;Property;_diffusegradient01;diffusegradient01;2;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.5817609,0.7941998,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;174.7075,-335.4592;Inherit;False;Property;_diffusegradient02;diffusegradient02;3;0;Create;True;0;0;0;False;0;False;0.3509552,0.6132076,0.5379074,1;0.6132076,0.6132076,0.6132076,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1918.966,-810.0084;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Aqin/Character/Less;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;1;False;;3;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;21;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.LightAttenuation;419;809.9371,1253.479;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;1447.968,-1077.86;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;128;1294.168,-215.0024;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
WireConnection;176;0;175;0
WireConnection;176;1;181;0
WireConnection;181;0;180;0
WireConnection;182;0;160;0
WireConnection;13;0;15;0
WireConnection;13;1;14;0
WireConnection;13;2;5;1
WireConnection;154;0;152;0
WireConnection;161;0;160;0
WireConnection;161;1;162;0
WireConnection;180;0;182;0
WireConnection;133;0;190;2
WireConnection;133;1;139;0
WireConnection;134;0;190;1
WireConnection;134;1;139;0
WireConnection;80;0;81;0
WireConnection;7;0;12;0
WireConnection;3;0;7;0
WireConnection;3;1;50;3
WireConnection;3;2;2;0
WireConnection;3;3;4;3
WireConnection;258;0;50;4
WireConnection;81;0;30;0
WireConnection;132;0;133;0
WireConnection;132;1;141;0
WireConnection;135;0;134;0
WireConnection;135;1;141;0
WireConnection;140;0;132;0
WireConnection;140;1;135;0
WireConnection;140;2;142;0
WireConnection;337;0;128;0
WireConnection;136;0;140;0
WireConnection;137;0;136;0
WireConnection;156;0;154;0
WireConnection;156;1;155;0
WireConnection;30;0;28;0
WireConnection;30;1;27;0
WireConnection;30;2;115;0
WireConnection;160;0;156;0
WireConnection;86;0;80;0
WireConnection;92;0;109;0
WireConnection;78;0;85;0
WireConnection;78;1;79;0
WireConnection;107;0;108;0
WireConnection;84;0;78;0
WireConnection;238;4;237;0
WireConnection;244;0;246;0
WireConnection;244;1;243;0
WireConnection;245;0;244;0
WireConnection;246;0;238;0
WireConnection;240;0;247;0
WireConnection;240;1;263;0
WireConnection;247;0;245;0
WireConnection;421;0;419;0
WireConnection;425;0;424;0
WireConnection;425;2;423;0
WireConnection;422;0;421;0
WireConnection;422;1;420;2
WireConnection;424;0;422;0
WireConnection;26;0;50;2
WireConnection;26;1;53;0
WireConnection;24;0;50;1
WireConnection;24;1;53;0
WireConnection;27;0;24;0
WireConnection;27;1;114;0
WireConnection;28;0;26;0
WireConnection;28;1;114;0
WireConnection;5;1;3;0
WireConnection;151;0;137;0
WireConnection;163;0;161;0
WireConnection;163;1;164;0
WireConnection;165;0;163;0
WireConnection;446;0;165;0
WireConnection;168;0;446;0
WireConnection;170;0;168;0
WireConnection;172;0;170;0
WireConnection;175;0;173;0
WireConnection;175;1;174;0
WireConnection;175;2;172;0
WireConnection;109;0;107;0
WireConnection;109;1;111;0
WireConnection;108;0;91;0
WireConnection;108;1;110;0
WireConnection;91;0;89;0
WireConnection;91;1;90;0
WireConnection;89;0;87;0
WireConnection;89;1;88;0
WireConnection;0;2;177;0
WireConnection;0;13;337;0
WireConnection;177;0;176;0
WireConnection;177;1;178;0
WireConnection;128;0;13;0
WireConnection;128;1;92;0
ASEEND*/
//CHKSM=94EBAC8F6601DBE32EF56AF8B3E1A9C39CF04211