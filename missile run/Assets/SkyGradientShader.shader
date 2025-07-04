Shader "Custom/SkyGradient"
{
    Properties
    {
        _TopColor ("Top Color", Color) = (0.5,0.8,1,1)
        _BottomColor ("Bottom Color", Color) = (0,0.4,0.8,1)
    }
    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Opaque" }
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _TopColor;
            fixed4 _BottomColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; // Use mesh UVs
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Blend based on UV.y (0 at bottom, 1 at top)
                return lerp(_BottomColor, _TopColor, i.uv.y);
            }
            ENDCG
        }
    }
}
