Shader "Custom Effects/OutlineEdgeShader"
{
    Properties
    {
        _OutlineWeight ("Outline Weight", Range(.002, 0.03)) = .005
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        // No culling or depth
        Cull Front
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
        ZTest LEqual
        LOD 250

        Pass
        {
            Name "TOON OUTLINE"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(VariablesPerMaterials) //Buffer Macros for Direct3D 11
            float _OutlineWeight;
            float4 _OutlineColor;
            CBUFFER_END

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID // necessary only if you want to access instanced properties in fragment Shader.
            };

            struct VertOutput
            {
                float4 pos : SV_POSITION;
                half fogCoord : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            VertOutput vert (appdata input)
            {
                VertOutput output;
                UNITY_SETUP_INSTANCE_ID(input); //This is only necessary fragment shader access instanced mats
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                input.vertex.xyz += input.normal.xyz * _OutlineWeight;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.vertex.xyz); //offers vertex data to the vertex programs
                output.pos = vertexInput.positionCS;

                output.fogCoord = ComputeFogFactor(output.pos.z);
                output.color = _OutlineColor;
                return output;
            }

            half4 frag (VertOutput i) : SV_Target
            {
                i.color.rbg = MixFog(i.color.rgb, i.fogCoord);
                return i.color;
            }

            ENDHLSL
        }
    }
}
