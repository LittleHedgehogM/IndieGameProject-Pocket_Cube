// Made with Amplify Shader Editor v1.9.1.8
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Aqin/Shadertest004"
{
	Properties
	{
		_diffusegradient01("diffusegradient01", Color) = (1,1,1,1)
		_diffusegradient02("diffusegradient02", Color) = (0.3509552,0.6132076,0.5379074,1)
		_TextureSample1("Texture Sample 0", 2D) = "white" {}
		_OutlineWidth("Outline Width", Range( 0 , 0.2)) = 0.02
		_TextureSample2("Texture Sample 0", 2D) = "white" {}
		_HeighRatioInPut("HeighRatioInPut ", Range( 0 , 10)) = 10
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 15
		_Color0("Color 0", Color) = (0,0,0,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ }
		ZWrite On
		Cull Front
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface outlineSurf Outline nofog  keepalpha noshadow noambient novertexlights nolightmap nodynlightmap nodirlightmap nometa noforwardadd vertex:outlineVertexDataFunc tessellate:tessFunction 
		
		void outlineVertexDataFunc( inout appdata_full v )
		{
			float2 uv_TextureSample2 = v.texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode50 = tex2Dlod( _TextureSample2, float4( uv_TextureSample2, 0, 0.0) );
			float TextureA258 = tex2DNode50.a;
			float outlineVar = ( _OutlineWidth * TextureA258 );
			v.vertex.xyz *= ( 1 + outlineVar);
		}
		inline half4 LightingOutline( SurfaceOutput s, half3 lightDir, half atten ) { return half4 ( 0,0,0, s.Alpha); }
		void outlineSurf( Input i, inout SurfaceOutput o )
		{
			o.Emission = _Color0.rgb;
		}
		ENDCG
		

		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		ZWrite On
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

		uniform float4 _diffusegradient01;
		uniform float4 _diffusegradient02;
		uniform sampler2D _TextureSample1;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform float _HeighRatioInPut;
		uniform float _EdgeLength;
		uniform float4 _Color0;
		uniform float _OutlineWidth;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			v.vertex.xyz += 0;
			v.vertex.w = 1;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float2 temp_cast_0 = (1.0).xx;
			float2 uv_TexCoord7 = i.uv_texcoord * temp_cast_0;
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			float4 tex2DNode50 = tex2D( _TextureSample2, uv_TextureSample2 );
			float3 temp_cast_1 = (i.viewDir.z).xxx;
			float2 Offset3 = ( ( tex2DNode50.b - 1 ) * temp_cast_1.xy * _HeighRatioInPut ) + uv_TexCoord7;
			float4 lerpResult13 = lerp( _diffusegradient01 , _diffusegradient02 , tex2D( _TextureSample1, Offset3 ).r);
			c.rgb = lerpResult13.rgb;
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
Node;AmplifyShaderEditor.CommentaryNode;117;128.8725,-567.4103;Inherit;False;1517.705;868.1461;Toon diffuse;4;89;15;14;13;Toon diffuse;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;46;-914.1636,-552.5628;Inherit;False;1024.409;657.902;Comment;6;2;3;4;12;5;7;Base;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;76;-1382.066,128.0586;Inherit;False;1503.606;511.2192;Comment;2;50;258;Normals;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;77;-717.0706,714.9427;Inherit;False;835.6508;341.2334;Comment;0;N dot L;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;130;-2472.802,-1312.93;Inherit;False;1601.529;532.0251;;0;Reflection;0.5220588,0.6044625,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;13;620.1099,-240.9034;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-739.6166,-488.3809;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-874.3007,-470.3729;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-817.6157,-152.6478;Inherit;False;Property;_HeighRatioInPut;HeighRatioInPut ;5;0;Create;True;0;0;0;False;0;False;10;1.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxMappingNode;3;-472.6962,-261.028;Inherit;False;Normal;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;4;-711.6985,-74.58756;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;258;-900.5056,492.9243;Inherit;False;TextureA;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;-1336.246,276.1215;Inherit;True;Property;_TextureSample2;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;4043a05d39af1aa4eb2717e1db3c205b;9138feb8dc43c494395caba74376a3a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-205.9466,-293.0869;Inherit;True;Property;_TextureSample1;Texture Sample 0;2;0;Create;True;0;0;0;False;0;False;-1;df2bad5f2f660604eb5627fdab9245e1;df2bad5f2f660604eb5627fdab9245e1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;89;567.4832,-105.9456;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;174.7075,-500.4691;Inherit;False;Property;_diffusegradient01;diffusegradient01;0;0;Create;True;0;0;0;False;0;False;1,1,1,1;0.07450981,0.6156863,0.9254902,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;14;174.7075,-335.4592;Inherit;False;Property;_diffusegradient02;diffusegradient02;1;0;Create;True;0;0;0;False;0;False;0.3509552,0.6132076,0.5379074,1;0.08235294,0.4666667,0.7529412,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1918.966,-810.0084;Float;False;True;-1;6;ASEMaterialInspector;0;0;CustomLighting;Aqin/Shadertest004;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;6;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;447;732.2477,377.9642;Inherit;False;919.9573;567.8873;Comment;5;456;455;453;451;448;Custom Outline;1,0.6029412,0.7097364,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;451;1124.438,799.2072;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;456;782.0641,792.6943;Float;False;Property;_OutlineWidth;Outline Width;3;0;Create;True;0;0;0;False;0;False;0.02;0.039;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;455;1112.104,490.8823;Inherit;False;Property;_Color0;Color 0;11;0;Create;True;0;0;0;False;0;False;0,0,0,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;448;871.8884,863.5963;Inherit;False;258;TextureA;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OutlineNode;453;1429.739,511.3799;Inherit;False;1;True;None;1;0;Front;True;True;True;True;0;False;;3;0;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT3;0
WireConnection;13;0;15;0
WireConnection;13;1;14;0
WireConnection;13;2;5;1
WireConnection;7;0;12;0
WireConnection;3;0;7;0
WireConnection;3;1;50;3
WireConnection;3;2;2;0
WireConnection;3;3;4;3
WireConnection;258;0;50;4
WireConnection;5;1;3;0
WireConnection;0;13;13;0
WireConnection;0;11;453;0
WireConnection;451;0;456;0
WireConnection;451;1;448;0
WireConnection;453;0;455;0
WireConnection;453;1;451;0
ASEEND*/
//CHKSM=61CB2E985E8FCC305A270CB4F4062A8C3F46D4AD