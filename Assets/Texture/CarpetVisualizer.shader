Shader "Fractal/CarpetVisualizer"
{
    Properties
    {
        _Textures ("Textures", 2DArray) = "" {}
        _BackgroundColor ("BackgroundColor", vector) = (1, 1, 1, 1)
        _Window("Window", vector) = (0, 0, 4, 4)
        _Length("Length", Float) = 10
        _TexturesCount("_TexturesCount", Int) = 0
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

            float4 _Window;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = (v.uv - .5) * _Window.zw;
                o.uv += _Window.xy;
                return o;
            }

            sampler2D _MainTex;
            UNITY_DECLARE_TEX2DARRAY(_Textures);
            float _TexturesIndices[500];
            float4 _TexturesTransformations[1000];
            float _Length;
            int _TexturesCount;

            float4 _BackgroundColor;


            float4 getCarpetColorAt(float2 pos) {
                pos.x = frac(pos.x);
                pos.y = pos.y % _Length;

                float2 diff;
                for (int i = 0; i < _TexturesCount; i++) {
                    diff = pos - _TexturesTransformations[i * 2].xy;
                    if (abs(diff.x) < _TexturesTransformations[i * 2].z && abs(diff.y) < _TexturesTransformations[i * 2].w) {
                        pos = (diff / _TexturesTransformations[i * 2].zw) + .5f;
                        return UNITY_SAMPLE_TEX2DARRAY(_Textures, float3(pos.xy * _TexturesTransformations[i * 2 + 1].zw + _TexturesTransformations[i * 2 + 1].xy, _TexturesIndices[i]));
                    }
                }
                return _BackgroundColor;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pos = i.uv;
                if (pos.x < 0 || pos.x > 1) {
                    pos *= 10;
                    float sum = floor(pos.x) + floor(pos.y);
                    if (sum % 2 == 0)
                        return .5;
                    return .8;
                }
                return getCarpetColorAt(pos);
            }
            ENDCG
        }
    }
}
