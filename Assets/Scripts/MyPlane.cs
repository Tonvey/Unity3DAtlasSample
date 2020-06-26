using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlane : MonoBehaviour
{
    //public Material myMat;
    public int tileID = 0;
    private Vector3[] vertices = new Vector3[4];
    private int[] indices = new int[6];
    public Vector2[] uvs = new Vector2[4];
    void makeMesh()
    {
        var obj = this.gameObject;
        MeshFilter mf = obj.GetComponent<MeshFilter>();
        if (mf == null)
        {
            mf = obj.AddComponent<MeshFilter>();
        }
        MeshRenderer mr = obj.GetComponent<MeshRenderer>();
        if (mr == null)
        {
            mr = obj.AddComponent<MeshRenderer>();
        }
        mf.mesh.vertices = vertices;
        mf.mesh.triangles = this.indices;
        mf.mesh.uv = uvs;
        var material = new Material(Shader.Find("Unlit/MyShader"));
        var textureRes = Resources.Load("Images/texture") ;
        var texture = textureRes as Texture2D;
        material.SetTexture("_MainTex", texture);
        material.SetInt("_TileID", this.tileID);
        material.SetVectorArray("_Rects", GlobalContext.Rects);
        mr.material = material;
        obj.AddComponent<MeshCollider>();
    }
    void makePlane()
    {
        this.vertices[0].Set(-0.5f, -0.5f, 0.0f);
        this.vertices[1].Set(-0.5f, 0.5f, 0.0f);
        this.vertices[2].Set(0.5f, 0.5f, 0.0f);
        this.vertices[3].Set(0.5f, -0.5f, 0.0f);
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        indices[3] = 0;
        indices[4] = 2;
        indices[5] = 3;
    }
    // Start is called before the first frame update
    void Start()
    {
        makePlane();
        makeMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
