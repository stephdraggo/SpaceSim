Shader "MyShady/Lighting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} //;
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
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 wPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _Colour;
            float4 _MainTex_ST;
            float _Gloss;
            float _glowMag;
            float _glowFreq;
            float _glowSpeed;
            float _glowInt;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //diffuse
                float3 N = normalize(i.normal);
                float3 L = _WorldSpaceLightPos0.xyz;
                float3 lambert = saturate(dot(N, L));
                float3 diffuseLight = lambert * _LightColor0.xyz;
                diffuseLight *= _Colour;

                //speculate
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float3 H = normalize(L + V);
                float specExpo = exp2(_Gloss * 10);
                float3 specularLight = saturate(dot(H, N));
                specularLight = pow(specularLight, specExpo) * _Gloss;
                specularLight *= _LightColor0.xyz;

                //fresnel pulse
                float pulse = cos(_Time.y * _glowSpeed) * _glowMag;
                float fresnel = (1 - dot(V, N)) * pulse + _glowFreq;
                fresnel *= _glowInt;


                return fixed4(specularLight + diffuseLight + fresnel, 1);
            }
            ENDCG
        }

    }
}
