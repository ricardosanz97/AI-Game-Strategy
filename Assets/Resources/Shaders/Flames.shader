// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32570,y:32760,varname:node_2865,prsc:2|emission-5857-OUT,alpha-7137-OUT;n:type:ShaderForge.SFN_Tex2d,id:7140,x:31648,y:32819,ptovrint:False,ptlb:MainTexture,ptin:_MainTexture,varname:node_7140,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:13e7c41a3cc3bc5468376aaf5f173040,ntxv:0,isnm:False|UVIN-6177-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:952,x:30823,y:32915,varname:node_952,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:6177,x:31266,y:32951,varname:node_6177,prsc:2,spu:1,spv:0|UVIN-952-UVOUT,DIST-4448-OUT;n:type:ShaderForge.SFN_Time,id:3338,x:30681,y:33312,varname:node_3338,prsc:2;n:type:ShaderForge.SFN_Slider,id:9826,x:30524,y:33149,ptovrint:False,ptlb:PannerTimeSpeed,ptin:_PannerTimeSpeed,varname:node_9826,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.25,max:1;n:type:ShaderForge.SFN_Multiply,id:4448,x:30958,y:33108,varname:node_4448,prsc:2|A-9826-OUT,B-3338-T;n:type:ShaderForge.SFN_Multiply,id:7137,x:32186,y:32972,varname:node_7137,prsc:2|A-7140-A,B-9932-OUT;n:type:ShaderForge.SFN_Sin,id:4407,x:31517,y:33205,varname:node_4407,prsc:2|IN-6039-TDB;n:type:ShaderForge.SFN_Add,id:9227,x:31804,y:33256,varname:node_9227,prsc:2|A-4407-OUT,B-5895-OUT;n:type:ShaderForge.SFN_Time,id:6039,x:31293,y:33205,varname:node_6039,prsc:2;n:type:ShaderForge.SFN_Slider,id:5895,x:31360,y:33413,ptovrint:False,ptlb:AppendToSinGraphic,ptin:_AppendToSinGraphic,varname:node_5895,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_ConstantClamp,id:9932,x:31988,y:33118,varname:node_9932,prsc:2,min:0.25,max:0.5|IN-9227-OUT;n:type:ShaderForge.SFN_Color,id:6357,x:31648,y:32612,ptovrint:False,ptlb:FlamesColor,ptin:_FlamesColor,varname:node_6357,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.8435739,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:5857,x:32000,y:32709,varname:node_5857,prsc:2|A-6357-RGB,B-7140-RGB;proporder:7140-9826-5895-6357;pass:END;sub:END;*/

Shader "Shader Forge/Flames" {
    Properties {
        _MainTexture ("MainTexture", 2D) = "white" {}
        _PannerTimeSpeed ("PannerTimeSpeed", Range(0, 1)) = 0.25
        _AppendToSinGraphic ("AppendToSinGraphic", Range(0, 1)) = 0
        _FlamesColor ("FlamesColor", Color) = (1,0.8435739,0,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float _PannerTimeSpeed;
            uniform float _AppendToSinGraphic;
            uniform float4 _FlamesColor;
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
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
////// Lighting:
////// Emissive:
                float4 node_3338 = _Time;
                float2 node_6177 = (i.uv0+(_PannerTimeSpeed*node_3338.g)*float2(1,0));
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(node_6177, _MainTexture));
                float3 emissive = (_FlamesColor.rgb*_MainTexture_var.rgb);
                float3 finalColor = emissive;
                float4 node_6039 = _Time;
                fixed4 finalRGBA = fixed4(finalColor,(_MainTexture_var.a*clamp((sin(node_6039.b)+_AppendToSinGraphic),0.25,0.5)));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float _PannerTimeSpeed;
            uniform float4 _FlamesColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_3338 = _Time;
                float2 node_6177 = (i.uv0+(_PannerTimeSpeed*node_3338.g)*float2(1,0));
                float4 _MainTexture_var = tex2D(_MainTexture,TRANSFORM_TEX(node_6177, _MainTexture));
                o.Emission = (_FlamesColor.rgb*_MainTexture_var.rgb);
                
                float3 diffColor = float3(0,0,0);
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, 0, specColor, specularMonochrome );
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
