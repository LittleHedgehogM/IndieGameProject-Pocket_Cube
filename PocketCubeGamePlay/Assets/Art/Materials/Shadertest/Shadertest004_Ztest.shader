// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aqin/Shadertest004/Ztest_Great"
{
	Properties
	{
		_diffusegradient2("diffusegradient01", Color) = (1,1,1,1)
		_diffusegradient3("diffusegradient02", Color) = (0.3509552,0.6132076,0.5379074,1)
		_TextureSample4("Texture Sample 0", 2D) = "white" {}
		_TextureSample3("Texture Sample 0", 2D) = "white" {}
		_HeighRatioInPut1("HeighRatioInPut ", Range( 0 , 10)) = 10
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+1" }
		Cull Back
		Stencil
		{
			Ref 1
			Comp Equal
			Pass Keep
			Fail Keep
		}
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Tessellation.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
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

		uniform float4 _diffusegradient2;
		uniform float4 _diffusegradient3;
		uniform sampler2D _TextureSample4;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform float _HeighRatioInPut1;
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
			float2 temp_cast_0 = (1.0).xx;
			float2 uv_TexCoord471 = i.uv_texcoord * temp_cast_0;
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float4 tex2DNode476 = tex2D( _TextureSample3, uv_TextureSample3 );
			float3 temp_cast_1 = (i.viewDir.z).xxx;
			float2 Offset474 = ( ( tex2DNode476.b - 1 ) * temp_cast_1.xy * _HeighRatioInPut1 ) + uv_TexCoord471;
			float4 lerpResult470 = lerp( _diffusegradient2 , _diffusegradient3 , tex2D( _TextureSample4, Offset474 ).r);
			c.rgb = lerpResult470.rgb;
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
				vertexDataFunc( v );
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
				surfIN.viewDir = worldViewDir;
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
Node;AmplifyShaderEditor.CommentaryNode;460;-213.7612,-3092.071;Inherit;False;1517.705;868.1461;Toon diffuse;3;480;479;470;Toon diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;461;-1256.797,-3077.224;Inherit;False;1024.409;657.902;Comment;6;477;475;474;473;472;471;Base;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;462;-1724.7,-2396.603;Inherit;False;1503.606;511.2192;Comment;2;476;469;Normals;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;469;-1243.139,-2031.737;Inherit;False;TextureA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;470;277.4763,-2765.564;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;471;-1082.25,-3013.042;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;472;-1216.934,-2995.034;Inherit;False;Constant;_Float2;Float 0;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;473;-1160.249,-2677.309;Inherit;False;Property;_HeighRatioInPut1;HeighRatioInPut ;4;0;Create;True;0;0;0;False;0;False;10;1.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;474;-815.3298,-2785.689;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;475;-1054.332,-2599.249;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;476;-1678.88,-2248.54;Inherit;True;Property;_TextureSample3;Texture Sample 0;3;0;Create;True;0;0;0;False;0;False;-1;4043a05d39af1aa4eb2717e1db3c205b;9138feb8dc43c494395caba74376a3a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;477;-548.5803,-2817.748;Inherit;True;Property;_TextureSample4;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;df2bad5f2f660604eb5627fdab9245e1;df2bad5f2f660604eb5627fdab9245e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;479;-167.9262,-3025.13;Inherit;False;Property;_diffusegradient2;diffusegradient01;0;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.07450981,0.6156863,0.9254902,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;480;-167.9262,-2860.12;Inherit;False;Property;_diffusegradient3;diffusegradient02;1;0;Create;True;0;0;0;False;0;False;0.3509552,0.6132076,0.5379074,1;0.08235294,0.4666667,0.7529412,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1463.366,-2726.918;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Aqin/Shadertest004/Ztest_Great;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;1;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;True;1;False;;255;False;;255;False;;5;False;;1;False;;1;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Absolute;0;;-1;-1;-1;5;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;469;0;476;4
WireConnection;470;0;479;0
WireConnection;470;1;480;0
WireConnection;470;2;477;1
WireConnection;471;0;472;0
WireConnection;474;0;471;0
WireConnection;474;1;476;3
WireConnection;474;2;473;0
WireConnection;474;3;475;3
WireConnection;477;1;474;0
WireConnection;0;13;470;0
ASEEND*/
//CHKSM=9B7C53C94D74C76853317B584EFCB972C1F45684