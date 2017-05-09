// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.03 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.03;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:False,lmpd:False,lprd:True,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3856,x:33460,y:32933,varname:node_3856,prsc:2|diff-3954-RGB,spec-2698-OUT,gloss-8560-OUT,emission-3522-OUT,amdfl-3421-RGB;n:type:ShaderForge.SFN_Tex2d,id:3954,x:33125,y:32721,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:_Diffuse,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_NormalVector,id:1523,x:30476,y:33378,prsc:2,pt:False;n:type:ShaderForge.SFN_Vector4Property,id:7847,x:30461,y:33641,ptovrint:True,ptlb:BackLight Dir(XYZ),ptin:_BackLightDirXYZGlossW,varname:_BackLightDirXYZGlossW,prsc:2,glob:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Power,id:8554,x:31506,y:33515,varname:node_8554,prsc:2|VAL-7101-OUT,EXP-1553-OUT;n:type:ShaderForge.SFN_Vector1,id:1553,x:31402,y:33758,varname:node_1553,prsc:2,v1:5;n:type:ShaderForge.SFN_Multiply,id:2930,x:31731,y:33515,varname:node_2930,prsc:2|A-7906-RGB,B-8554-OUT;n:type:ShaderForge.SFN_Color,id:3421,x:33342,y:33479,ptovrint:False,ptlb:AmbientColor,ptin:_AmbientColor,varname:_AmbientColor,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:7906,x:31477,y:33193,ptovrint:False,ptlb:RimLightColor,ptin:_RimLightColor,varname:_RimLightColor,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Fresnel,id:3868,x:30877,y:33812,varname:node_3868,prsc:2;n:type:ShaderForge.SFN_Dot,id:7101,x:31118,y:33782,varname:node_7101,prsc:2,dt:1|A-5631-OUT,B-3868-OUT;n:type:ShaderForge.SFN_Blend,id:5631,x:30778,y:33574,varname:node_5631,prsc:2,blmd:10,clmp:True|SRC-1523-OUT,DST-7847-XYZ;n:type:ShaderForge.SFN_Multiply,id:7629,x:33110,y:33302,cmnt:Emission,varname:node_7629,prsc:2|A-2949-OUT,B-8087-OUT;n:type:ShaderForge.SFN_Color,id:6960,x:32643,y:33478,ptovrint:False,ptlb:Emission(B),ptin:_EmissionB,varname:_EmissionB,prsc:2,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Sin,id:290,x:32605,y:33921,varname:node_290,prsc:2|IN-3794-T;n:type:ShaderForge.SFN_Time,id:3794,x:32425,y:33905,varname:node_3794,prsc:2;n:type:ShaderForge.SFN_Abs,id:2638,x:32760,y:33921,varname:node_2638,prsc:2|IN-290-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:8087,x:32942,y:33839,ptovrint:False,ptlb:emission_blink_switch,ptin:_emission_blink_switch,varname:node_5926,prsc:2,on:False|A-6514-OUT,B-2638-OUT;n:type:ShaderForge.SFN_Vector1,id:6514,x:32732,y:33821,varname:node_6514,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:2949,x:32878,y:33648,varname:node_2949,prsc:2|A-3435-OUT,B-6960-RGB,C-9744-B;n:type:ShaderForge.SFN_Slider,id:3435,x:32617,y:33374,ptovrint:False,ptlb:glow power,ptin:_glowpower,varname:node_433,prsc:2,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Tex2d,id:9744,x:32289,y:33211,ptovrint:False,ptlb:lightctrl_mask,ptin:_lightctrl_mask,cmnt:R-reflection G-Spec B-Emssive,varname:node_9744,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2698,x:32913,y:32916,varname:node_2698,prsc:2|A-9744-G,B-7716-RGB,C-2632-OUT;n:type:ShaderForge.SFN_Color,id:7716,x:32696,y:33087,ptovrint:False,ptlb:spec color,ptin:_speccolor,varname:node_7716,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:2632,x:32759,y:33264,ptovrint:False,ptlb:spec scale,ptin:_specscale,varname:node_2632,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Slider,id:8560,x:33086,y:33032,ptovrint:False,ptlb:gloss,ptin:_gloss,varname:node_8560,prsc:2,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Add,id:3522,x:33270,y:33302,varname:node_3522,prsc:2|A-7629-OUT,B-6581-RGB;n:type:ShaderForge.SFN_Color,id:6581,x:33110,y:33466,ptovrint:False,ptlb:EmissiveTint,ptin:_EmissiveTint,varname:node_6581,prsc:2,glob:False,c1:0,c2:0,c3:0,c4:1;proporder:3954-9744-7716-2632-7847-3421-7906-6960-8087-3435-8560-6581;pass:END;sub:END;*/

Shader "Kingsoft/Character/CharacterMedium" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _lightctrl_mask ("lightctrl_mask", 2D) = "white" {}
        _speccolor ("spec color", Color) = (0.5,0.5,0.5,1)
        _specscale ("spec scale", Float ) = 0
        _BackLightDirXYZGlossW ("BackLight Dir(XYZ)", Vector) = (0,0,0,0)
        _AmbientColor ("AmbientColor", Color) = (0.5,0.5,0.5,1)
        _RimLightColor ("RimLightColor", Color) = (0.5,0.5,0.5,1)
        _EmissionB ("Emission(B)", Color) = (0,0,0,1)
        [MaterialToggle] _emission_blink_switch ("emission_blink_switch", Float ) = 1
        _glowpower ("glow power", Range(0, 5)) = 0
        _gloss ("gloss", Range(0, 1)) = 0
        _EmissiveTint ("EmissiveTint", Color) = (0,0,0,1)
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
            #define SHOULD_SAMPLE_SH_PROBE ( defined (LIGHTMAP_OFF) )
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
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
            uniform float4 _AmbientColor;
            uniform float4 _EmissionB;
            uniform fixed _emission_blink_switch;
            uniform float _glowpower;
            uniform sampler2D _lightctrl_mask; uniform float4 _lightctrl_mask_ST;
            uniform float4 _speccolor;
            uniform float _specscale;
            uniform float _gloss;
            uniform float4 _EmissiveTint;
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
                float3 shLight : TEXCOORD6;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                #pragma multi_compile LightProbe_Off LightProbe_On
				#ifdef LightProbe_On
                    o.shLight = ShadeSH9(float4(mul(unity_ObjectToWorld, float4(v.normal,0)).xyz * 1.0,1)) * 0.5;
                #endif
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = float4( o.pos.xy / o.pos.w, 0, 0 );
                o.screenPos.y *= _ProjectionParams.x;
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
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                //when no light in scene, below instructions are redundant, and might cause imcompatibility with ios9
                //float attenuation = LIGHT_ATTENUATION(i);
                //float3 attenColor = attenuation * _LightColor0.xyz; 
				float3 attenColor = 0;
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _lightctrl_mask_var = tex2D(_lightctrl_mask,TRANSFORM_TEX(i.uv0, _lightctrl_mask)); // R-reflection G-Spec B-Emssive
                float3 specularColor = (_lightctrl_mask_var.g*_speccolor.rgb*_specscale);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                indirectDiffuse += _AmbientColor.rgb; // Diffuse Ambient Light
                #pragma multi_compile LightProbe_Off LightProbe_On
				#ifdef LightProbe_On
                    indirectDiffuse *= 0.7+0.3*i.shLight;; // Per-Vertex Light Probes / Spherical harmonics
                #endif
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 diffuse = (directDiffuse + indirectDiffuse) * _Diffuse_var.rgb;
////// Emissive:
                float4 node_3794 = _Time + _TimeEditor;
                float3 emissive = (((_glowpower*_EmissionB.rgb*_lightctrl_mask_var.b)*lerp( 1.0, abs(sin(node_3794.g)), _emission_blink_switch ))+_EmissiveTint.rgb);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}