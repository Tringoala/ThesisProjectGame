using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField][CreateAssetMenu]
public class Local : ScriptableObject
{
    public int id;
    public string[] dialogue;
}