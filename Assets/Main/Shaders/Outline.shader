Shader "Custom/Outline" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range (0.002, 1)) = 0.005
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags {"Queue"="Overlay" "RenderType"="Opaque"}
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _OutlineColor;
        float _OutlineWidth;

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;

            // Add outline effect
            float2 d = fwidth(IN.uv_MainTex);
            float outline = 1.0 - smoothstep(0.5 - _OutlineWidth/2, 0.5 + _OutlineWidth/2, 
                                              saturate(tex2D(_MainTex, IN.uv_MainTex + d).a));

            // Mix outline color with main color
            o.Emission = _OutlineColor.rgb * outline;
        }
        ENDCG
    } 
    FallBack "Diffuse"
}
