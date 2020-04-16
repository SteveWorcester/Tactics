using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneric : MonoBehaviour
{
    // regular movement? difficult movement? add to turn counter? reduce move by X instead of block?
    // LOS block bool?
    public float MoveDifficulty;
    public float TileHeight;
    public bool Pathable;
    public bool BlocksLineOfSight;

    

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(MoveDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
