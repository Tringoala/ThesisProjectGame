using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class ss : MonoBehaviour
{

    string filePath;
    public  Word[] words;
    private ObjectController oc;

    int objectid;
    Camera maincam;
    int[] EXC = { 11, 16, 17, 20, 21, 22, 24 };
    // Start is called before the first frame update
    void Start()
    {
        objectid = 0;
        filePath = Application.dataPath + "/Words";
        string[] files = Directory.GetFiles(filePath, "*.asset", SearchOption.TopDirectoryOnly);
        int c = 0;
        words = new Word[files.Length];
        foreach (string fiile in files)
        {
            string tempfiile = "Assets" + fiile.Replace("\\", "/").Replace(Application.dataPath, "");
            words[c] = AssetDatabase.LoadAssetAtPath<Word>(tempfiile);
            c += 1;
        }
        foreach (var t in words)
        {
            Debug.Log(t.name);
        }
        maincam = GameObject.Find("Main Camera").GetComponent<Camera>();



        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().materials = words[objectid].objectMaterials;

        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = words[objectid].objectMesh;

        gameObject.AddComponent<MeshFilter>();
        gameObject.GetComponent<MeshFilter>().sharedMesh = words[objectid].wordObjectMeshFilter.sharedMesh;
    }


    //Execption for elem: 11,16,17,20,21,22,24 make cam's fov 10 default is 45
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            objectid += 1;
            if (objectid >= words.Length)
            {
                objectid = 0;
            }
            gameObject.GetComponent<MeshRenderer>().materials = words[objectid].objectMaterials;
            gameObject.GetComponent<MeshCollider>().sharedMesh = words[objectid].objectMesh;
            gameObject.GetComponent<MeshFilter>().sharedMesh = words[objectid].wordObjectMeshFilter.sharedMesh;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            objectid -= 1;
            if (objectid < 0)
            {
                objectid = words.Length - 1;
            }
            gameObject.GetComponent<MeshRenderer>().materials = words[objectid].objectMaterials;
            gameObject.GetComponent<MeshCollider>().sharedMesh = words[objectid].objectMesh;
            gameObject.GetComponent<MeshFilter>().sharedMesh = words[objectid].wordObjectMeshFilter.sharedMesh;

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            takeScreenshot();
            //ss and save it to current word object
        }
    }
    private void takeScreenshot()
    {
        if (EXC.Contains(objectid))
        {
            maincam.fieldOfView = 10;
        }
        else
        {
            maincam.fieldOfView = 45;
        }
        string captured_sprite = "Assets/Words_Sprites/" + words[objectid].name + ".png";
        ScreenCapture.CaptureScreenshot(captured_sprite);
        //Use invoke to make it wait for the saving ss
        Debug.Log(captured_sprite + " " + words[objectid].name + " added");
        Texture2D texSS = AssetDatabase.LoadAssetAtPath<Texture2D>(captured_sprite);
        Sprite mysprite = Sprite.Create(texSS, new Rect(0, 0, texSS.width, texSS.height), new Vector2(0.5f, 0.5f),1);
        words[objectid].word_UI_Image = mysprite;
    }
}