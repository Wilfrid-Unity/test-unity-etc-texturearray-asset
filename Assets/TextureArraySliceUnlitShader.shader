Shader "Unlit/TextureArraySliceUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2DArray) = "white" {}
        _Index("Index", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            Texture2DArray _MainTex;
            SamplerState sampler_MainTex;
            float4 _MainTex_ST;
            float _Index;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 col = _MainTex.Sample(sampler_MainTex, float3(i.uv, _Index));
                return fixed4(col, 1.0);
            }
            ENDCG
        }
    }
}
