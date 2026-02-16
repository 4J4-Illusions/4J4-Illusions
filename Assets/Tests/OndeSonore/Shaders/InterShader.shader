// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Intersection"
{

    Properties
    {  
        
         // Color Utilized When Not Intersecting
        _RegularColor("Main Color", Color) = (1, 1, 1, .5)
     
         // Color Utilized When Intersecting
        _HighlightColor("Highlight Color", Color) = (1, 1, 1, .5)
        
         // Maximum Difference Between Intersections
        _HighlightThresholdMax("Highlight Threshold Max", Float) = 1

         // Defines The Blurrines Quality Of The Intersections
        _Quality ("Quality", Float) = 0.5
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
            uniform float4    _RegularColor;
            uniform float4    _HighlightColor;
            uniform float     _HighlightThresholdMax;
            uniform float     _Quality;
  

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
          
      
                // Defines All Necessiary Attributes
                float4 finalColor = _RegularColor;
             
                // Recieves The Camera Distance From The Depth Buffer 
                float sceneZ = LinearEyeDepth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD (i.projPos) ).r);
                                                       
          
                // Calculates Genuine Distance To Camera
                float partZ = i.projPos.z;
            
                // If Similarites Are Discovered Between Intersecting Points Within The "Threshold" Parameters
                float diff = (abs(sceneZ - partZ)) / _HighlightThresholdMax;
      
                // Gives Intersecting Edges A Rather Charming Visual Aesthetic
                if (diff <= _Quality)
                finalColor = lerp( _HighlightColor,  _RegularColor, float4(diff, diff, diff, diff));
                               
              
        
                // Defines The Shader Texture That'll Be Released To The Camera Render
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
  
 
    // Reverts To A Classic "VertexLit" Shader If The Defined Shader Doesn't Compute
    FallBack "VertexLit"
}