using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{


    //==========Do not change these variables=============
    [HideInInspector]
    public float halfUnitHeight = 0; 

    protected List<TerrainGeneric> _selectableTiles = new List<TerrainGeneric>();
    protected Stack<TerrainGeneric> _movePath = new Stack<TerrainGeneric>();
    protected GameObject[] allTiles;
    protected TerrainGeneric _currentTile;

    protected Vector3 moveVelocity = new Vector3();
    protected Vector3 moveHeading = new Vector3();

    //=====================================================


    // Turn counter per square moved or per move action (bool? + counter)

    void Start()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
        halfUnitHeight = GetComponent<Collider>().bounds.extents.y;
    }

    void Update()
    {

    }

    public void SetCurrentTile()
    {
        _currentTile = GetTargetTile(gameObject);
        _currentTile.UnitLocation = true;
    }

    public TerrainGeneric GetTargetTile(GameObject target)
    {
        TerrainGeneric targetTile = null;
        
        RaycastHit tileFinder;
        // The static max halfheight of any unit is 4
        try
        {
            if (Physics.Raycast(target.transform.position, -Vector3.up, out tileFinder, 4.0f))
            {
                targetTile = tileFinder.collider.GetComponent<TerrainGeneric>();
            }
            return targetTile;
        }
        catch (NullReferenceException e)
        {
            throw new NullReferenceException("Base Class Exception: The target is not standing on a tile.", e);
        }        
    }

    public void SetAdjacencyList(float jumpHeight)
    {
        foreach (var tile in allTiles)
        {
            TerrainGeneric checkTile = tile.GetComponent<TerrainGeneric>();
            checkTile.FindAdjacentTiles(jumpHeight);
        }
    }

    public void SetSelectableTiles(float jumpHeight, int move)
    {
        SetAdjacencyList(jumpHeight);
        SetCurrentTile();

        // BFS Algorithm - B.readth F.irst S.earch to find selectable tiles.
        Queue<TerrainGeneric> process = new Queue<TerrainGeneric>();
        process.Enqueue(_currentTile);
        _currentTile.VisitedTile = true;

        while (process.Count > 0)
        {
            TerrainGeneric t = process.Dequeue();
            _selectableTiles.Add(t);
            t.SelectableTile = true;
            if (t.Distance < move)
            {
                foreach (TerrainGeneric tile in t.AdjacencyList)
                {
                    if (!tile.VisitedTile)
                    {
                        tile.ParentTile = t;
                        tile.VisitedTile = true;
                        tile.Distance = 1 + t.Distance;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }
}
