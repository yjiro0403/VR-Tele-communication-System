﻿
Shader "Custom/transparentwithshadow"
{
	Properties{
		_Color("Main Color", Color) = (0, 0, 0, 0)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	}
	SubShader{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "Geometry" }
		blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		CGPROGRAM
		#pragma surface surf Lambert exclude_path : prepass
		sampler2D _MainTex;
	fixed4 _Color;
	struct Input{
		float2 uv_MainTex;
	};
	
	void surf(Input IN, inout SurfaceOutput o){
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
		}
	ENDCG
	}

		Fallback "VertexLit"


}
