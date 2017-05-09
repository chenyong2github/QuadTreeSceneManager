// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

///////////////////////////////////////////
// author     : chen yong
// create time: 2015/7/15
// modify time: 2016/3/22, convert original surf to vert/frag mode
// description: Add flash point lighting in the fragment of diffuse rendering
///////////////////////////////////////////

Shader "Kingsoft/Scene/Road" {

	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base", 2D) = "white" {}		
		_PointLightPosXYZRangeW ("PointLight Pos(XYZ) Range(W)", Vector) = (0,0,0,0)
		_PointLightColor ("PointLight Color", Color) = (1,1,1,1)	
	}

	CGINCLUDE		
	#include "UnityCG.cginc"
	struct v2f_full
	{
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
		half4 worldPos : TEXCOORD1;
		half3 normalDir : TEXCOORD2;
		UNITY_FOG_COORDS(3)
		#ifdef LIGHTMAP_ON
			half2 uvLM : TEXCOORD4;
		#endif		
	};


	half4 _Color;
	sampler2D _MainTex;
	float4 _MainTex_ST;
	// sampler2D unity_Lightmap;
	// float4 unity_LightmapST;

	half4 _PointLightPosXYZRangeW;
	half4 _PointLightColor; 
			
	ENDCG 

	SubShader {
		Tags { "RenderType"="Opaque" "LIGHTMODE"="ForwardBase" }

		LOD 300 

		Pass {

			CGPROGRAM
					
			v2f_full vert (appdata_full v) 
			{
				v2f_full o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				UNITY_TRANSFER_FOG(o, o.pos);
				o.uv.xy = TRANSFORM_TEX(v.texcoord,_MainTex);
				#ifdef LIGHTMAP_ON
					o.uvLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif
				o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				
				return o; 
			}
				
			fixed4 frag (v2f_full i) : COLOR0 
			{						
				fixed4 tex = tex2D (_MainTex, i.uv.xy)*_Color;

				#ifdef LIGHTMAP_ON
					fixed3 lm = ( DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM)));
					tex.rgb *= lm;
				#endif

				fixed4 flashColor = fixed4(0, 0, 0, 0);				
				#ifdef FlashLight_On
					half d = distance(_PointLightPosXYZRangeW.xyz, i.worldPos);
					half3 flashattenColor = _PointLightColor.rgb*pow(max(0, _PointLightPosXYZRangeW.w-d), 2);

					half3 flashlightDirection = normalize(_PointLightPosXYZRangeW.xyz - i.worldPos.xyz);
					half NdotL = max(0.0,dot( i.normalDir, flashlightDirection ));
					half directDiffuse = max( 0.0, NdotL) * flashattenColor;
					flashColor.rgb = directDiffuse * tex.rgb;
					//flashColor = 0;
				#endif
			
				tex += flashColor;
				UNITY_APPLY_FOG(i.fogCoord, tex);

				return tex;
			}	
		
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
			#pragma multi_compile FlashLight_On FlashLight_Off
			#pragma fragmentoption ARB_precision_hint_fastest 
			#pragma multi_compile_fog
	
			ENDCG
		}
	}
}
