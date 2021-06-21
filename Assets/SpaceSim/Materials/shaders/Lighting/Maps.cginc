#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

#define UseLighting

struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    float3 normal : TEXCOORD1;
    float3 wPos : TEXCOORD2;
    float3 tangent : TEXCOORD3;
    float3 biTangent : TEXCOORD4;
    LIGHTING_COORDS(5,6)
};

sampler2D _Albedo;
sampler2D _Normals;
float _NormalIntensity;
sampler2D _Roughness;
float4 _Colour;
float4 _Albedo_ST;
float _Gloss;
float _glowMag;
float _glowFreq;
float _glowSpeed;
float _glowInt;

v2f vert (appdata v)
{
    v2f o;
    o.uv = TRANSFORM_TEX(v.uv, _Albedo);
    o.vertex = UnityObjectToClipPos(v.vertex);

    o.normal = UnityObjectToWorldNormal(v.normal);
    o.tangent = UnityObjectToWorldDir(v.tangent.xyz);
    o.biTangent = cross(o.normal, o.tangent);
    o.biTangent *= v.tangent.w * unity_WorldTransformParams.w;

    o.wPos = mul(unity_ObjectToWorld, v.vertex);

    TRANSFER_VERTEX_TO_FRAGMENT(o);
    return o;
}

fixed4 frag (v2f i) : SV_Target
{
    
    float3 albedo = tex2D(_Albedo, i.uv).rgb;
    float3 surfaceColour = albedo * _Colour.rgb;
    
    float3 tangentSpaceNormal = UnpackNormal(tex2D(_Normals,i.uv));
    tangentSpaceNormal = normalize(lerp(float3(0,0,1),tangentSpaceNormal,_NormalIntensity));
    float3x3 mtxTanToWorld =
    {
        i.tangent.x, i.biTangent.x, i.normal.x,
        i.tangent.y, i.biTangent.y, i.normal.y,
        i.tangent.z, i.biTangent.z, i.normal.z,
    };

    float3 N = normalize(mul(mtxTanToWorld,tangentSpaceNormal));

    #ifdef UseLighting

    //diffuse
    float3 L = normalize(UnityWorldSpaceLightDir(i.wPos)); //_WorldSpaceLightPos0.xyz;
    float attenuation = LIGHT_ATTENUATION(i);
    float3 lambert = saturate(dot(N, L));
    float3 diffuseLight = lambert * attenuation * _LightColor0.xyz;
    diffuseLight *= _Colour;

    //speculate
    float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
    float3 H = normalize(L + V);

    float specExpo = exp2(_Gloss * 10);
    float3 specularLight = saturate(dot(H, N));
    specularLight = pow(specularLight, specExpo) * _Gloss * attenuation;
    specularLight *= _LightColor0.xyz;

    //fresnel pulse
    float pulse = cos(_Time.y * _glowSpeed) * _glowMag;
    float fresnel = (1 - dot(V, N)) * pulse + _glowFreq;
    fresnel *= _glowInt;


    return fixed4(specularLight + diffuseLight*surfaceColour + fresnel, 1);

    #else

        #ifdef IsInBasePass
            return _Colour;
        #else
            return 0;
        #endif

    #endif
}