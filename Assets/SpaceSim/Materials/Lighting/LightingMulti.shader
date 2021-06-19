Shader "MyShady/LightingMulti"
{
    Properties
    {
        _Albedo ("Albedo", 2D) = "white" {} //;
        [NoScaleOffset] _Normals ("Normals", 2D) = "Bump" {} //;
        _NormalIntensity("Normal Intensity", Range(0,1))=1//;
        _Roughness("Roughness",2D)="Roughness"{}//;

        _Gloss ("Gloss", Range(0, 1)) = 1 //;
        _Colour ("Colour", Color) = (1, 1, 1) //;
        _glowMag ("Glow Range", Range(0, 1)) = 0.5 //;
        _glowFreq ("Glow Range Offset", Range(0, 1)) = 0.5 //;
        _glowInt ("Glow Intensity", Range (0, 1)) = 1 //;
        _glowSpeed ("Pulse Speed", Range(1, 20)) = 4 //;
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass //base pass
        {
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #define IsInBasePass
            #pragma vertex vert
            #pragma fragment frag

            #include "Maps.cginc"
            
            ENDCG
        }

        Pass //add pass
        {
            Tags {"LightMode" = "ForwardAdd"}
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd

            #include "Maps.cginc"
            
            ENDCG
        }
    }
}
