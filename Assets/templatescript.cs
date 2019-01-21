using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class templatescript : MonoBehaviour
{
    public KMBombInfo info;
    public KMAudio audio;

    

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    void Awake()
    {
        moduleId = moduleIdCounter++;
    }

    void Start()
    {

    }


}