Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 10)) = 1
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                float2 texelSize = _MainTex_TexelSize.xy * _OutlineWidth;
                
                // Muestreamos los bordes interiores
                float alpha = c.a;
                float aLeft = tex2D(_MainTex, IN.texcoord + float2(-texelSize.x, 0)).a;
                float aRight = tex2D(_MainTex, IN.texcoord + float2(texelSize.x, 0)).a;
                float aUp = tex2D(_MainTex, IN.texcoord + float2(0, texelSize.y)).a;
                float aDown = tex2D(_MainTex, IN.texcoord + float2(0, -texelSize.y)).a;
                
                // Si el pixel es sólido pero algún vecino es transparente o está cerca del borde de la textura
                if (alpha > 0.1 && (aLeft < 0.1 || aRight < 0.1 || aUp < 0.1 || aDown < 0.1))
                {
                    return _OutlineColor;
                }
                
                fixed4 finalColor = c * IN.color;
                finalColor.rgb *= finalColor.a;
                return finalColor;
            }
        ENDCG
        }
    }
}
