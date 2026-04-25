Shader "Unlit/BlastRadius"
{
    Properties
    {
        _Red("Red", Color) = (1, 0, 0, 1)
        _TimeScale("Speed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Red;
            float _TimeScale;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Create a sine wave that oscillates between 0 and 1
                float t = sin(_Time.y * _TimeScale) * 0.5 + 0.5;

                // Multiply color by the sine value (blinking effect)
                return _Red * t;
            }
            ENDCG
        }
    }
}