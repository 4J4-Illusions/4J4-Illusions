// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//Highlights intersections with other objects
Shader "Custom/IntersectionHighlights"
{
    Properties
    {
        _RegularColor("Main Color", Color) = (1, 1, 1, .5) //Color when not intersecting
        _HighlightColor("Highlight Color", Color) = (1, 1, 1, .5) //Color when intersecting
        _HighlightThresholdMax("Highlight Threshold Max", Float) = 1 //Max difference for intersections
        _ZBias("Highlight Z Bias", Float) = 2.5    //Balance out the Z-axis fading
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent"  }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform sampler2D _CameraDepthTexture; //Depth Texture
            uniform float4 _RegularColor;
            uniform float4 _HighlightColor;
            uniform float _HighlightThresholdMax;
            uniform float _ZBias;
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 projPos : TEXCOORD1; //Screen position of pos
            };
            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.projPos = ComputeScreenPos(o.pos);
                return o;
            }
            half4 frag(v2f i) : COLOR
            {
                float4 finalColor = _RegularColor;
                //Get the distance to the camera from the depth buffer for this point
                float sceneZ = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r * 400;

                //Actual distance to the camera
                float partY = i.projPos.y + (i.projPos.y/_ZBias);

                //If the two are similar, then there is an object intersecting with our object
                float diff = (abs(sceneZ - partY)) / _HighlightThresholdMax;
                if (diff <= 1)
                {
                    finalColor = lerp(_HighlightColor, _RegularColor, diff);
                }
               
                half4 c;
                c.r = finalColor.r;
                c.g = finalColor.g;
                c.b = finalColor.b;
                c.a = finalColor.a;
                return c;
            }
            ENDCG
        }
    }
    FallBack "VertexLit"
}
