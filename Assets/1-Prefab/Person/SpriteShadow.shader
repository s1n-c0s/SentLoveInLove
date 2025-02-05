Shader "Custom/SpriteShadow"
{
    Properties
    {
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Color Tint", Color) = (1, 1, 1, 1)
        _FlipX("Flip X", Float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        LOD 100

        // Main Pass: Render the Sprite Normally
        Pass
        {
            Name "MainPass"
            Tags { "LightMode"="UniversalForward" }

            // Standard sprite blending and depth settings
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Properties
            sampler2D _MainTex;
            float4 _Color;
            float _FlipX;

            // Vertex structure for sprite
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 texCoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 texCoord : TEXCOORD0;
            };

            // Vertex shader
            Varyings vert(Attributes v)
            {
                Varyings o;
                float4 pos = v.positionOS;
                if (_FlipX > 0.5)
                {
                    pos.x = -pos.x;
                }
                o.positionCS = TransformObjectToHClip(float4(pos.xyz, 1.0)); // Transform to clip space
                o.texCoord = v.texCoord; // Directly pass the texture coordinates
                return o;
            }

            // Fragment shader
            half4 frag(Varyings i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.texCoord); // Sample the texture
                return texColor * _Color; // Apply color tint
            }
            ENDHLSL
        }

        // Shadow Caster Pass: Cast Shadows
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }

            // Optimized settings for shadow casting (no color or texture sampling)
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            ZTest LEqual
            Cull Off // Cull off to ensure both sides are rendered for shadows

            HLSLPROGRAM
            #pragma vertex vertShadow
            #pragma fragment fragShadow
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Properties
            float _FlipX;

            // Attributes and varying structure for shadow pass
            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            // Vertex shader for shadow pass
            Varyings vertShadow(Attributes v)
            {
                Varyings o;
                float4 pos = v.positionOS;
                if (_FlipX > 0.5)
                {
                    pos.x = -pos.x;
                }
                o.positionCS = TransformObjectToHClip(float4(pos.xyz, 1.0)); // Transform to clip space
                return o;
            }

            // Fragment shader for shadow pass
            float4 fragShadow() : SV_Target
            {
                return float4(0, 0, 0, 1); // Solid shadow (no need for transparency)
            }
            ENDHLSL
        }
    }

    FallBack "Sprites/Default"
}
