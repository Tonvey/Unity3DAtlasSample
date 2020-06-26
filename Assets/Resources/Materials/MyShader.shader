Shader "Unlit/MyShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TileID  ("TileID", int) = 0
  
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
#pragma exclude_renderers d3d11 gles
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
                float repeatWeight : TEXCOORD1;
            };

            sampler2D _MainTex;
            int _TileID;
            float4 _MainTex_ST;
            fixed4 _Rects[14];

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.repeatWeight = 0.0f;
                if(_MainTex_ST.x > 1.1f ||
                   _MainTex_ST.y > 1.1f ||
                   _MainTex_ST.x < -0.1f || 
                   _MainTex_ST.y < -0.1f)
                {
                    o.repeatWeight = 10000000.0f;
                }
                return o;
            }
            fixed2 converToNewUV(fixed2 oldUV)
            {
                fixed4 rect = _Rects[_TileID];
                oldUV = frac(oldUV);
                return fixed2(oldUV.x*rect.z+rect.x,oldUV.y*rect.w+rect.y);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed2 newUV = converToNewUV(i.uv);
                if(i.repeatWeight==0)
                {
                    newUV = clamp(newUV,0.0,1.0);
                }
                fixed4 col = tex2D(_MainTex, newUV);
                if(col.a<0.2)
                    discard;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
