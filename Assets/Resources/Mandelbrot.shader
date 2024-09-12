Shader "Fractal/Mandelbrot"
{
    Properties
    {
        _Texture("Texture", 2D) = "white"
        _CarpetTransformation("CarpetTransformation", vector) = (0, 0, 1, 1)
        _Textures("Textures", 2DArray) = "" {}
        _OutsideColor("OutsideColor", vector) = (1, 1, 1, 1)
        _InsideColor("InsideColor", vector) = (1, 1, 1, 1)
        _Window("Window", vector) = (0, 0, 4, 4)
        _TexturesCount("_TextureCount", Int) = 0
        _LightAngle("LightAngle", Float) = 0
        _LightHeight("LightHeight", Float) = 1
        _LightTextureAngle("LightTextureAngle", Float) = 0
        _EscapeRadius("EscapeRadius", Float) = 16
        _Angle("Angle", Float) = 0
        _ZStart("ZStart", vector) = (0, 0, 0, 0)
        _Power("Power", vector) = (2, 0, 0, 0)
        _JuliaFlag("JuliaFlag", Int) = 0
        _TransparentFlag("TransparentFlag", Int) = 0
    }
        SubShader
        {
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
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
                    o.uv = v.uv;
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
                int _TextureCount;

                float4 _OutsideColor;
                float4 _InsideColor;
                int _TransparentFlag;
                int _JuliaFlag;

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

                        //Tilling
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

                float4 transparentColorAt(float2 pos) {
                    pos *= 20;
                    float sum = floor(pos.x) + floor(pos.y);
                    if (sum % 2 == 0)
                        return .5;
                    return .8;
                }

                float4 blend(float4 a, float4 b) {
                    float4 col = (a.a * a + b.a * b) / (a.a + b.a);
                    col.a = max(a.a, b.a);
                    return col;
                }


                fixed4 frag(v2f i) : SV_Target
                {
                    float2 pos = i.uv;
                    float2 c = (pos - .5) * _Window.zw;
                    c = complex_mul(polar_to_rect(_Angle), c);
                    c += _Window.xy;

                    //c = complex_pow(c, float2(_Power.y, 0));

                    float2 z = float2(0.000000001, 0) + _ZStart.xy;

                    if (_JuliaFlag == 1) {
                        z = c;
                        c = _ZStart.xy;
                    }

                    float2 aux;
                    pos.y *= _Window.w / _Window.z;
                    float4 col = transparentColorAt(pos);
                    float2 dC = float2(0, 0);
                    uint iteration;
                    float smoothIteration = 0;
                    [loop]
                    for (iteration = 0; iteration < _MaxIter && length(z) < _EscapeRadius; iteration++) {
                        /*aux = polar_to_rect(float2(exp(z.x), z.y));     // e^z
                        dC = complex_mul(dC, aux);
                        dC.x += 1;
                        z = aux + c;
                        smoothIteration += exp(-_Power.x * length(z));*/
                        /*aux = complex_pow(z, float2(_Power.x - 1, _Power.y));
                        dC = complex_mul(float2(_Power.x, _Power.y), complex_mul(dC, aux));
                        dC.x += 1;
                        z = complex_mul(aux, z) + c;*/

                        aux = complex_pow(z, float2(_Power.x - 1, _Power.y));
                        dC = complex_mul(float2(_Power.x, _Power.y), complex_mul(dC, aux));
                        dC.x += 1;
                        z = complex_mul(z, aux) + c;

                        /*aux = complex_pow(z, float2(_Power.x + 1, 0));
                        dC = complex_mul(float2(_Power.x + 2, 0), complex_mul(dC, aux));
                        dC.x += 1;
                        z = complex_mul(z, aux) + c;
                        iteration++;*/
                    }
                    if (iteration > _MaxIter-1) return _TransparentFlag == 0 ? _InsideColor : _InsideColor.a * _InsideColor + (1 - _InsideColor.a) * col;
                    smoothIteration = iteration + 6 - log(log(length(z))) / log(_Power.x) /*log(_Power.x * (_Power.x + 2)) * 2 */;

                    if (smoothIteration > _MaxIter) return _TransparentFlag == 0 ? _InsideColor : _InsideColor.a * _InsideColor + (1 - _InsideColor.a) * col;

                    dC = complex_div(z, dC);
                    dC /= length(dC);

                    float2 light = polar_to_rect(TAU * _LightTextureAngle);
                    float textureX = acos(dot(dC, light)) / PI;
                    

                    light = polar_to_rect(2 * PI * _LightAngle);
                    float brightness = dot(dC, light) + _LightHeight;
                    brightness /= (1 + _LightHeight);
                    brightness = max(0, brightness);
                    //brightness = 1;

                    light = _CarpetTransformation.zw;
                    light.y /= 4;
                    light = _CarpetTransformation.xy + light * float2(textureX, smoothIteration);
                    float4 carpetCol = getCarpetColorAt(light);

                    carpetCol.rgb = carpetCol.a * carpetCol.rgb + (1 - carpetCol.a) * _OutsideColor.rgb * _OutsideColor.a;
                    carpetCol.rgb *= brightness;
                    carpetCol.a = carpetCol.a + (1 - carpetCol.a) * _OutsideColor.a;
                    return _TransparentFlag == 0 ? carpetCol : carpetCol.a * carpetCol + (1 - carpetCol.a) * col;
                    //col = tex2D(_Texture, light);
                    //return col;
                    //return (col * col.w + (1-col.w) * _Color) * brightness;
                }
                ENDCG
            }
        }
}
