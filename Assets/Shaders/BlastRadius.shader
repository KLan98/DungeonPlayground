Shader "Unlit/BlastRadius"
{
    Properties
    {
        _TimeScale("Speed", Float) = 1.0
        _Opacity("Opacity", Range(0.3, 0.8)) = 0.8
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform float _TimeScale;
            uniform float _Opacity;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 localPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.localPos = v.vertex.xyz;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float inverseLerp(float currentValue, float mn, float mx)
            {
                return (currentValue - mn) / (mx - mn);
            }

            float remap(float currentValue, float inMin, float inMax, float outMin, float outMax)
            {
                float t = inverseLerp(currentValue, inMin, inMax);
                return lerp(outMin, outMax, t);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float t = _Time.y * _TimeScale;
                float function = remap(sin(t), -1.0, 1.0, 0.3, 0.8);

                fixed3 outputColor = fixed3(1, 0, 0);

                return fixed4(outputColor, _Opacity * function);
            }

            ENDCG
        }
    }
}