using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    public Word word;
    [HideInInspector]
    public int idObject;
    public GameObject �nlemOBJ;
    private bool firsttime2;
    private GameObject �nlem;

    private GameObject controller;
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.tag = "LearnableObject";
    }
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<Controller>().newgame)
        {
            word.isLearned = false;
        }

        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().materials = word.objectMaterials;

        gameObject.AddComponent<MeshCollider>();
        gameObject.GetComponent<MeshCollider>().sharedMesh = word.objectMesh;

        gameObject.AddComponent<MeshFilter>();
        gameObject.GetComponent<MeshFilter>().sharedMesh = word.wordObjectMeshFilter.sharedMesh;

        if (word.isLearned)
        {
            idObject = word.id;
            firsttime2 = false;
        }
        else
        {
            if (word.id == 26)
            {
                �nlem = GameObject.Instantiate(�nlemOBJ, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), gameObject.transform.rotation, gameObject.transform);
            }
            else
            {
                �nlem = GameObject.Instantiate(�nlemOBJ, gameObject.transform);
            }

            idObject = word.id;
            firsttime2 = true;
        }
    }
    private void Update()
    {
        if (word.isLearned)
        {
            gameObject.tag = "LearnedObject";
            if (firsttime2)
            {
                DestroyImmediate(�nlem);
                firsttime2=false;
            }
        }
        else
        {
            gameObject.tag = "LearnableObject";
        }
    }
}
