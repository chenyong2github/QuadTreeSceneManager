// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Area/Water/StaticWater"{
	
	Properties
	{
		_NormalMap("NormalMap" ,2D) = "black" {}
		_BumpMap0("Diffuse0" ,2D) = "black" {}
		_BumpMap1("EdgeMap" ,2D) = "black" {}
		_BankMap("BankMap" , 2D) = "black" {}
		_Foam0 ("Foam0" ,2D) = "black"{}
		_Foam1 ("Foam1" , 2D) = "black"{}
		_ScaleMainColor("ScaleMainColor" , Range(0.0, 1.0)) = 0.3 
		_MainColor("MainColor" , Color) = (0.1, 0.1 ,0.6, 1.0)
		_Shininess("Shininess", Float) = 2 
		_Strength ("Strength" , Range(0 , 0.5)) = 0 
		_FoamSpeed1("Foam Speed 1" , Range(0, 0.5)) = 0  
		_FoamSpeed2("Foam Speed 2" , Range(0 ,0.5)) = 0 
		_FoamControl("FoamControl" , Range(0 , 2)) = 0 
		_FoamScale("FoamScale" , Range(0, 1)) = 0 
		_FadingSineControl("Fading Sine Control", Range(0, 10)) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"}		
		AlphaTest Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag 
			#pragma multi_compile_fog
			#pragma target 3.0
			#include "UnityCG.cginc"

			
			sampler2D _NormalMap ; float4 _NormalMap_ST ;
			sampler2D _BumpMap0 ;float4 _BumpMap0_ST ;
			sampler2D _BumpMap1 ;
			sampler2D _BankMap ;
			sampler2D _Foam0 ; float4 _Foam0_ST ;
			sampler2D _Foam1 ; float4 _Foam1_ST ;
			float4 _MainColor ;
			float _Strength ;
			float4 _TimeEditor ;
			float _FoamSpeed1 ;
			float _FoamSpeed2 ;
			float _FoamControl ;
			
			float _ScaleMainColor ;
			float _Shininess ;
			float _FoamScale ;
			float _FadingSineControl;
			
			struct appdata
			{
				float4 vertex : POSITION ;
				float3 normal : NORMAL ;
				float2 textCoord : TEXCOORD0 ;				
				float4 vertexColor :COLOR ;
			};

			struct v2f
			{
				float4 pos: SV_POSITION ;
				float2 reflUV : TEXCOORD0 ;
				float2 refbumpUV : TEXCOORD1 ;				
				float3 normalDir :TEXCOORD2 ;
				UNITY_FOG_COORDS(3)				
			};

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				o.reflUV = v.textCoord ;
				o.refbumpUV = v.textCoord * 0.3333 + _Time.x * float2(0.0, 0.5) * 0.1; 
				
				o.normalDir = mul(unity_ObjectToWorld , float4(v.normal , 0)).xyz ;
				float4 time = _Time + _TimeEditor ;
				v.vertex.xyz += (normalize((float3(1,0.5 ,0.5) + v.normal))* v.vertexColor.r * sin(((v.vertexColor.b * 3.141592654) + time.g)) * _Strength) ;
				o.pos = mul(UNITY_MATRIX_MVP , v.vertex) ;
				UNITY_TRANSFER_FOG(o, o.pos);
				return  o ;
			}

			

			half4 frag(v2f i ) : COLOR
			{

				float4 normalColor = tex2D(_NormalMap , TRANSFORM_TEX(i.refbumpUV.xy ,_NormalMap ));
				float2 perturbation = 0.39 * (normalColor.rg - 0.5) ;
				
				float2 bumpUV = saturate(i.reflUV + perturbation * 0.5) ;
				float4 bumpColor0 = tex2D(_BumpMap0 ,TRANSFORM_TEX(bumpUV ,_BumpMap0));	
				float4 bumpColor1 = tex2D(_BumpMap1 ,bumpUV) ;
				float4 backColor = tex2D(_BankMap, bumpUV) ; 
				
				float4  combineColor = lerp(bumpColor0 , _MainColor , _ScaleMainColor) ;
				float4 finalColor = pow( bumpColor1.r  , _Shininess) + combineColor  ; 
				finalColor = lerp(finalColor , backColor ,saturate(bumpColor1.r ));
				
				float4 time = _Time + _TimeEditor ;
				float t = 1.0 - (sin(time.g * 0.025) * 0.5 + 0.5);
				float timeCos = cos(t) ;
				float timeSin = sin(t) ;
				float2 FoamUV0 = (mul(((1.0+(i.reflUV*1.1))+((1.0 - sin((time.g*_FoamSpeed1)))*0.2+0.3)*float2(0,1))
				-i.reflUV,float2x2( timeCos, -timeSin, timeSin, timeCos))+i.reflUV);
				
				float4 FoamColor0  = tex2D(_Foam0 , TRANSFORM_TEX(FoamUV0 ,_Foam0) ) ;
				float2 FoamUV1 = 1.0 - i.reflUV * 1.1;
				FoamUV1 = ((1.0 + FoamUV1)+((1.0 - sin(((_FoamSpeed2+0.005)*time.g)))*0.25+0.5)*float2(0,1));
				float4 FoamColor1 = tex2D(_Foam1 , TRANSFORM_TEX(FoamUV1 , _Foam1))  ;
				
				float scale = cos( 1.0 -(time.g / _FadingSineControl)) ;
				float3 color = lerp(FoamColor0.rgb + FoamColor1.rgb , float3(0.0, 0.0, 0.0) , scale) ;
				finalColor.rgb= finalColor.rgb + color * _FoamScale ;
				//finalColor.rgb = finalColor.rgb + FoamCombineColor *_FoamScale ;
				
				fixed4 col = fixed4(finalColor.rgb, 1);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;				
			}			
			ENDCG
		}
	}
}