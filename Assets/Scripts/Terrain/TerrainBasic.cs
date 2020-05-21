using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBasic : TerrainGeneric
{
    public float MoveDifficulty = 1.0f;
    public bool BlocksLineOfSight = false;
    public bool Pathable = true;
    public Color UnitLocationColor = Color.yellow;
    public Color TargetLocationColor = Color.blue;
    public Color MoveSelectableTileColor = Color.cyan;
    public Color IsUnpathableColor = Color.white;
    public Color NotSpecialColor = Color.clear;

    void Start()
    {
        IsPathable = Pathable;
        isUnitLocationColor = UnitLocationColor;
        isTargetLocationColor = TargetLocationColor;
        isMoveSelectableColor = MoveSelectableTileColor;
        isPathableColor = IsUnpathableColor;
        isNotSpecialColor = NotSpecialColor;
    }
}
