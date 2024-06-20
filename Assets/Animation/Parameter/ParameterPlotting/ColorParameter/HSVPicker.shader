Shader "Color/HSVPicker"
{
    Properties
    {
        _HSVColor("HSVColor", vector) = (1, 1, 1, 1)
        _BackgroundColor("BackgroundColor", vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Assets/Utils/ShaderCode/Utils.cginc"

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            static const float TAU = 6.28318530717958647692;
            float4 _HSVColor;
            float4 _BackgroundColor;
            float _Size;

            fixed4 frag(v2f i) : SV_Target
            { 
                float2 pixel = 2 * (i.uv - .5);
                float m = min(_ScreenParams.y, _ScreenParams.x);
                pixel *= _ScreenParams.xy / m;
                if (length(pixel) > .9 && length(pixel) < 1) {
                    m = atan2(pixel.y, pixel.x) / TAU;
                    m = frac(m + 1);
                    return hsv_to_rgb(float3(m, _HSVColor.gb));
                }
                if (abs(pixel.x) < .6 && abs(pixel.y) < .6) {
                    pixel /= .6 * 2;
                    pixel += .5;
                    return hsv_to_rgb(float3(_HSVColor.r, pixel.x, pixel.y));
                }

                return _BackgroundColor;
            }
            ENDCG
        }
    }
}
