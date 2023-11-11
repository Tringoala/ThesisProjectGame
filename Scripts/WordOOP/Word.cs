using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu]
public class Word : ScriptableObject 
{
    public int id;
    public string wordSTR_EN;
    public string wordSTR_TR;
    public Sprite word_UI_Image;
    public MeshRenderer wordObjectMeshRenderer;
    public Mesh objectMesh;
    public MeshFilter wordObjectMeshFilter;
    public Material[] objectMaterials;
    public bool isLearned;
    public bool isStudied;
}
