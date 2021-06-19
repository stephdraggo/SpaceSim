Shader "MyShady/healthbar"
{
    Properties
    {
        [NoScaleOffset]
        _MainTex ("Texture", 2D) = "white" {} //;
        _Health ("Health", Range(0,1)) = 1 //;
        _Chunks ("Chunks", int) = 8 //;
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Health;
            int _Chunks;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed InverseLerp(fixed a, fixed b, fixed v)
            {
                return (v-a)/(b-a);
            }

            fixed ChunkIt(fixed a)
            {
                return floor(a*_Chunks)/_Chunks;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //good health for longer
                fixed tHealthColour = saturate(InverseLerp(0, 0.8, ChunkIt(_Health))); //above .8 it green

                //texture
                fixed4 colour = tex2D(_MainTex, fixed2(tHealthColour, i.uv.y));

                //mask
                fixed healthBarMask = _Health > ChunkIt(i.uv.x); //get the amount of health and the rest is black/clear

                fixed healthFlash = (0.2 > _Health) * 0.5; //flashes between -0.5 and 0.5

                fixed flash = cos(_Time.y * 10) * healthFlash + 1; //flash in time between 0.5 and 1.5

                //fixed flash = cos(_Time.y * 2 / _Health) * 0.5 + 1; //this one works fine

                //colour at transparency
                return fixed4(colour.rgb * healthBarMask, healthBarMask * flash);
            }
            ENDCG
        }
    }
}
