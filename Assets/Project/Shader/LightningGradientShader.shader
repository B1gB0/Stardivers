Shader "Custom/LightningGradientShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _GradientTex ("Gradient Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 1.0
        _NoiseScale ("Noise Scale", Float) = 1.0
        _Emission ("Emission", Float) = 2.0
        [Toggle(USE_GRADIENT)] _UseGradient("Use Gradient", Float) = 1
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature USE_GRADIENT
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
            sampler2D _NoiseTex;
            sampler2D _GradientTex;
            float4 _GradientTex_ST;
            float _Speed;
            float _NoiseScale;
            float _Emission;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Анимация шума
                float2 noiseUV = i.uv * _NoiseScale + _Time.y * _Speed;
                float noise = tex2D(_NoiseTex, noiseUV).r;
                
                // Основная текстура с искажением
                fixed4 mainTex = tex2D(_MainTex, i.uv + noise * 0.1);
                
                // Выборка из градиентной текстуры
                #if USE_GRADIENT
                    float gradientPos = i.uv.x; // Позиция вдоль линии
                    fixed4 gradient = tex2D(_GradientTex, float2(gradientPos, 0.5));
                #else
                    fixed4 gradient = fixed4(1, 1, 1, 1);
                #endif
                
                // Комбинирование
                fixed4 col = mainTex * gradient;
                col.rgb *= _Emission;
                
                return col;
            }
            ENDCG
        }
    }
}