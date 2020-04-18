﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : TerrainGeneric
{
    public float MoveDifficulty = 1.0f;
    public bool BlocksLineOfSight = false;
    public bool Pathable = true;

    void Start()
    {
        IsPathable = Pathable;
    }

    void Update()
    {
        
    }
}
