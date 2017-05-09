// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.03 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.03;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.2034494,fgcg:0.350669,fgcb:0.5220588,fgca:1,fgde:0,fgrn:14.87,fgrf:38.5,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:5815,x:32719,y:32712,varname:node_5815,prsc:2|diff-9691-RGB,emission-2877-OUT,amdfl-4395-RGB;n:type:ShaderForge.SFN_Tex2d,id:9691,x:31960,y:32567,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:8216,x:32103,y:33152,ptovrint:False,ptlb:Pattern,ptin:_Pattern,varname:_Pattern,prsc:2,tex:97b01aa420dcb084899c2d9a2bed23b3,ntxv:0,isnm:False|UVIN-9351-OUT;n:type:ShaderForge.SFN_Multiply,id:8180,x:32131,y:32881,varname:node_8180,prsc:2|A-9691-A,B-9263-RGB;n:type:ShaderForge.SFN_Panner,id:8833,x:31544,y:33136,varname:node_8833,prsc:2,spu:1,spv:0;n:type:ShaderForge.SFN_Color,id:9263,x:31916,y:32881,ptovrint:False,ptlb:Light Color,ptin:_LightColor,varname:_LightColor,prsc:2,glob:False,c1:0.7019608,c2:0.5960785,c3:0.2745098,c4:1;n:type:ShaderForge.SFN_Multiply,id:2877,x:32349,y:32881,varname:node_2877,prsc:2|A-8180-OUT,B-8216-RGB;n:type:ShaderForge.SFN_Multiply,id:9351,x:31824,y:33190,varname:node_9351,prsc:2|A-8833-UVOUT,B-9812-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9812,x:31667,y:33454,ptovrint:False,ptlb:Light Speed,ptin:_LightSpeed,varname:_LightSpeed,prsc:2,glob:False,v1:0.025;n:type:ShaderForge.SFN_Color,id:4395,x:32661,y:33395,ptovrint:False,ptlb:color,ptin:_color,varname:node_4395,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:9691-8216-9263-9812-4395;pass:END;sub:END;*/

Shader "Kingsoft/Scene/sfx_windowsOnOff" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
		_Alpha("Alpha", 2D) = "black" {}
        _Pattern ("Pattern", 2D) = "white" {}
        _LightColor ("Light Color", Color) = (0.7019608,0.5960785,0.2745098,1)
        _LightSpeed ("Light Speed", Float ) = 0.025
        _color ("color", Color) = (0.5,0.5,0.5,1)
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

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
			#pragma multi_compile_fog
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
            uniform float4 _TimeEditor;
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
			uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            uniform sampler2D _Pattern; uniform float4 _Pattern_ST;
            uniform float4 _LightColor;
            uniform float _LightSpeed;
            uniform float4 _color;
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
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
				UNITY_TRANSFER_FOG(o, o.pos);
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
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
//                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                indirectDiffuse += _color.rgb; // Diffuse Ambient Light
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
				float4 _Alpha_var = tex2D(_Alpha, TRANSFORM_TEX(i.uv0, _Alpha));
                float3 diffuse = (directDiffuse + indirectDiffuse) * _Diffuse_var.rgb;
////// Emissive:
                float4 node_7507 = _Time + _TimeEditor;
                float2 node_9351 = ((i.uv0+node_7507.g*float2(1,0))*_LightSpeed);
                float4 _Pattern_var = tex2D(_Pattern,TRANSFORM_TEX(node_9351, _Pattern));
                float3 emissive = ((_Alpha_var.r*_LightColor.rgb)*_Pattern_var.rgb);
/// Final Color:
                float3 finalColor = diffuse + emissive;
				fixed4 col = fixed4(finalColor,1);
				UNITY_APPLY_FOG(i.fogCoord, col); // fog towards black due to our blend mode
				return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
