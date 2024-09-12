Shader "Fractal/CarpetVisualizer"
{
    Properties
    {
        _Textures ("Textures", 2DArray) = "" {}
        _BackgroundColor ("BackgroundColor", vector) = (1, 1, 1, 1)
        _Window("Window", vector) = (0, 0, 4, 4)
        _Length("Length", Float) = 10
        _TextureCount("_TextureCount", Int) = 0
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

            UNITY_DECLARE_TEX2DARRAY(_Textures);
            float _TextureIndices[500];
            float4 _TextureTransformations[1000];
            float4 _TextureColors[500];
            float _Length;
            int _TextureCount;

            float4 _BackgroundColor;

            float myFrac(float v) {
                return frac(frac(v) + 1);
            }

            float myMod(float v, float m) {
                return (v % m + m) % m;
            }

            float4 getCarpetColorAt(float2 pos) {
                pos.x = myFrac(pos.x);

                float2 diff;
                float2 texPos;
                float4 color = 0;
                float aux;
                for (int i = 0; i < _TextureCount; i++) {
                    texPos = float2(_TextureTransformations[i * 2].x, _TextureTransformations[i * 2].y);
                    diff = pos - texPos;

                    aux = _TextureTransformations[i * 2 + 1].x;
                    if (aux != 0)
                        diff.x = myMod(diff.x + aux / 2, aux) - aux / 2;

                    aux = _TextureTransformations[i * 2 + 1].y;
                    if (aux != 0)
                        diff.y = myMod(diff.y + aux / 2, aux) - aux / 2;


                    if (abs(diff.x) < _TextureTransformations[i * 2].z && abs(diff.y) < _TextureTransformations[i * 2].w) {
                        texPos = (diff / _TextureTransformations[i * 2].zw) + .5f;
                        float4 col = UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, float3(texPos.xy * _TextureTransformations[i * 2 + 1].zw + .25, _TextureIndices[i]), 0);
                        col *= _TextureColors[i];
                        if (col.a == 1)
                            return col;
                        col.a = (1 - color.a) * col.a;
                        color.rgb += col.rgb * col.a;
                        color.a += col.a;
                    }
                }
                return color;
            }

            float transparentColorAt(float2 pos) {
                pos *= 10;
                float sum = floor(pos.x) + floor(pos.y);
                if (sum % 2 == 0)
                    return .5;
                return .8;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pos = i.uv;
                float4 carpetColor = getCarpetColorAt(pos);

                if (pos.x < 0 || pos.x > 1)
                    return 0;
                if (carpetColor.a == 0) 
                    return transparentColorAt(pos);

                return carpetColor;
            }
            ENDCG
        }
    }
}
