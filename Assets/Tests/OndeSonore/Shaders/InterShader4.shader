Shader "Custom/Intersect/PassesWithStencil"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        //Rendertype: Opaque, Transparent or Overlay all give same result...
        //Queue is important here! Must be over 2500 to get front to back rendering, Overlay = 4000 (Transparent also works..)
        Tags
        {
            "RenderType"="Opaque" "Queue" = "Overlay"
        }
        LOD 100
        ZWrite Off
        //first pass sets the mask for the "front geometry"
        Pass
        {
            Cull Back
            ZTest Greater
            ColorMask 0

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

        }
        //second pass turn off culling, could be Cull Off or Cull Front (both acheive the same thing)
        //use the mask from the first pass and draw the color
        Pass
        {
            Cull Off
            ZTest Greater

            Stencil
            {
                Ref 1
                Comp NotEqual
            }

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


            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color; // just return it
            }
            ENDCG
        }
        // reset the stencil buffer so other objects dont mask out this one
        Pass
        {
            //Cull Back
            ZTest Greater
            ColorMask 0

            Stencil
            {
                Ref 0
                Comp Always
                Pass Zero
            }

        }
    }
}
