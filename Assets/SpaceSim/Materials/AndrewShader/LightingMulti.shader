Shader "Unlit/LightingMulti"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Gloss("Gloss", Range(0,1)) = 1
        _Color("Color", Color) = (1,1,1,1)
        _glowMag("glowMag", Range(0,1)) = 0.5
        _glowFreq("glowFreq", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        //Base pass
        Pass
        {
            Tags{"LightMode" = "ForwardBase"}     
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Maps.cginc"
            
            ENDCG
        }

        //Add pass
        Pass
        {
            Tags{"LightMode" = "ForwardAdd"}
            Blend One One // src * 1 + dst * 1
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Maps.cginc"
            
            ENDCG
        }
    }
}
