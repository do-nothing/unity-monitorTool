Shader "Microwise/Map2" {
	Properties {
		_BgTex ("Bg Tex", 2D) = "white" {}
		_MainTex ("Map Tex", 2D) = "white" {}
		_PlayerTex ("Player Tex", 2D) = "white" {}
		_Target2 ("Target", Vector) = (0, 0, 0, 0)
		_Color2 ("Color", Vector) = (0, 0, 0, 0)
	}
	SubShader {		
		Pass { 
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			sampler2D _BgTex;
			float4 _BgTex_ST;
			sampler2D _MainTex;
			sampler2D _PlayerTex;
			float4 _PlayerTex_ST;
			float4 _Target2;
			float4 _Target3;
			float4 _Target4;
			float4 _Target5;
			float4 _Target6;
			float4 _Color2;
			float4 _Color3;
			float4 _Color4;
			float4 _Color5;
			float4 _Color6;
			sampler2D _triggers;
			
			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD1;
				float2 uv2: TEXCOORD2;
				float2 uv3: TEXCOORD3;
				float2 uv4: TEXCOORD4;
				float2 uv5: TEXCOORD5;
				float2 uv6: TEXCOORD6;				
			};

			float2 calcPlayerUv(a2v v, float4 target) {
				//move player
				float2 _PlayerTex_ST_zw = (_BgTex_ST.zw - target.xy * 0.004) * _PlayerTex_ST.y / _BgTex_ST.x;
				_PlayerTex_ST_zw.x += (2 / (_BgTex_ST.x * _BgTex_ST.y) - 1) * 0.5 * _PlayerTex_ST.x;
				_PlayerTex_ST_zw.y += (2 / _BgTex_ST.x  - 1) * 0.5 * _PlayerTex_ST.y;
				_PlayerTex_ST_zw += _PlayerTex_ST.zw;
				float2 uv = v.texcoord.xy * _PlayerTex_ST.xy + _PlayerTex_ST_zw;

				//spin player
				uv -= float2(0.5, 0.5);
				float2 spin;
				float degree = radians(target.w + 90);
				spin.x = uv.x * cos(degree) + uv.y * sin(degree);
				spin.y = uv.y * cos(degree) - uv.x * sin(degree);
				uv = spin;
				uv += float2(0.5, 0.5);

				return uv;
			}
			
			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float2 tiling = float2(_BgTex_ST.x*_BgTex_ST.y,_BgTex_ST.x);
				o.uv = v.texcoord.xy * tiling + (float2(1,1)-tiling) * 0.5 + _BgTex_ST.zw;

				o.uv2 = calcPlayerUv(v, _Target2);
				o.uv3 = calcPlayerUv(v, _Target3);
				o.uv4 = calcPlayerUv(v, _Target4);
				o.uv5 = calcPlayerUv(v, _Target5);
				o.uv6 = calcPlayerUv(v, _Target6);

				return o;
			}
			
			fixed3 calcPlayerColor(fixed3 color, float2 uv, float4 target, float2 uv1, fixed4 idColor){
				if(idColor.w <= 0){
					return color;
				}

				target *= 0.002;
				if(distance(uv, target.xy) < target.z){
					color = lerp(color, float4(0,0.5,1,1), 0.2);
				}

				fixed4 playerColor = tex2D(_PlayerTex, uv1);
				if(playerColor.r + playerColor.g + playerColor.b < 2){
					playerColor.rgb = idColor;
				}
				color = lerp(color, playerColor.rgb, playerColor.w);

				return color;
			}

			fixed4 frag(v2f i) : SV_Target {
				i.uv += 0.5;
				fixed3 color;
				if (i.uv.x > 2 || i.uv.x < 0 || i.uv.y < 0 || i.uv.y > 2){
					color = tex2D(_BgTex, i.uv).rgb;	
					i.uv *= 0.5;
				} else {
					i.uv *= 0.5;
					color = tex2D(_MainTex, i.uv).rgb;	
					fixed4 trigersColor = tex2D(_triggers, i.uv);
					color = lerp(color, trigersColor, trigersColor.w);
				}

				color = calcPlayerColor(color, i.uv, _Target2, i.uv2, _Color2);
				color = calcPlayerColor(color, i.uv, _Target3, i.uv3, _Color3);
				color = calcPlayerColor(color, i.uv, _Target2, i.uv4, _Color4);
				color = calcPlayerColor(color, i.uv, _Target3, i.uv5, _Color5);
				color = calcPlayerColor(color, i.uv, _Target2, i.uv6, _Color6);

				return fixed4(color, 1.0);
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
