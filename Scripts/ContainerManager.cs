using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    //will be saved with game
    public int containsWord;//will be select by editor
    public bool isLearned;
    public List<NonObjectWord> nonObjectWordsdb = new List<NonObjectWord>();
    public int containerID;
    public GameObject �nlemOBJ;
    private GameObject �nlem;
    private bool firsttime;

    private void Start()
    {
        isLearned = false;
        �nlem = GameObject.Instantiate(�nlemOBJ, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), gameObject.transform.rotation, gameObject.transform);
        firsttime = false;
    }
    private void Update()
    {
        if (isLearned)
        {
            if (firsttime)
            {
                firsttime = false;
                DestroyImmediate(�nlem);
            }
        }
    }
}
