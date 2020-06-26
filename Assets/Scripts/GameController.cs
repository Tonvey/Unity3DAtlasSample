using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Data;
using Newtonsoft.Json;

class AtlasFrameSize
{
    public int w;
    public int h;
}
class Quard<T>
{
    public Quard() { }
    public Quard(T x, T y, T w, T h)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
    }
    public T x;
    public T y;
    public T w;
    public T h;
}
class AtlasFrame
{
    public string filename { get; set; }
    public Quard<int> frame { get; set; }
    public bool rotated { get; set; }
    public bool trimmed { get; set; }
    public Quard<int> spriteSourceSize { get; set; }
    public AtlasFrameSize sourceSize { get; set; }
}

class AtlasMeta
{
    public string app { get; set; }
    public string Version { get; set; }
    public string image { get; set; }
    public AtlasFrameSize size { get; set; }
    public string smartupdate { get; set; }
}
class AtlasFrames
{
    public AtlasFrame[] frames;
    public AtlasMeta meta;
}
public class GameController : MonoBehaviour
{
    public GameObject planePrefab;
    public GameObject planeRepeatePrefab;
    // Start is called before the first frame update
    void Start()
    {
        string str = File.ReadAllText("Assets/Resources/Data/texture.json");//查找在StreamingAssets文件下的Json文件

        Debug.Log(str);
        var dt = (AtlasFrames)JsonConvert.DeserializeObject(str,typeof(AtlasFrames));
        var imageSize = dt.meta.size;
        var unitU = 1.0f / (imageSize.w);
        var unitV = 1.0f / (imageSize.h);
        var textureInfos = new Dictionary<string, Quard<float>>();
        var nameList = new List<string>();
        var rectList = new List<Vector4>();
        Debug.Log("frames length is :"+ dt.frames.Length);


        int id = 0;
        foreach (var i in dt.frames)
        {
            Debug.Log(i.filename);
            var pixelBeginX = i.frame.x;
            var pixelBeginY = imageSize.h - i.frame.y - i.frame.h;
            var beginU = (float)pixelBeginX / (imageSize.w - 1);
            var beginV = (float)pixelBeginY / (imageSize.h - 1);
            var widthInU = (i.frame.w - 1) * unitU;
            var heightInV = (i.frame.h - 1) * unitV;
            textureInfos.Add(i.filename, new Quard<float>(beginU,beginV,widthInU,heightInV));

            nameList.Add(i.filename);
            rectList.Add(new Vector4(beginU, beginV, widthInU, heightInV));

            var obj = Instantiate(planePrefab, new Vector3(id * 15.0f, 0, 0),new Quaternion());
            var objRepeat = Instantiate(planeRepeatePrefab, new Vector3(id * 15.0f, 15.0f, 0), new Quaternion());
            var myPlane = obj.GetComponent<MyPlane>();
            myPlane.tileID = id;
            myPlane = objRepeat.GetComponent<MyPlane>();
            myPlane.tileID = id;

            ++id;
        }
        GlobalContext.Rects = rectList.ToArray();

        var plane = GameObject.Find("Plane");
        //var mat = plane.GetComponent<Renderer>().material;
        //var mat = Resources.Load<Material>("Assets/Materials/MyMat.mat");
        //mat.SetVectorArray("_Rects", GlobalContext.Rects);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
