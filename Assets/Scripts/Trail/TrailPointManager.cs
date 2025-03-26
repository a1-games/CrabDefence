using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailPointManager : MonoBehaviour
{
    private static TrailPointManager instance;
    public static TrailPointManager AskFor { get => instance; }
    private void Awake()
    {
        instance = this;
    }
    public Transform[] points;
}
