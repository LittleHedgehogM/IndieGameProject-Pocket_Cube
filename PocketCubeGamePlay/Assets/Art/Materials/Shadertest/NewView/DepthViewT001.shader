// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aqin/DepthView001/ViewT"
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
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
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
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+1" "IsEmissive" = "true"  }
		Cull Back
		ZWrite On
		Stencil
		{
			Ref 1
			Comp Equal
			Pass Keep
			Fail Keep
		}
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
			float2 uv_TexCoord362 = i.uv_texcoord * temp_cast_2;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode390 = tex2D( _TextureSample2, uv_TextureSample2 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 temp_cast_3 = (ase_worldViewDir.z).xxx;
			float2 Offset367 = ( ( tex2DNode390.b - 1 ) * temp_cast_3.xy * _HeighRatioInPut ) + uv_TexCoord362;
			float4 lerpResult351 = lerp( _diffusegradient01 , _diffusegradient02 , tex2D( _TextureSample1, Offset367 ).r);
			float4 appendResult379 = (float4(( ( tex2DNode390.g * _N_BaseCellOffset001 ) - _N_BaseCellOffset002 ) , ( ( tex2DNode390.r * _N_BaseCellOffset001 ) - _N_BaseCellOffset002 ) , 0.95 , 0.0));
			float3 normalizeResult361 = normalize( (WorldNormalVector( i , appendResult379.xyz )) );
			float3 NewNormals384 = normalizeResult361;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult386 = dot( NewNormals384 , ase_worldlightDir );
			float NdotL389 = dotResult386;
			float4 main374 = ( lerpResult351 * saturate( ( round( ( ( ( NdotL389 + _BaseCellOffset001 ) / _BaseCellOffset002 ) + _BaseCellOffset003 ) ) + _BaseCellOffset004 ) ) );
			c.rgb = main374.rgb;
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
			float4 tex2DNode360 = tex2D( _TextureSample3, uv_TextureSample3 );
			float4 appendResult373 = (float4(( ( tex2DNode360.g * _R_BaseCellOffset001 ) - _R_BaseCellOffset002 ) , ( ( tex2DNode360.r * _R_BaseCellOffset001 ) - _R_BaseCellOffset002 ) , 0.95 , 0.0));
			float3 normalizeResult377 = normalize( (WorldNormalVector( i , appendResult373.xyz )) );
			float3 myVarName396 = normalizeResult377;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult378 = dot( reflect( myVarName396 , float3( 0,0,0 ) ) , ase_worldlightDir );
			float temp_output_381_0 = saturate( dotResult378 );
			float clampResult403 = clamp( ( ( ( round( ( pow( temp_output_381_0 , _ReflectOffset001 ) * _ReflectOffset002 ) ) * 0.5 ) - 1.0 ) * 2.0 ) , -0.25 , 1.0 );
			float4 lerpResult404 = lerp( _ReflectOff01 , _Reflect02 , clampResult403);
			o.Emission = ( ( lerpResult404 * saturate( round( temp_output_381_0 ) ) ) * _ReflectPow ).rgb;
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
Node;AmplifyShaderEditor.CommentaryNode;338;-547.6252,-1121.428;Inherit;False;2456.841;703.4816;reflect;23;415;414;404;403;402;401;400;399;398;397;381;378;375;356;355;354;350;349;348;347;346;345;344;Reflect02;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;339;413.1826,-372.9557;Inherit;False;1517.705;868.1461;Toon diffuse;16;417;416;410;409;408;407;406;387;385;383;382;374;357;353;352;351;Toon diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;340;-629.8535,-358.1082;Inherit;False;1024.409;657.902;Comment;6;395;368;367;364;363;362;Base;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;341;-1097.756,322.5132;Inherit;False;1503.606;511.2192;Comment;13;394;393;392;391;390;384;380;379;370;369;366;365;361;Normals;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;342;-432.7605,909.3972;Inherit;False;835.6508;341.2334;Comment;4;405;389;388;386;N dot L;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;343;-2188.492,-1118.475;Inherit;False;1601.529;532.0251;;12;413;412;411;396;377;376;373;372;371;360;359;358;Reflection;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;344;-268.7221,-642.4039;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;345;1571.276,-882.4054;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;346;1731.278,-882.4054;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;347;1252.85,-581.6681;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;348;354.3999,-557.3187;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;349;1433.276,-762.4052;Inherit;False;Property;_ReflectPow;ReflectPow;19;0;Create;True;0;0;0;False;0;False;0.5;0.5705882;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;350;-5.201996,-520.49;Inherit;False;Property;_ReflectOffset002;Reflect Off set002;20;0;Create;True;0;0;0;False;0;False;2;3.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;351;904.42,-46.44884;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;352;574.5133,333.0334;Inherit;False;Property;_BaseCellOffset003;Base Cell Offset003;16;0;Create;True;0;0;0;False;0;False;0.6117647;0.4836274;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;353;579.7834,417.1648;Inherit;False;Property;_BaseCellOffset004;Base Cell Offset004;17;0;Create;True;0;0;0;False;0;False;0.7676471;0.6377987;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;354;-220.7228,-738.405;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;355;322.276,-706.4041;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;356;1097.98,-581.8605;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;357;1600.087,-2.151245;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;358;-1695.74,-948.6964;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;359;-1743.934,-1052.043;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;360;-2143.388,-1017.752;Inherit;True;Property;_TextureSample3;Texture Sample 0;21;0;Create;True;0;0;0;False;0;False;-1;9fb015b33574ef74eae24849a3c3e563;9fb015b33574ef74eae24849a3c3e563;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;361;61.06209,470.6279;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;362;-455.3065,-293.9263;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;363;-589.9906,-275.9183;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;364;-533.3057,41.80676;Inherit;False;Property;_HeighRatioInPut;HeighRatioInPut ;23;0;Create;True;0;0;0;False;0;False;10;1.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;365;-1008.675,668.7795;Inherit;False;Property;_N_BaseCellOffset001;N_Base Cell Offset001;7;0;Create;True;0;0;0;False;0;False;2.235294;2.69;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;366;-1012.599,738.1193;Inherit;False;Property;_N_BaseCellOffset002;N_Base Cell Offset002;14;0;Create;True;0;0;0;False;0;False;2.235294;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;367;-188.3861,-66.57346;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;368;-427.3884,119.867;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;369;-616.1956,687.3788;Inherit;False;TextureA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;370;-136.5621,469.7867;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;371;-1545.351,-949.7255;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;372;-1593.968,-1052.973;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;373;-1369.304,-987.5494;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;374;1733.964,-5.436234;Inherit;False;main;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;375;-476.7217,-770.4052;Inherit;False;396;myVarName;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;376;-1216.616,-988.7465;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;377;-995.9559,-980.3615;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;378;37.6825,-706.4041;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;379;-312.2863,463.4395;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;380;-448.3125,626.7894;Inherit;False;Constant;_Float1;Float 1;12;0;Create;True;0;0;0;False;0;False;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;381;163.2759,-706.4041;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;382;576.7299,182.9332;Float;False;Property;_BaseCellOffset001;Base Cell Offset001;6;0;Create;True;0;0;0;False;0;False;0;-0.09803539;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;383;574.4097,260.0296;Float;False;Property;_BaseCellOffset002;Base Cell Offset002;5;0;Create;True;0;0;0;False;0;False;0.01;0.5205612;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;384;207.1521,466.5293;Float;False;NewNormals;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;385;1460.212,104.3908;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;386;-58.47021,996.1958;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;387;1224.787,100.538;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;388;-382.7603,1071.63;Inherit;True;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;389;185.4992,993.6165;Float;True;NdotL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;390;-1051.936,470.576;Inherit;True;Property;_TextureSample2;Texture Sample 0;22;0;Create;True;0;0;0;False;0;False;-1;4043a05d39af1aa4eb2717e1db3c205b;9138feb8dc43c494395caba74376a3a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;391;-638.7194,502.2926;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;392;-686.9166,398.9453;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;393;-536.9506,398.0145;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;394;-488.3356,501.2638;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;395;78.36349,-98.63235;Inherit;True;Property;_TextureSample1;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;df2bad5f2f660604eb5627fdab9245e1;df2bad5f2f660604eb5627fdab9245e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;396;-821.2479,-982.4014;Inherit;False;myVarName;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;397;-8.873108,-594.3821;Inherit;False;Property;_ReflectOffset001;Reflect Off set001;18;0;Create;True;0;0;0;False;0;False;4.393466;2.588895;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;398;483.2761,-706.4041;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;399;611.2756,-706.4041;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;400;745.1645,-709.4447;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;401;911.7142,-706.792;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;402;1091.274,-722.405;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;403;1247.382,-736.6021;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-0.25;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;404;1395.275,-898.4054;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;405;-424.168,938.0272;Inherit;False;384;NewNormals;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;406;593.8674,61.37466;Inherit;False;389;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;407;1337.6,101.3173;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;408;1100.687,94.98846;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;409;981.1277,94.31146;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;410;851.7933,88.50896;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;411;-2115.69,-820.2104;Inherit;False;Property;_R_BaseCellOffset001;R_Base Cell Offset001;8;0;Create;True;0;0;0;False;0;False;2.235294;2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;412;-2113.615,-662.8689;Inherit;False;Property;_R_BaseCellOffset002;R_Base Cell Offset002;15;0;Create;True;0;0;0;False;0;False;2.235294;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;413;-1505.328,-824.2004;Inherit;False;Constant;_Float2;Float 1;12;0;Create;True;0;0;0;False;0;False;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;415;1091.274,-898.4054;Inherit;False;Property;_Reflect02;Reflect02;1;0;Create;True;0;0;0;False;0;False;0.6509434,0.6076939,0.4339622,1;0.9056604,0.5525097,0.5525097,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;416;459.0176,-306.0145;Inherit;False;Property;_diffusegradient01;diffusegradient01;2;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.5,0.01242236,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;417;459.0176,-141.0046;Inherit;False;Property;_diffusegradient02;diffusegradient02;3;0;Create;True;0;0;0;False;0;False;0.3509552,0.6132076,0.5379074,1;0.6509434,0.1116815,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;414;1091.274,-1058.405;Inherit;False;Property;_ReflectOff01;Reflect Off01;0;0;Create;True;0;0;0;False;0;False;0.8588235,0.5792313,0,1;0.3962264,0.3301886,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2218.801,-480.1891;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Aqin/DepthView001/ViewT;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;1;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;True;1;False;;255;False;;255;False;;5;False;;1;False;;1;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Absolute;0;;-1;-1;-1;9;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;345;0;404;0
WireConnection;345;1;347;0
WireConnection;346;0;345;0
WireConnection;346;1;349;0
WireConnection;347;0;356;0
WireConnection;348;0;381;0
WireConnection;351;0;416;0
WireConnection;351;1;417;0
WireConnection;351;2;395;1
WireConnection;354;0;375;0
WireConnection;355;0;381;0
WireConnection;355;1;397;0
WireConnection;356;0;348;0
WireConnection;357;0;351;0
WireConnection;357;1;385;0
WireConnection;358;0;360;2
WireConnection;358;1;411;0
WireConnection;359;0;360;1
WireConnection;359;1;411;0
WireConnection;361;0;370;0
WireConnection;362;0;363;0
WireConnection;367;0;362;0
WireConnection;367;1;390;3
WireConnection;367;2;364;0
WireConnection;367;3;368;3
WireConnection;369;0;390;4
WireConnection;370;0;379;0
WireConnection;371;0;358;0
WireConnection;371;1;412;0
WireConnection;372;0;359;0
WireConnection;372;1;412;0
WireConnection;373;0;371;0
WireConnection;373;1;372;0
WireConnection;373;2;413;0
WireConnection;374;0;357;0
WireConnection;376;0;373;0
WireConnection;377;0;376;0
WireConnection;378;0;354;0
WireConnection;378;1;344;0
WireConnection;379;0;394;0
WireConnection;379;1;393;0
WireConnection;379;2;380;0
WireConnection;381;0;378;0
WireConnection;384;0;361;0
WireConnection;385;0;407;0
WireConnection;386;0;405;0
WireConnection;386;1;388;0
WireConnection;387;0;408;0
WireConnection;389;0;386;0
WireConnection;391;0;390;2
WireConnection;391;1;365;0
WireConnection;392;0;390;1
WireConnection;392;1;365;0
WireConnection;393;0;392;0
WireConnection;393;1;366;0
WireConnection;394;0;391;0
WireConnection;394;1;366;0
WireConnection;395;1;367;0
WireConnection;396;0;377;0
WireConnection;398;0;355;0
WireConnection;398;1;350;0
WireConnection;399;0;398;0
WireConnection;400;0;399;0
WireConnection;401;0;400;0
WireConnection;402;0;401;0
WireConnection;403;0;402;0
WireConnection;404;0;414;0
WireConnection;404;1;415;0
WireConnection;404;2;403;0
WireConnection;407;0;387;0
WireConnection;407;1;353;0
WireConnection;408;0;409;0
WireConnection;408;1;352;0
WireConnection;409;0;410;0
WireConnection;409;1;383;0
WireConnection;410;0;406;0
WireConnection;410;1;382;0
WireConnection;0;2;346;0
WireConnection;0;13;374;0
ASEEND*/
//CHKSM=200925F57C872597802967115A1BAF2CDCDE452C