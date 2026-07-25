Shader "Custom/Wave Animation"
{
    Properties
    {
        _ColorA("Color A", Color) = (0, 0, 0, 1)
        _ColorB("Color B", Color) = (1, 1, 1, 1)
        _ColorStart("Color Start", Range(0, 1)) = 0
        _ColorEnd("Color End", Range(0, 1)) = 0
        _AnimationSpeed("Animation Speed", Range(-10, 10)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent"
               "RenderPipeline" = "UniversalPipeline"
               "Queue" = "Transparent"
        }

        Pass
        {
            Cull Off
            ZWrite Off
            
            Blend One OneMinusSrcAlpha
            
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            #define TAU (2*PI)

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                half4 _ColorA;
                half4 _ColorB;
                float _ColorStart;
                float _ColorEnd;
                float _AnimationSpeed;
            CBUFFER_END

            struct MeshAttributes
            {
                float4 positionOS : POSITION;
                float3 normals : NORMAL;
                float2 uv0 : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normal_uv : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };


            Varyings vert(MeshAttributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normal_uv = TransformObjectToWorldNormal(IN.normals);
                OUT.uv = IN.uv0;
                return OUT;
            }
            
            float InverseLerp(float a, float b, float value)
            {
                return (value - a) / (b - a);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float xOffset = sin(IN.uv.y * TAU) * 0.1;
                float wave = cos((IN.uv.x + xOffset) * TAU * 5 + _Time.y * _AnimationSpeed)* 0.5 + 0.5;
                wave *= 1-IN.uv.y;
                
                wave *= (abs(IN.normal_uv.y) < 0.999);
                
                return wave * _ColorA;
            }
            ENDHLSL
        }
    }
}
