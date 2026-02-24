Shader "Unlit/IntersectionHighlight"
{
    Properties
    {
        _IntersectColor ("Intersection Color", Color) = (1,1,1,1)
        _BaseColor ("Base Color", Color) = (1,1,1,0.01)
        _IntersectWidth ("Intersection Width", Range(0, 2)) = 0.5
    }

    SubShader
    {
        Tags { 
            "Queue" = "Transparent" 
            "RenderType" = "Transparent" 
        }

        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
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
                float4 screenPos : TEXCOORD0;
                float eyeDepth : TEXCOORD1;
            };

            sampler2D _CameraDepthTexture;
            fixed4 _IntersectColor;
            fixed4 _BaseColor;
            float _IntersectWidth;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                o.eyeDepth = -UnityObjectToViewPos(v.vertex).z;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Profondeur de la scène (objets opaques déjà rendus)
                float sceneDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                
                // Profondeur de ce pixel transparent
                float fragDepth = i.eyeDepth;
                
                // Différence de profondeur
                float diff = sceneDepth - fragDepth;
                
                // Si diff est petit (proche de 0), on est en intersection
                float intersect = 1.0 - saturate(diff / _IntersectWidth);
                
                // Mélange entre couleur de base et couleur d'intersection
                fixed4 finalColor = lerp(_BaseColor, _IntersectColor, intersect);
                
                return finalColor;
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}