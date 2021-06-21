Shader "MyShady/shad1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} //;
        _ColourA ("Colour A", Color) = (0,0,0,0) //;
        _ColourB ("Colour B", Color) = (0,0,0,0) //;
        _ColourStart ("Colour Start", Range(0, 1)) = 1 //;
        _ColourEnd ("Colour End", Range(0, 1)) = 0 //;
    }
    SubShader
    {
        Tags
        { 
        "RenderType" = "Transparent"
        "Queue" = "Transparent"
        }

        //no LOD thanks
        //level of detail
        //render distance      
        //LOD 100 //max LOD allowed

        Pass
        {
            Cull Off //Back/Front
            ZWrite Off
            //ZTest Always //quest marker
            //Ztest GEqual //lens of truth
            Blend One One
            //BlendOp Sub

            //Shader code starts here
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float2 uv : TEXCOORD0; //uv is vector2
                //float2 uv1 : TEXCOORD1; //uv is vector2
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _ColourA;
            float4 _ColourB;
            float4 _MainTex_ST;
            float _ColourStart;
            float _ColourEnd;

            #define TAU 6.28318530718
            #define PI 3.14

            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal=UnityObjectToWorldNormal(v.normals);
                o.uv = v.uv; //(v.uv+_ColourEnd)*_ColourStart; //TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float InverseLerp(float a, float b, float v)
            {
                return (v - a) / (b - a);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                

                //float t = (InverseLerp(_ColourStart, _ColourEnd, i.uv.x));

                //float t = abs(frac(i.uv.x * 5) * 2 - 1);

                //_Time.xyzw;
                //x=seconds/20, y=seconds, z=seconds*2, w=seconds*3
                

                //switch x and y to rotate direction 90 degrees
                float xOffset = cos(i.uv.x * PI * 16) * 0.1; //make zigzag
                
                float t = cos((i.uv.y + xOffset + _Time.y) * PI * 5) * 0.5 + 0.5; //the 0.5 makes the colours what you chose

                t *= i.uv.y;

                t *= abs(i.normal.y) < 0.999;

                //lerpy slurpy
                float4 outColour = lerp(_ColourA, _ColourB, i.uv.y);
                outColour*=t;
                return outColour;

                // sample the texture
                fixed4 gradient = fixed4(outColour); //tex2D(_MainTex, i.uv)   float4(_ColourA.yy,_ColourA.xx)   fixed4(i.normal,1)
                return gradient;
            }
            ENDCG
        }
    }
}
