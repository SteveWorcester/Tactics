using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneric : MonoBehaviour
{

    // Designer playground
    public float MoveDifficulty = 1.0f;
    public float TileHeight = 1.0f;
    public bool BlocksLineOfSight = false;
    public bool PathableTile = true; 
    // nice to have: pass-through occupied tile mechanic    

    //=========================Do not edit these vars=========================== 
    // Tile highlighting and selection
    [HideInInspector]
    public bool UnitLocation = false;
    [HideInInspector]
    public bool TargetLocation = false;
    [HideInInspector]
    public bool SelectableTile = false;
    [HideInInspector]
    public int Distance = 0;

    // BFS vars (creating adjacency list)
    [HideInInspector]
    public List<TerrainGeneric> AdjacencyList = new List<TerrainGeneric>();
    [HideInInspector]
    public bool VisitedTile = false;
    [HideInInspector]
    public TerrainGeneric ParentTile = null;

    //==========================================================================

    void Start()
    {
        Debug.Log($"==MAPPING TILE ADJACENCY== \nCurrent Tile: {this.name}");
        AdjacencyList.ForEach(tile => Debug.Log($"Adjacent to {this.name}: {tile.name}"));
    }

    void Update()
    {
        if (UnitLocation)
        {
            GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (TargetLocation)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else if (SelectableTile)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void ResetTile()
    {
        UnitLocation = false;
        TargetLocation = false;
        SelectableTile = false;
        Distance = 0;
        AdjacencyList.Clear();
        VisitedTile = false;
        ParentTile = null;
    }

    public void FindAdjacentTiles(float unitJumpHeight)
    {
        ResetTile();

        MarkTileAsAdjacent(Vector3.forward, unitJumpHeight); // forward = blue arrow
        MarkTileAsAdjacent(-Vector3.forward, unitJumpHeight);
        MarkTileAsAdjacent(Vector3.right, unitJumpHeight); // right = red arrow
        MarkTileAsAdjacent(-Vector3.right, unitJumpHeight);
    }

    public void MarkTileAsAdjacent(Vector3 direction, float unitJumpHeight)
    {
        // tiles are currently 1x1x1 size. if this size changes, then halfExtents will have to change. If this ever changes, fix it programatically 
        Vector3 halfExtents = new Vector3(0.5f, (1 + unitJumpHeight)/2.0f, .5f); 
        Collider[] tileFinders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider finder in tileFinders)
        {
            TerrainGeneric terrain = GetComponent<TerrainGeneric>();
            if (terrain != null && terrain.PathableTile)
            {
                RaycastHit tileOccupied;
                if (!Physics.Raycast(terrain.transform.position, Vector3.up, out tileOccupied, 1.0f))
                {
                    AdjacencyList.Add(terrain);
                }
            }
        }
    }
}
