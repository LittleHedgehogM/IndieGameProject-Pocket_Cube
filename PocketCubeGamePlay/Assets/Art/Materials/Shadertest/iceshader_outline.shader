// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aqin/iceshader_outline"
{
	Properties
	{
		_Basecolor("Base color", Color) = (1,1,1,0.1254902)
		_Reflect02("Reflect02", Color) = (0.6509434,0.6076939,0.4339622,1)
		_ReflectOffset001("Reflect Off set001", Range( 0 , 10)) = 4.393466
		_ReflectPow("ReflectPow", Range( 0 , 1)) = 0.5
		_ReflectOffset002("Reflect Off set002", Range( 0 , 10)) = 2
		_Fresnel("Fresnel", Range( 0 , 10)) = 2
		_Fresnel1("Fresnel", Range( 0 , 10)) = 2
		_TextureSample3("Texture Sample 0", 2D) = "white" {}
		_Float3("Float 0", Range( -10 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
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
		uniform float _Float3;
		uniform float _Fresnel;
		uniform float _Fresnel1;

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
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float dotResult58 = dot( ase_worldViewDir , ase_worldNormal );
			float temp_output_87_0 = ( _Float3 * ( ( saturate( pow( ( 1.0 - saturate( abs( dotResult58 ) ) ) , _Fresnel ) ) * _Fresnel1 ) + 0.0 ) );
			o.Emission = ( ( ( lerpResult31 * saturate( round( temp_output_22_0 ) ) ) * _ReflectPow ) + temp_output_87_0 ).rgb;
			o.Alpha = ( _Basecolor.a + temp_output_87_0 );
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
Node;AmplifyShaderEditor.CommentaryNode;2;-706.0278,-471.0947;Inherit;False;2521.521;726.7368;reflect;24;80;78;9;24;28;26;25;11;37;4;8;31;5;6;12;30;1;36;22;21;10;7;87;88;Reflect02;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;3;-1750.497,-470.3588;Inherit;False;978.5253;374.6113;;5;15;34;20;19;18;Reflection;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-1305.996,-377.1236;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;19;-1153.308,-378.3206;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;20;-932.6482,-369.9355;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1442.02,-213.7747;Inherit;False;Constant;_Float2;Float 1;12;0;Create;True;0;0;0;False;0;False;0.95;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-1722.58,-408.6264;Inherit;True;Property;_TextureSample3;Texture Sample 0;7;0;Create;True;0;0;0;False;0;False;-1;9fb015b33574ef74eae24849a3c3e563;9fb015b33574ef74eae24849a3c3e563;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;7;-53.06639,93.01481;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ReflectOpNode;10;-628.1889,-88.0715;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;21;-369.7835,-56.07063;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;22;-244.1907,-56.07063;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;36;683.808,-248.0719;Inherit;False;Property;_Reflect02;Reflect02;1;0;Create;True;0;0;0;False;0;False;0.6509434,0.6076939,0.4339622,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;1476.901,-426.9053;Inherit;False;Property;_Basecolor;Base color;0;0;Create;True;0;0;0;False;0;False;1,1,1,0.1254902;1,1,1,0.1411765;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;30;839.9169,-86.26846;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;-0.25;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;12;697.5147,147.4731;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;6;844.3849,124.6655;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;1372.813,-228.0719;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;31;987.8099,-248.0719;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;1066.811,-5.071634;Inherit;False;Property;_ReflectPow;ReflectPow;3;0;Create;True;0;0;0;False;0;False;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;1141.811,-236.0718;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;37;-677.4882,7.929477;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;11;-89.0639,-64.00002;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;134.9357,16;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;26;342.9367,-48;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;517.2485,-69.45857;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-416.339,55.95138;Inherit;False;Property;_ReflectOffset001;Reflect Off set001;2;0;Create;True;0;0;0;False;0;False;4.393466;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-412.6679,129.8435;Inherit;False;Property;_ReflectOffset002;Reflect Off set002;4;0;Create;True;0;0;0;False;0;False;2;1.666554;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;78;1678.132,-206.1609;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;58;96.31872,347.7904;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;68;463.1696,350.8217;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;65;218.7708,349.5217;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;71;-131.4779,453.3214;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PowerNode;69;647.7745,349.376;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;67;334.4687,352.1215;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;72;921.0896,373.9726;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;321.7687,440.5221;Inherit;False;Property;_Fresnel;Fresnel;5;0;Create;True;0;0;0;False;0;False;2;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;727.0893,579.9725;Inherit;False;Property;_Fresnel1;Fresnel;6;0;Create;True;0;0;0;False;0;False;2;2.705882;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;1061.089,382.9726;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;56;-163.6818,280.1904;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1916.619,-315.55;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Aqin/iceshader_outline;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.FresnelNode;81;1124.74,154.071;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;80;1689.51,14.48191;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;83;857.8352,225.8824;Inherit;False;Property;_Float1;Float 0;8;0;Create;True;0;0;0;False;0;False;0;1.529398;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;1144.441,81.94069;Inherit;False;Property;_Float3;Float 0;9;0;Create;True;0;0;0;False;0;False;0;0.8235294;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;1443.441,98.94066;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;74;1267.919,407.2258;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
WireConnection;18;0;15;1
WireConnection;18;1;15;2
WireConnection;18;2;34;0
WireConnection;19;0;18;0
WireConnection;20;0;19;0
WireConnection;7;0;22;0
WireConnection;10;0;20;0
WireConnection;21;0;10;0
WireConnection;21;1;37;0
WireConnection;22;0;21;0
WireConnection;30;0;28;0
WireConnection;12;0;7;0
WireConnection;6;0;12;0
WireConnection;5;0;4;0
WireConnection;5;1;8;0
WireConnection;31;1;36;0
WireConnection;31;2;30;0
WireConnection;4;0;31;0
WireConnection;4;1;6;0
WireConnection;11;0;22;0
WireConnection;11;1;24;0
WireConnection;25;0;11;0
WireConnection;25;1;9;0
WireConnection;26;0;25;0
WireConnection;28;0;26;0
WireConnection;78;0;1;4
WireConnection;78;1;87;0
WireConnection;58;0;56;0
WireConnection;58;1;71;0
WireConnection;68;0;67;0
WireConnection;65;0;58;0
WireConnection;69;0;68;0
WireConnection;69;1;70;0
WireConnection;67;0;65;0
WireConnection;72;0;69;0
WireConnection;73;0;72;0
WireConnection;73;1;75;0
WireConnection;0;0;1;0
WireConnection;0;2;80;0
WireConnection;0;9;78;0
WireConnection;81;2;83;0
WireConnection;80;0;5;0
WireConnection;80;1;87;0
WireConnection;87;0;88;0
WireConnection;87;1;74;0
WireConnection;74;0;73;0
ASEEND*/
//CHKSM=21340DFA36700AB00243F0B23F5C8C086701A628