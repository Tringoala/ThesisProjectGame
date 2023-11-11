using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataElem
{

    public int playerPoint;
    public float[] position;
    public int[] containedID;
    public int[] learnedID;
    public int[] learnedContainerID;

    public DataElem(playerOOP player, Controller gm)
    {
        playerPoint = player.getPoint();

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        containedID = gm.containedID;
        learnedID = gm.learnedID;
        learnedContainerID = gm.learnedContainersID;
    }
}