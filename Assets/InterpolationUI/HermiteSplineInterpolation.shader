Shader "New/HermiteSplineInterpolation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Window  ("Window", vector) = (0, 0, 0, 0)
        _SplineCount("SplineCount", Int) = 0
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


            sampler2D _MainTex;
            float4 _Window;
            float4 _Points[256];
            int _SplineCount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = (v.uv - .5) * _Window.zw + _Window.xy;
                return o;
            }

            float hermite_interpolation(float t) {

                float transformedT;
                int l = 0, r = _SplineCount, index;
                while (l < r) {
                    index = floor((l + r)/2);
                    if (t < _Points[index * 2].x) {
                        r = index;
                        continue;
                    }
                    if (t > _Points[index * 2].y) {
                        l = index;
                        continue;
                    }
                    break;
                }
                index *= 2;
                transformedT = (t - _Points[index].x) / (_Points[index].y - _Points[index].x);
                float transformedTSquared = transformedT * transformedT;
                float oneMinusTSquared = 1 - transformedT;
                oneMinusTSquared *= oneMinusTSquared;

                float value = _Points[index + 1].x * (transformedT * oneMinusTSquared) +
                    (transformedTSquared * (3 - 2 * transformedT)) +
                    _Points[index + 1].y * (transformedTSquared * (transformedT - 1));

                return value * (_Points[index + 0].w - _Points[index + 0].z) + _Points[index + 0].z;
            }

            float DistanceToLineSegment(float2 p0, float2 p1, float2 p)
            {
                float distanceP0 = length(p0 - p);
                float distanceP1 = length(p1 - p);

                float l2 = pow(length(p0 - p1), 2.);
                float t = max(0., min(1., dot(p - p0, p1 - p0) / l2));
                float2 projection = p0 + t * (p1 - p0);
                float distanceToProjection = length(projection - p);

                return min(min(distanceP0, distanceP1), distanceToProjection);
            }

            float DistanceToFunction(float2 p)
            {
                float result = 100;
                float xDelta = 1 / _Window.z;
                
                for (float i = -3 ; i < 3 ; i += 1)
                {
                    float2 q = p;
                    q.x += xDelta * i;
                    if (q.x < _Points[0].x || q.x + xDelta > _Points[(_SplineCount - 1) * 2].y)
                        continue;

                    float2 p0 = float2(q.x, hermite_interpolation(q.x));
                    float2 p1 = float2(q.x + xDelta, hermite_interpolation(q.x + xDelta));
                    result = min(result, DistanceToLineSegment(p0, p1, p));
                }

                return result;
            }



            fixed4 frag(v2f i) : SV_Target
            {
                float2 pos = i.uv;

                

                /*int index = -1;
                int it;
                for (it = 0; it < _SplineCount; it++) 
                    if (pos.x > _Points[it * 2].x && pos.x < _Points[it * 2].y) {
                        index = it*2;
                        break;
                    }*/

                float distanceToPlot = DistanceToFunction(pos);
                float intensity = smoothstep(0., 1., 1. - distanceToPlot / _Window.w * 200);
                intensity = pow(intensity, 1. / 2.2);

                return intensity;
                /*it = 0;
                float2 f;
                float t = (pos.x - _Points[index].x) / (_Points[index + 3].x - _Points[index].x);
                float l = 0;
                float r = 1;
                while (it++ < 32) {
                    f = cubic_bezier(t, index);

                    if (f.x > pos.x)
                        r = t;
                    else
                        l = t;

                    t = (l + r) / 2;
                }

                //t = length((f - pos)  _ScreenParams.yx / _Window.wz );
                t = abs(((f - pos) / _Window.wz).y));
                if (t < .02)
                    return (.02 - t) / .02;
                return 0;*/
            }
            ENDCG
        }
    }
}
