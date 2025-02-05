Shader "Custom/Gray"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint",Color) = (1,1,1,1)
        _StencilComp("Stencil Comparison",Float) = 8
        _Stencil("Stencil ID",Float) = 0
        _StencilOp("Stencil Operation",Float) = 0
        _StencilWriteMask("Stencil Write Mask",Float) = 255
        _StencilReadMask("Stencil Read Mask",Float) = 255

        _ColorMask("Color Mask",Float) = 25
        _GrayValue("Gray Value",Range(0,1)) = 1

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip",Float) = 0
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

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        // No culling or depth
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(o.worldPosition);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;
                return o;
            }   

            sampler2D _MainTex;
            float _GrayValue;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                float grayColor = col.r * 0.3 + col.g * 0.59 + col.b * 0.11;
                #ifdef UNITY_UI_ALPHACLIP
                clip(col.a - 0.001);
                #endif 
                // just invert the colors
                // col.rgb = 1 - col.rgb;
                return lerp(col,fixed4(grayColor,grayColor,grayColor,col.a),_GrayValue);
            }
            ENDCG
        }
    }
}
