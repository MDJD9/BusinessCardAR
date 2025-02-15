Shader "Custom/ChromaKeyURP_Mobile"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _MaskCol ("Chroma Key Color", Color) = (1, 0, 0, 1)
        _Sensitivity ("Threshold Sensitivity", Range(0, 1)) = 0.5
        _Smooth ("Smoothing", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalRenderPipeline"}
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MaskCol;
            half _Sensitivity;
            half _Smooth;

            // Tiling & Offset
            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                
                // Apply Tiling & Offset manually
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                // Convert color to YCrCb
                half maskY = 0.2989 * _MaskCol.r + 0.5866 * _MaskCol.g + 0.1145 * _MaskCol.b;
                half maskCr = 0.7132 * (_MaskCol.r - maskY);
                half maskCb = 0.5647 * (_MaskCol.b - maskY);

                half Y = 0.2989 * col.r + 0.5866 * col.g + 0.1145 * col.b;
                half Cr = 0.7132 * (col.r - Y);
                half Cb = 0.5647 * (col.b - Y);

                half blendValue = smoothstep(_Sensitivity, _Sensitivity + _Smooth, distance(half2(Cr, Cb), half2(maskCr, maskCb)));

                col.a = blendValue;
                return col;
            }
            ENDHLSL
        }
    }
}
