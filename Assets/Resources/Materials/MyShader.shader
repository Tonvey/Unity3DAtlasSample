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
            float4 _Rects[14];

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
            float2 converToNewUV(float2 oldUV , float4 rect)
            {
                oldUV = frac(oldUV);
                return float2(oldUV.x*rect.z+rect.x,oldUV.y*rect.w+rect.y);
            }

            float4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 rect = _Rects[_TileID];
                float4 col; 
                if(i.repeatWeight!=0)
                {
                    float2 newUV = clamp(i.uv,0.0,1.0);
                    //col = tex2D(_MainTex, i.uv);
                    col = float4(1.0,1.0,1.0,1.0);
                }
                else
                {
                    //float2 newUV = converToNewUV(i.uv,rect);
                    float2 newUV = frac(i.uv);
                    newUV = float2(rect.z * newUV.x , rect.w * newUV.y);
                    float2 dx = ddx(newUV);
                    float2 dy = ddx(newUV);
                    float extrudeUnit = 10.0 / 4176.0;
                    newUV = float2(newUV.x + rect.x , newUV.y + rect.y);
                    dx = clamp(rect.z * dx, -extrudeUnit, extrudeUnit);
                    dy = clamp(rect.w * dy, -extrudeUnit, extrudeUnit);
                    col = tex2D(_MainTex, newUV, dx, dy);
                }
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
