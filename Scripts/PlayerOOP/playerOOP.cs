using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerOOP : MonoBehaviour
{
    public int point;
    private void Awake()
    {
        point = 0;
    }

    public int getPoint()
    {
        return this.point;
    }
    public void addPoint(int pt)
    {
        this.point += pt;
    }
}
