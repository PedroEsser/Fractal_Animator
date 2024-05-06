Shader "New/CubicSplineInterpolation"
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

            float2 cubic_bezier(float t, int index) {
                return (1 - t) * (1 - t) * (1 - t) * _Points[index + 0].xy
                    + 3 * (1 - t) * (1 - t) * t * _Points[index + 1].xy
                    + 3 * (1 - t) * t * t * _Points[index + 2].xy
                    + t * t * t * _Points[index + 3].xy;
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

            float DistanceToFunction(float2 p, int index)
            {
                float result = 100;
                float xDelta = 1 / _ScreenParams.x;
                
                for (float i = -3 ; i < 3 ; i += 1)
                {
                    float2 q = p;
                    q.x += xDelta * i;

                    float2 p0 = float2(q.x, cubic_bezier(q.x, index).y);
                    float2 p1 = float2(q.x + xDelta, cubic_bezier(q.x + xDelta, index).y);
                    result = min(result, DistanceToLineSegment(p0, p1, p));
                }

                return result;
            }



            fixed4 frag(v2f i) : SV_Target
            {
                float2 pos = i.uv;

                if(pos.x < _Points[0].x || pos.x > _Points[(_SplineCount - 1) * 4 + 3].x)
                    return 0;

                int index = -1;
                int it;
                for (it = 0; it < _SplineCount; it++) 
                    if (pos.x > _Points[it * 4].x && pos.x < _Points[it * 4 + 3].x) {
                        index = it*4;
                        break;
                    }

                //return DistanceToFunction(pos, index);
                float distanceToPlot = DistanceToFunction(pos, index);
                float intensity = smoothstep(0., 1., 1. - distanceToPlot * 1. * _ScreenParams.y);
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
