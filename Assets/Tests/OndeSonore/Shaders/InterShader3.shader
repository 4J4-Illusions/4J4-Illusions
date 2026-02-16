Shader "Unlit/TwoPasses"
{
    SubShader
    {
        LOD 100

        Tags { 
            "Queue" = "Transparent" 
            "RenderType" = "Transparent" 
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        // Collided color
        Pass  // Executes this first
        {
            Tags { "LightMode" = "SRPDefaultUnlit" }

            // Blend SrcAlpha OneMinusSrcAlpha

            Cull Off
            // ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Green
                // return float4(0,1,0,0);
                // White
                return float4(1,1,1,0);
                // White full opaque
                // return float4(1,1,1,1);
            }

            ENDCG
        }

        // Non-collided color
        Pass  // Executes this second
        {
            Tags { "LightMode" = "UniversalForward" }

            // Blend SrcAlpha OneMinusSrcAlpha
            // ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Red
                // return float4(1,0,0,0);
                // Red full opaque
                // return float4(1,0,0,1);
                // White with opacity
                return float4(1,1,1,.01);
            }
            ENDCG
        }
    }
}
