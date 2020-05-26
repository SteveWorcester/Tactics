using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainGeneric : MonoBehaviour
{
    //=========================Do not edit these vars=========================== 
    // Tile highlighting and selection 
    [HideInInspector]
    public bool UnitLocation = false;
    [HideInInspector]
    public bool TargetLocation = false;
    [HideInInspector]
    public bool SelectableTile = false;
    [HideInInspector]
    public bool IsPathable = true;
    [HideInInspector]
    public bool AttackableTile = false;

    // BFS VARS - DO NOT REMOVE/MODIFY
    [HideInInspector]
    public int Distance = 0;
    [HideInInspector]
    public List<TerrainGeneric> AdjacencyList = new List<TerrainGeneric>();
    [HideInInspector]
    public bool VisitedTile = false;
    [HideInInspector]
    public TerrainGeneric ParentTile = null;

    // Tile Colors
    [HideInInspector]
    public Color isUnitLocationColor;
    [HideInInspector]
    public Color isTargetLocationColor;
    [HideInInspector]
    public Color isMoveSelectableColor;
    [HideInInspector]
    public Color isPathableColor;
    [HideInInspector]
    public Color isNotSpecialColor;
    [HideInInspector]
    public Color isAttackableColor;

    //==========================================================================

    void Update()
    {
        if (UnitLocation)
        {
            gameObject.GetComponent<Renderer>().material.color = isUnitLocationColor;
        }
        else if (TargetLocation)
        {
            gameObject.GetComponent<Renderer>().material.color = isTargetLocationColor;
        }
        else if (AttackableTile)
        {
            gameObject.GetComponent<Renderer>().material.color = isAttackableColor;
        }
        else if (SelectableTile)
        {
            gameObject.GetComponent<Renderer>().material.color = isMoveSelectableColor;
        }
        else if (!IsPathable)
        {
            gameObject.GetComponent<Renderer>().material.color = isPathableColor;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = isNotSpecialColor;
        }
    }

    public void ResetTile()
    {
        UnitLocation = false;
        TargetLocation = false;
        SelectableTile = false;
        AttackableTile = false;
        Distance = 0;
        AdjacencyList.Clear();
        VisitedTile = false;
        ParentTile = null;
    }

    public void FindAdjacentTiles(float unitJumpHeight, bool isPathable)
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
        Vector3 halfExtents = new Vector3(0.25f, (1 + unitJumpHeight)/2.0f, .25f); 
        Collider[] tileFinders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider finder in tileFinders)
        {
            TerrainGeneric terrain = finder.GetComponent<TerrainGeneric>();
            if (terrain != null && terrain.IsPathable)
            {
                RaycastHit tileOccupied;
                if (!Physics.Raycast(terrain.transform.position, Vector3.up, out tileOccupied, 1.0f))
                {
                    AdjacencyList.Add(terrain);
                }
            }
        }
    }

    public void FindAdjacentAttackableTiles(float abilityHeightRestriction)
    {
        ResetTile();

        MarkTileAsAttackableAdjacent(Vector3.forward, abilityHeightRestriction); // forward = blue arrow
        MarkTileAsAttackableAdjacent(-Vector3.forward, abilityHeightRestriction);
        MarkTileAsAttackableAdjacent(Vector3.right, abilityHeightRestriction); // right = red arrow
        MarkTileAsAttackableAdjacent(-Vector3.right, abilityHeightRestriction);
    }

    public void MarkTileAsAttackableAdjacent(Vector3 direction, float abilityHeightRestriction)
    {
        // tiles are currently 1x1x1 size. if this size changes, then halfExtents will have to change. If this ever changes, fix it programatically 
        Vector3 halfExtents = new Vector3(0.25f, (1 + abilityHeightRestriction) / 2.0f, .25f);
        Collider[] tileFinders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider finder in tileFinders)
        {
            TerrainGeneric terrain = finder.GetComponent<TerrainGeneric>();
            if (terrain != null && IsPathable)
            {
                AdjacencyList.Add(terrain);
            }
        }
    }
}
