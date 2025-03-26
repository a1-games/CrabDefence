using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPainter : MonoBehaviour
{
    public LineRenderer line;
    private void Start()
    {
        Vector3[] tmpV = new Vector3[TrailPointManager.AskFor.points.Length];
        for (int i = 0; i < tmpV.Length; i++)
        {
            tmpV[i] = TrailPointManager.AskFor.points[i].position;
        }
        line.positionCount = tmpV.Length;
        line.SetPositions(tmpV);
    }
}
