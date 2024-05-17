Shader "Fractal/Mandelbulb"
{
    Properties
    {
        _BackgroundColor("BackgroundColor", vector) = (1, 1, 1, 1)
        _InsideColor("InsideColor", vector) = (0, 0, 0, 0)
        _Position("Position", vector) = (-1, 0, 0, 0)
        _Window("Window", vector) = (1, 0, 0, 0)
        _FOV("FOV", Float) = 40
        _EscapeRadius("EscapeRadius", Float) = 100
        _MaxIter("MaxIter", Float) = 10
        _MaxRayLength("MaxRayLength", Float) = 4
        _Step("Step", Float) = 0.1
        _Power("Power", vector) = (8, 0, 0, 0)
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

                float4 _Position;
                float4 _Window;

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
                float _MaxRayLength;
                float _EscapeRadius;
                float _Step;
                float _FOV;
                float4 _Power;
                float4 _BackgroundColor;
                float4 _InsideColor;

                float myFrac(float v) {
                    return frac(frac(v) + 1);
                }

                float myMod(float v, float m) {
                    return (v % m + m) % m;
                }

                void sphericalToCartesian(float3 spherical, out float3 cartesian) {
                    cartesian.x = spherical.x * sin(spherical.y) * cos(spherical.z);
                    cartesian.y = spherical.x * sin(spherical.y) * sin(spherical.z);
                    cartesian.z = spherical.x * cos(spherical.y);
                }

                void cartesianToSpherical(float3 cartesian, out float3 spherical) {
                    spherical.x = length(cartesian);
                    spherical.y = acos(cartesian.z / spherical.x);
                    spherical.z = atan2(cartesian.y, cartesian.x);
                }

                float mandelbulb(float3 c) {
                    float3 z = c;
                    float3 aux = 0;
                    for (int i = 0; i < _MaxIter; i++) {
                        cartesianToSpherical(z, aux);
                        if (aux.x > 2)
                            return aux.x;
                        aux.x = pow(aux.x, _Power.x);
                        aux.y *= _Power.x;
                        aux.z *= _Power.x;
                        sphericalToCartesian(aux, z);
                        z += c;
                    }
                    return length(z);
                }

                float sdBox(float3 p, float3 b)
                {
                    float3 q = abs(p) - b;
                    return length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0);
                }

                float sdBoxFrame(float3 p, float3 b, float e)
                {
                    p = abs(p) - b;
                    float3 q = abs(p + e) - e;
                    return min(min(
                        length(max(float3(p.x, q.y, q.z), 0.0)) + min(max(p.x, max(q.y, q.z)), 0.0),
                        length(max(float3(q.x, p.y, q.z), 0.0)) + min(max(q.x, max(p.y, q.z)), 0.0)),
                        length(max(float3(q.x, q.y, p.z), 0.0)) + min(max(q.x, max(q.y, p.z)), 0.0));
                }

                //float3 castRay()

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 coord = i.uv - .5;
                    coord.y *= _ScreenParams.y / _ScreenParams.x;
                    coord += _Window.xy;
                    float3 ray = normalize(float3(coord * tan(_FOV), 1));
                    coord *= float2(_FOV, _FOV * );

                    
                    float3 step = 0;
                    sphericalToCartesian(float3(1, coord.xy), step);
                    step *= _Step;
                    float3 b = float3(2, 2, 2);
                    for (float3 ray = _Position; length(ray - _Position) < _MaxRayLength; ray += step) {
                        
                        //angle.x = mandelbulb(ray);
                       // if (angle.x < 1)
                        //    return ray.xyzx;
                        //if (length(ray) < 1)
                        //    return ray.xyzy;
                        if (sdBoxFrame(ray, b, 0.025) < 0)
                            //return length(ray - _Position) / _MaxRayLength;
                            return 1;
                    }
                    return 0;
                }
                ENDCG
            }
        }
}
