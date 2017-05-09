// Shader created with Shader Forge v1.03 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.03;sub:START;pass:START;ps:flbk:,lico:0,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:False,hqlp:False,tesm:0,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,dith:2,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:1,fgcg:1,fgcb:0.8039216,fgca:1,fgde:0,fgrn:20,fgrf:42,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:964,x:32671,y:32645,varname:node_964,prsc:2|emission-6699-OUT,alpha-4655-R;n:type:ShaderForge.SFN_Tex2d,id:7502,x:31346,y:32488,ptovrint:False,ptlb:tex1,ptin:_tex1,varname:node_7502,prsc:2,tex:8df5ec9580e72f0478bbc9a01caf4872,ntxv:0,isnm:False|UVIN-1615-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:9158,x:30964,y:32512,ptovrint:False,ptlb:normal,ptin:_normal,varname:node_9158,prsc:2,tex:dd01653c5e7a5ad48b7ec934b4bc4626,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:8855,x:31417,y:32946,ptovrint:False,ptlb:alph1,ptin:_alph1,varname:node_8855,prsc:2,tex:28d89acf513f7d443ab4fa6eec4c1fd2,ntxv:0,isnm:False|UVIN-8364-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8364,x:31181,y:32986,varname:node_8364,prsc:2,uv:0;n:type:ShaderForge.SFN_Tex2d,id:3923,x:31681,y:32812,ptovrint:False,ptlb:tex2,ptin:_tex2,varname:node_3923,prsc:2,tex:b437ee6053bbd9948be26924942158d4,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:6246,x:32009,y:33009,varname:node_6246,prsc:2|A-3923-RGB,B-8855-RGB;n:type:ShaderForge.SFN_Subtract,id:9766,x:31648,y:32660,varname:node_9766,prsc:2|A-1955-OUT,B-8855-R;n:type:ShaderForge.SFN_Vector1,id:1955,x:31444,y:32694,varname:node_1955,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:3389,x:31889,y:32592,varname:node_3389,prsc:2|A-7502-RGB,B-9766-OUT;n:type:ShaderForge.SFN_Add,id:1891,x:32237,y:32750,varname:node_1891,prsc:2|A-3389-OUT,B-6246-OUT;n:type:ShaderForge.SFN_Tex2d,id:4655,x:32220,y:33007,ptovrint:False,ptlb:alph2,ptin:_alph2,varname:node_4655,prsc:2,tex:30506984f41112441aebd1a0e93edaaf,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Panner,id:1615,x:30964,y:32300,varname:node_1615,prsc:2,spu:0,spv:0.1;n:type:ShaderForge.SFN_Tex2d,id:9458,x:31429,y:32034,ptovrint:False,ptlb:hailang,ptin:_hailang,varname:node_9458,prsc:2,tex:26f90ae27eaec1947a9a69d9cd03fb18,ntxv:0,isnm:False|UVIN-923-UVOUT;n:type:ShaderForge.SFN_Panner,id:923,x:31150,y:32039,varname:node_923,prsc:2,spu:0,spv:0.13|DIST-1531-OUT;n:type:ShaderForge.SFN_Sin,id:1531,x:30908,y:32076,varname:node_1531,prsc:2|IN-7789-T;n:type:ShaderForge.SFN_Add,id:6699,x:32411,y:32517,varname:node_6699,prsc:2|A-7310-OUT,B-1891-OUT;n:type:ShaderForge.SFN_Time,id:7789,x:30459,y:32051,varname:node_7789,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9046,x:31877,y:31883,varname:node_9046,prsc:2|A-7229-OUT,B-9458-RGB;n:type:ShaderForge.SFN_Vector1,id:3572,x:31277,y:31728,varname:node_3572,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Sin,id:7969,x:31062,y:31757,varname:node_7969,prsc:2|IN-7911-T;n:type:ShaderForge.SFN_Time,id:7911,x:30825,y:31740,varname:node_7911,prsc:2;n:type:ShaderForge.SFN_ConstantClamp,id:254,x:31250,y:31841,varname:node_254,prsc:2,min:0.2,max:1|IN-7969-OUT;n:type:ShaderForge.SFN_Add,id:9539,x:31455,y:31852,varname:node_9539,prsc:2|A-3572-OUT,B-254-OUT;n:type:ShaderForge.SFN_Subtract,id:5671,x:31887,y:32282,varname:node_5671,prsc:2|A-1678-OUT,B-9766-OUT;n:type:ShaderForge.SFN_Vector1,id:1678,x:31677,y:32338,varname:node_1678,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:7310,x:32248,y:32281,varname:node_7310,prsc:2|A-687-OUT,B-5671-OUT;n:type:ShaderForge.SFN_Vector1,id:8351,x:31523,y:31687,varname:node_8351,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:7229,x:31700,y:31808,varname:node_7229,prsc:2|A-8351-OUT,B-9539-OUT;n:type:ShaderForge.SFN_Panner,id:2096,x:31172,y:32215,varname:node_2096,prsc:2,spu:0,spv:0.05;n:type:ShaderForge.SFN_Add,id:687,x:32072,y:32123,varname:node_687,prsc:2|A-9046-OUT,B-4048-OUT;n:type:ShaderForge.SFN_Tex2d,id:565,x:31413,y:32232,ptovrint:False,ptlb:hailang2,ptin:_hailang2,varname:node_565,prsc:2,tex:cf037d39cc1416c46b1bf33e8e72a680,ntxv:0,isnm:False|UVIN-2096-UVOUT;n:type:ShaderForge.SFN_Multiply,id:4048,x:31844,y:32110,varname:node_4048,prsc:2|A-1319-OUT,B-565-RGB;n:type:ShaderForge.SFN_Vector1,id:1319,x:31670,y:32034,varname:node_1319,prsc:2,v1:0.3;proporder:7502-9158-8855-3923-4655-9458-565;pass:END;sub:END;*/

Shader "Shader Forge/e_water_01" {
    Properties {
        _tex1 ("tex1", 2D) = "white" {}
        _normal ("normal", 2D) = "bump" {}
        _alph1 ("alph1", 2D) = "white" {}
        _tex2 ("tex2", 2D) = "white" {}
        _alph2 ("alph2", 2D) = "white" {}
        _hailang ("hailang", 2D) = "white" {}
        _hailang2 ("hailang2", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
			#pragma multi_compile_fog
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
            uniform float4 _TimeEditor;
            uniform sampler2D _tex1; uniform float4 _tex1_ST;
            uniform sampler2D _alph1; uniform float4 _alph1_ST;
            uniform sampler2D _tex2; uniform float4 _tex2_ST;
            uniform sampler2D _alph2; uniform float4 _alph2_ST;
            uniform sampler2D _hailang; uniform float4 _hailang_ST;
            uniform sampler2D _hailang2; uniform float4 _hailang2_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
				UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = float4( o.pos.xy / o.pos.w, 0, 0 );
                o.screenPos.y *= _ProjectionParams.x;
				UNITY_TRANSFER_FOG(o, o.pos);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 node_7911 = _Time + _TimeEditor;
                float4 node_7789 = _Time + _TimeEditor;
                float2 node_923 = (i.uv0+sin(node_7789.g)*float2(0,0.13));
                float4 _hailang_var = tex2D(_hailang,TRANSFORM_TEX(node_923, _hailang));
                float4 node_9944 = _Time + _TimeEditor;
                float2 node_2096 = (i.uv0+node_9944.g*float2(0,0.05));
                float4 _hailang2_var = tex2D(_hailang2,TRANSFORM_TEX(node_2096, _hailang2));
                float4 _alph1_var = tex2D(_alph1,TRANSFORM_TEX(i.uv0, _alph1));
                float node_9766 = (1.0-_alph1_var.r);
                float2 node_1615 = (i.uv0+node_9944.g*float2(0,0.1));
                float4 _tex1_var = tex2D(_tex1,TRANSFORM_TEX(node_1615, _tex1));
                float4 _tex2_var = tex2D(_tex2,TRANSFORM_TEX(i.uv0, _tex2));
                float3 emissive = (((((0.5*(0.5+clamp(sin(node_7911.g),0.2,1)))*_hailang_var.rgb)+(0.3*_hailang2_var.rgb))*(1.0-node_9766))+((_tex1_var.rgb*node_9766)+(_tex2_var.rgb*_alph1_var.rgb)));
                float3 finalColor = emissive;
                float4 _alph2_var = tex2D(_alph2,TRANSFORM_TEX(i.uv0, _alph2));
				fixed4 col = fixed4(finalColor,_alph2_var.r);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
