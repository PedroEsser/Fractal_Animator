Shader "Fractal/Mandelbrot"
{
    Properties
    {
        _Texture("Texture", 2D) = "white"
        _CarpetTransformation("CarpetTransformation", vector) = (0, 0, 1, 1)
        _Textures("Textures", 2DArray) = "" {}
        _BackgroundColor("BackgroundColor", vector) = (1, 1, 1, 1)
        _InsideColor("InsideColor", vector) = (1, 1, 1, 1)
        _Window("Window", vector) = (0, 0, 4, 4)
        _Length("Length", Float) = 1
        _TexturesCount("_TextureCount", Int) = 0
        _LightAngle("LightAngle", Float) = 0
        _LightHeight("LightHeight", Float) = 1
        _LightTextureAngle("LightTextureAngle", Float) = 0
        _EscapeRadius("EscapeRadius", Float) = 16
        _Angle("Angle", Float) = 0
        _ZStart("ZStart", vector) = (0, 0, 0, 0)
        _Power("Power", vector) = (2, 0, 0, 0)
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
                //#pragma target 3.5

                #include "UnityCG.cginc"
                #include "Assets/Utils/ShaderCode/Utils.cginc"
                #include "Assets/Utils/ShaderCode/Complex.cginc"

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
                float _Angle;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = (v.uv - .5) * _Window.zw;
                    o.uv = complex_mul(polar_to_rect(_Angle), o.uv.xy);
                    o.uv += _Window.xy;
                    return o;
                }

                static const float PI = 3.141592653589793238462;
                static const float TAU = 6.28318530717958647692;
                float _MaxIter;
                float _EscapeRadius;
                float _LightAngle;
                float _LightHeight;
                float _LightTextureAngle;
                float4 _ZStart;
                float4 _Power;

                float4 _CarpetTransformation;
                UNITY_DECLARE_TEX2DARRAY(_Textures);
                float _TextureIndices[500];
                float4 _TextureTransformations[1000];
                float4 _TextureColors[500];
                float _Length;
                int _TextureCount;

                float4 _BackgroundColor;
                float4 _InsideColor;

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
                    for (int i = 0; i < _TextureCount; i++) {
                        texPos = float2(myFrac(_TextureTransformations[i * 2].x), _TextureTransformations[i * 2].y);
                        diff = pos - texPos;
                        if (texPos.x + _TextureTransformations[i * 2].z > 1 && (pos.x + 1) < texPos.x + _TextureTransformations[i * 2].z)
                            diff.x += 1;
                        else if (texPos.x - _TextureTransformations[i * 2].z < 0 && (pos.x - 1) > texPos.x - _TextureTransformations[i * 2].z)
                            diff.x -= 1;

                        diff.y = myMod(diff.y + _Length / 2, _Length) - _Length / 2;
                        if (abs(diff.x) < _TextureTransformations[i * 2].z && abs(diff.y) < _TextureTransformations[i * 2].w) {
                            texPos = (diff / _TextureTransformations[i * 2].zw) + .5f;
                            float4 col = UNITY_SAMPLE_TEX2DARRAY_LOD(_Textures, float3(texPos.xy * _TextureTransformations[i * 2 + 1].zw + _TextureTransformations[i * 2 + 1].xy, _TextureIndices[i]), 0);
                            col *= _TextureColors[i];
                            if (col.a == 1)
                                return col;
                            col.a = (1 - color.a) * col.a;
                            color.rgb += col.rgb * col.a;
                            color.a += col.a;
                            //return (col * col.w + _BackgroundColor * (1 - col.w));
                        }
                    }
                    return (color * color.w + _BackgroundColor * (1 - color.w));

                }


                fixed4 frag(v2f i) : SV_Target
                {
                    float2 c = i.uv;
                    float2 z = float2(0.000000001, 0) + _ZStart.xy;
                    float2 aux;
                    float4 col = float4(0, 0, 0, 0);
                    float2 dC = float2(0, 0);
                    uint iteration;
                    float power;

                    [loop]
                    for (iteration = 0; iteration < _MaxIter && length(z) < _EscapeRadius; iteration++) {
                        aux = complex_pow(z, float2(_Power.x - 1, _Power.y));
                        dC = complex_mul(float2(_Power.x, _Power.y), complex_mul(dC, aux));
                        dC.x += 1;
                        z = complex_mul(aux, z) + c;

                        /*aux = complex_pow(z, float2(_Power.x + 1, _Power.y));
                        dC = complex_mul(float2(_Power.x + 2, _Power.y), complex_mul(dC, aux));
                        dC.x += 1;
                        z = complex_mul(aux, z) + c;
                        iteration++;*/
                    }
                    if (iteration > _MaxIter-1) return _InsideColor;
                    float smoothIteration = iteration + 6 - log(log(length(z))) / (log(2) /* 3/2*/);

                    if (smoothIteration > _MaxIter) return _InsideColor;

                    dC = complex_div(z, dC);
                    dC /= length(dC);

                    float2 light = polar_to_rect(2 * PI * _LightTextureAngle);
                    float textureX = acos(dot(dC, light)) / PI;

                    

                    light = polar_to_rect(2 * PI * _LightAngle);
                    float brightness = dot(dC, light) + _LightHeight;
                    brightness /= (1 + _LightHeight);
                    brightness = max(0, brightness);
                    //brightness = 1;

                    light = _CarpetTransformation.zw;
                    light.y /= -4;
                    light = _CarpetTransformation.xy + light * float2(textureX, smoothIteration);
                    col = getCarpetColorAt(light);
                    col.xyz *= brightness;
                    return col;
                    //col = tex2D(_Texture, light);
                    //return col;
                    //return (col * col.w + (1-col.w) * _Color) * brightness;
                }
                ENDCG
            }
        }
}
