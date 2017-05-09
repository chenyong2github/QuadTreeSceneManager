// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.03 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.03;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:7969,x:32719,y:32712,varname:node_7969,prsc:2|diff-9858-RGB,spec-2190-OUT,emission-1885-OUT,amdfl-3059-RGB;n:type:ShaderForge.SFN_Tex2d,id:9858,x:31923,y:32596,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4440,x:32054,y:33100,varname:node_4440,prsc:2|A-8878-RGB,B-2429-OUT;n:type:ShaderForge.SFN_Color,id:8878,x:31841,y:33038,ptovrint:False,ptlb:Bloom Color,ptin:_BloomColor,varname:_BloomColor,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:3378,x:32338,y:33086,varname:node_3378,prsc:2|A-9858-A,B-4440-OUT;n:type:ShaderForge.SFN_Multiply,id:601,x:32216,y:32707,varname:node_601,prsc:2|A-9858-R,B-9858-G;n:type:ShaderForge.SFN_Multiply,id:2190,x:32416,y:32724,varname:node_2190,prsc:2|A-601-OUT,B-5745-OUT;n:type:ShaderForge.SFN_Slider,id:5745,x:31788,y:32897,ptovrint:False,ptlb:Specular Strength,ptin:_SpecularStrength,varname:_SpecularStrength,prsc:2,min:0,cur:0,max:10;n:type:ShaderForge.SFN_Lerp,id:1885,x:32447,y:32918,varname:node_1885,prsc:2|A-9858-RGB,B-3378-OUT,T-3482-OUT;n:type:ShaderForge.SFN_Vector1,id:3482,x:32470,y:33372,varname:node_3482,prsc:2,v1:0.5;n:type:ShaderForge.SFN_ValueProperty,id:2429,x:32054,y:33337,ptovrint:False,ptlb:Bloom Strength,ptin:_BloomStrength,varname:node_2429,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Color,id:3059,x:32470,y:33142,ptovrint:False,ptlb:diffuse color,ptin:_diffusecolor,varname:node_3059,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:9858-5745-8878-2429-3059;pass:END;sub:END;*/

Shader "Kingsoft/Scene/Masked_Bloom" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
		_Alpha("Alpha", 2D) = "black" {}
        _SpecularStrength ("Specular Strength", Range(0, 10)) = 0
        _BloomColor ("Bloom Color", Color) = (0.5,0.5,0.5,1)
        _BloomStrength ("Bloom Strength", Float ) = 0
        _diffusecolor ("diffuse color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
			#pragma multi_compile_fog
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 3x3 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither3x3( float value, float2 sceneUVs ) {
                float3x3 mtx = float3x3(
                    float3( 3,  7,  4 )/10.0,
                    float3( 6,  1,  9 )/10.0,
                    float3( 2,  8,  5 )/10.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,3);
                int ySmp = fmod(px.y,3);
                float3 xVec = 1-saturate(abs(float3(0,1,2) - xSmp));
                float3 yVec = 1-saturate(abs(float3(0,1,2) - ySmp));
                float3 pxMult = float3( dot(mtx[0],yVec), dot(mtx[1],yVec), dot(mtx[2],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform float4 _LightColor0;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
			uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            uniform float4 _BloomColor;
            uniform float _SpecularStrength;
            uniform float _BloomStrength;
            uniform float4 _diffusecolor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                LIGHTING_COORDS(4,5)
				UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				UNITY_TRANSFER_FOG(o, o.pos);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
				float4 _Alpha_var = tex2D(_Alpha, TRANSFORM_TEX(i.uv0, _Alpha));
                float node_2190 = ((_Diffuse_var.r*_Diffuse_var.g)*_SpecularStrength);
                float3 specularColor = float3(node_2190,node_2190,node_2190);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
//                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                indirectDiffuse += _diffusecolor.rgb; // Diffuse Ambient Light
                float3 diffuse = (directDiffuse + indirectDiffuse) * _Diffuse_var.rgb;
////// Emissive:
                float3 emissive = lerp(_Diffuse_var.rgb,(_Alpha_var.r*(_BloomColor.rgb*_BloomStrength)),0.5);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
				
				fixed4 col = fixed4(finalColor, 1);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
