Shader "Fractal/MandelbrotAP"
{
    Properties
    {
        _Texture("Texture", 2D) = "white"
        _Window("Window", vector) = (0, 0, 4, 4)
        _TextureTransformation("TextureTransformation", vector) = (0, 0, 1, 1)
        _MaxIter("MaxIter", Float) = 32
        _Color("Color", vector) = (1, 1, 1, 1)
        _LightAngle("LightAngle", Float) = 0
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
            #include "Assets/Utils/ShaderCode/ArbitraryPrecision.cginc"

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
            float4 _Window;
            float4 _TextureTransformation;
            sampler2D _Texture;
            float4 _Color;
            float _LightAngle;
            float _Angle;
            float4 _ZStart;
            float4 _Power;

            fixed4 frag(v2f i) : SV_Target
            {
                uint c_re[4];
                uint c_im[4];
                uint z_re[4];
                uint z_im[4];
                uint aux_re[4];
                uint aux_im[4];
                
                zero(aux_re);
                zero(aux_im);

                float2 c = (i.uv - .5) * _Window.zw;
                c = complex_mul(polar_to_rect(_Angle), c);
                c += _Window.xy;

                load(c_re, c.x);
                load(c_im, c.y);
                load(z_re, _ZStart.x);
                load(z_im, _ZStart.y);
                uint iteration;

                [loop]
                for (iteration = 0; iteration < _MaxIter; iteration++) {
                    /*complex_ap_mul(z_re, z_im, z_re, z_im, aux_re, aux_im);
                    complex_ap_add(aux_re, aux_im, c_re, c_im, z_re, z_im);

                    complex_ap_abs(z_re, z_im, aux_re);
                    if (aux_re[1] > _EscapeRadius)
                        break;*/

                    mul(z_re, z_re, aux_re);
                    mul(z_im, z_im, aux_im);
                    add(aux_im, aux_re, aux_im);

                    if (aux_im[1] > _EscapeRadius) 
                        return (float)iteration / _MaxIter;

                    mul(z_im, z_im, aux_im);
                    negate(aux_im);
                    mul(z_re, z_re, aux_re);
                    add(aux_re, aux_im, z_re);
                    add(z_re, c_re, z_re);

                    load(aux_im, 2.0);
                    mul(aux_im, z_re, aux_im);
                    mul(aux_im, z_im, aux_im);
                    add(aux_im, c_im, z_im);
                }
                if (iteration > _MaxIter - 1) return 0;
                complex_ap_abs(z_re, z_im, aux_re);
                float smoothIteration = iteration + 6 - log(log(aux_re[1])/2) / (log(2));
                if (smoothIteration > _MaxIter) return 0;
                return smoothIteration / _MaxIter;

                /*dC = complex_div(z, dC);
                dC /= length(dC);

                float2 light = polar_to_rect(2 * PI * _LightAngle);
                float brightness = dot(dC, light) + 1.5;
                brightness /= (1 + 1.5);
                brightness = max(0, brightness);
                //brightness = 1;


                col = tex2D(_Texture, _TextureTransformation.xy + _TextureTransformation.zw * float2(brightness, smoothIteration));
                if (col.w != 0)
                    return col * brightness;

                return _Color * brightness;*/
            }
            ENDCG
        }
    }
}
