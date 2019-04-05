Shader "Unlit/Distortion_fail"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Manipulator ("Manipulator", Float) = 200.0

		_CenterX ("Center X", float) = 0
        _CenterY ("Center Y", float) = 0
        _Amount ("Amount", float) = 5
        _WaveSpeed("Wave Speed", range(.50, 50)) = 20
        _WaveAmount("Wave Amount", range(0, 20)) = 10
	}
	SubShader
	{
		// Queue is important! this object must be rendered after
		// Opaque objects.
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 100
		GrabPass{
			// "_BGTex"
			// if the name is assigned to the grab pass
			// all objects that use this shader also use a shared
			// texture grabbed before rendering them.
			// otherwise a new _GrabTexture will be created for each object.
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				// this is a slot to put our screen coordinates into
				// it is a float4 instead of float2
				// because we need to use tex2Dproj() instead of tex2D()
				float4 screenUV : TEXCOORD1;
				
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Manipulator;
			
			// builtin variable to get Grabbed Texture if GrabPass has no name
			sampler2D _GrabTexture;

			float _CenterX;
            float _CenterY;
            float _Amount;
            float _WaveSpeed;
            float _WaveAmount;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				// builtin function to get screen coordinates for tex2Dproj()
				o.screenUV = ComputeGrabScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_TARGET{

				fixed2 center = fixed2(_CenterX/_ScreenParams.x, _CenterY/_ScreenParams.y);
                fixed time = _Time.y *  _WaveSpeed;
                fixed amt = _Amount/1000;

                fixed2 uv = center.xy-i.uv;
                uv.x *= _ScreenParams.x/_ScreenParams.y;

                fixed dist = sqrt(dot(uv,uv));
                fixed ang = dist * _WaveAmount - time;
                uv = i.uv + normalize(uv) * sin(ang) * amt;




				fixed4 grab = tex2Dproj(
					_GrabTexture, 
					i.screenUV + float4(uv.x, uv.y, 0, 0)
					// i.screenUV + float4( sin((_Time.x * _Manipulator)+i.screenUV.x*4.0)*0.1, sin((_Time.x * _Manipulator)+i.screenUV.x*4.0)*0.1, 0, 0)
				);
				return grab;
			}
			ENDCG
		}
	}
}
