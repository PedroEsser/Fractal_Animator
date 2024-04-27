Shader "New/RealLineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Background("Background", vector) = (1, 1, 1, 1)
        _Foreground("Foreground", vector) = (1, 1, 1, 1)
        _CurrentT("CurrentT", Float) = 0
        _Zoom    ("Zoom", Float) = 0
        _Delta   ("Delta", Float) = 0
        _Interval("Interval", vector) = (0, 10, 0, 0)
        _VerticalFlag("VerticalFlag", Int) = 0
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

            int _VerticalFlag;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                if(_VerticalFlag == 0)
                    o.uv = v.uv;
                else
                    o.uv = v.uv.yx;
                return o;
            }

            sampler2D _MainTex;
            float _CurrentT;
            float _Delta;
            float _Zoom;
            float4 _Interval;
            float4 _Background;
            float4 _Foreground;

            fixed4 frag(v2f i) : SV_Target
            {
                float y = abs(i.uv.y - .5);
                
                if (y > 0.3)
                    return _Background;

                float x = _CurrentT + (i.uv.x - .5) * _Zoom;
                float aux = frac(x / _Delta);
                aux = 0.5 - abs(aux - .5);
                aux *= _Delta * _ScreenParams.y / _Zoom;

                if (aux > 2)
                    return _Background;

                if(x >= _Interval.x && x <= _Interval.y || _Interval.x > _Interval.y)
                    return _Foreground;

                return _Foreground * .25;
            }
            ENDCG
        }
    }
}
