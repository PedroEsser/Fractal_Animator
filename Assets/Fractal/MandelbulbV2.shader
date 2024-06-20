Shader "Fractal/MandelbulbV2"
{
    Properties
    {
        _BackgroundColor("BackgroundColor", vector) = (1, 1, 1, 1)
        _InsideColor("InsideColor", vector) = (0, 0, 0, 0)
        _Position("Position", vector) = (-1, 0, 0, 0)
        _Window("Window", vector) = (1, 0, 0, 0)
        _FOV("FOV", Float) = 40
        _ViewRadius("ViewRadius", Float) = 10
        _EscapeRadius("EscapeRadius", Float) = 100
        _MaxIter("MaxIter", Float) = 10
        _Step("Step", Float) = 0.002
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
                float _EscapeRadius;
                float _Step;
                float _ViewRadius;
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


                float2 isphere(in float4 sph, in float3 ro, in float3 rd)
                {
                    float3 oc = ro - sph.xyz;
                    float b = dot(oc, rd);
                    float c = dot(oc, oc) - sph.w * sph.w;
                    float h = b * b - c;
                    if (h < 0.0) return -1.0;
                    h = sqrt(h);
                    return -b + float2(-h, h);
                }

                float map(in float3 p, out float4 resColor)
                {
                    float3 w = p;
                    float m = dot(w, w);

                    float4 trap = float4(abs(w), m);
                    float dz = 1.0;

                    for (int i = 0; i < 10; i++)
                    {
                        dz = 8.0 * pow(m, 3.5) * dz + 1.0;
                        // z = z^8+c
                        float r = length(w);
                        float b = 8.0 * acos(w.y / r);
                        float a = 8.0 * atan2(w.x, w.z);
                        w = p + pow(r, 8.0) * float3(sin(b) * sin(a), cos(b), sin(b) * cos(a));
                        trap = min(trap, float4(abs(w), m));

                        m = dot(w, w);
                        if (m > 256.0)
                            break;
                    }

                    resColor = float4(m, trap.yzw);

                    // distance estimation (through the Hubbard-Douady potential)
                    return 0.25 * log(m) * sqrt(m) / dz;
                }

                float3 calcNormal(in float3 pos, in float t, in float px)
                {
                    float4 tmp;
                    float2 e = float2(1.0, -1.0) * 0.5773 * 0.25 * px;
                    return normalize(e.xyy * map(pos + e.xyy, tmp) +
                        e.yyx * map(pos + e.yyx, tmp) +
                        e.yxy * map(pos + e.yxy, tmp) +
                        e.xxx * map(pos + e.xxx, tmp));
                }

                float softshadow(in float3 ro, in float3 rd, in float k)
                {
                    float res = 1.0;
                    float t = 0.0;
                    for (int i = 0; i < 64; i++)
                    {
                        float4 kk;
                        float h = map(ro + rd * t, kk);
                        res = min(res, k * h / t);
                        if (res < 0.001) break;
                        t += clamp(h, 0.01, 0.2);
                    }
                    return clamp(res, 0.0, 1.0);
                }

                float raycast(in float3 ro, in float3 rd, out float4 rescol, in float px)
                {
                    float res = -1.0;

                    // bounding sphere
                    float2 dis = isphere(float4(0.0, 0.0, 0.0, 1.25), ro, rd);
                    if (dis.y < 0.0) return -1.0;
                    dis.x = max(dis.x, 0.0);
                    dis.y = min(dis.y, 10.0);

                    // raymarch fractal distance field
                    float4 trap;

                    float t = dis.x;
                    for (int i = 0; i < 128; i++)
                    {
                        float3 pos = ro + rd * t;
                        float th = 0.25 * px * t;
                        float h = map(pos, trap);
                        if (t > dis.y || h < th) break;
                        t += h;
                    }

                    if (t < dis.y)
                    {
                        rescol = trap;
                        res = t;
                    }

                    return res;
                }

                const float3 light1 = float3(0.577, 0.577, -0.577);
                const float3 light2 = float3(-0.707, 0.000, 0.707);

                float3 refVector(in float3 v, in float3 n)
                {
                    return v;
                    float k = dot(v, n);
                    return (k > 0.0) ? v : v - 2.0 * n * k;
                }




                float3 render(in float2 p, in float4x4 cam)
                {
                    // ray setup
                    const float fle = 1.5;

                    //float2 sp = (2.0 * p - _ScreenParams.xy) / _ScreenParams.y;
                    float2 sp = p;
                    float px = 2.0 / (_ScreenParams.y * fle);

                    float3  ro = float3(cam[0].w, cam[1].w, cam[2].w);
                    float3  rd = normalize(mul(cam, float4(sp, fle, 1)).xyz);

                    // intersect fractal
                    float4 tra;
                    float t = raycast(ro, rd, tra, px);

                    float4 col = 0;

                    return t < 0 ? 0 : 1;

                    // color sky
                    if (t < 0.0)
                    {
                        col.xyz = float3(0.8, .9, 1.1) * (0.6 + 0.4 * rd.y);
                        col.xyz += 5.0 * float3(0.8, 0.7, 0.5) * pow(clamp(dot(rd, light1), 0.0, 1.0), 32.0);
                    }
                    // color fractal
                    else
                    {
                        // color
                        /*col = 0.01;
                        col = mix(col, float3(0.10, 0.20, 0.30), clamp(tra.y, 0.0, 1.0));
                        col = mix(col, float3(0.02, 0.10, 0.30), clamp(tra.z * tra.z, 0.0, 1.0));
                        col = mix(col, float3(0.30, 0.10, 0.02), clamp(pow(tra.w, 6.0), 0.0, 1.0));
                        col *= 0.5;*/
                        col = .5;

                        // lighting terms
                        float3  pos = ro + t * rd;
                        float3  nor = calcNormal(pos, t, px);

                        nor = refVector(nor, -rd);

                        float3  hal = normalize(light1 - rd);
                        float3  ref = reflect(rd, nor);
                        float occ = clamp(0.05 * log(tra.x), 0.0, 1.0);
                        float fac = clamp(1.0 + dot(rd, nor), 0.0, 1.0);

                        // sun
                        float sha1 = softshadow(pos + 0.001 * nor, light1, 32.0);
                        float dif1 = clamp(dot(light1, nor), 0.0, 1.0) * sha1;
                        float spe1 = pow(clamp(dot(nor, hal), 0.0, 1.0), 32.0) * dif1 * (0.04 + 0.96 * pow(clamp(1.0 - dot(hal, light1), 0.0, 1.0), 5.0));
                        // bounce
                        float dif2 = clamp(0.5 + 0.5 * dot(light2, nor), 0.0, 1.0) * occ;
                        // sky
                        float dif3 = (0.7 + 0.3 * nor.y) * (0.2 + 0.8 * occ);

                        float3 lin = 0;
                        lin += 12.0 * float3(1.50, 1.10, 0.70) * dif1;
                        lin += 4.0 * float3(0.25, 0.20, 0.15) * dif2;
                        lin += 1.5 * float3(0.10, 0.20, 0.30) * dif3;
                        lin += 2.5 * float3(0.35, 0.30, 0.25) * (0.05 + 0.95 * occ);
                        lin += 4.0 * fac * occ;
                        col.xyz *= lin;
                        col.xyz = pow(col, float3(0.7, 0.9, 1.0));
                        col.xyz += spe1 * 15.0;
                    }

                    // gamma
                    col.xyz = pow(col, float3(0.4545, 0.4545, 0.4545));

                    // vignette
                    col *= 1.0 - 0.05 * length(sp);

                    return col;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float time = _Time.x;

                    // camera
                    float di = 1.4 + 0.1 * cos(.29 * time);
                    float3 ro = di * float3(cos(.33 * time), 0.8 * sin(.37 * time), sin(.31 * time));
                    float3 ta = float3(0.0, 0.1, 0.0);
                    float cr = 0.5 * cos(0.1 * time);

                    // camera matrix
                    float3 cp = float3(sin(cr), cos(cr),0.0);
                    float3 cw = normalize(ta - ro);
                    float3 cu = normalize(cross(cw,cp));
                    float3 cv = (cross(cu,cw));
                    float4x4 cam = float4x4(cu, ro.x, cv, ro.y, cw, ro.z, 0.0, 0.0, 0.0, 1.0);

                    // render
                    return float4(render(i.uv, cam), 1);
                }
                ENDCG
            }
        }
}
