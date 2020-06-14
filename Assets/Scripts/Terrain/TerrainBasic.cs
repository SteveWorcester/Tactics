using UnityEngine;

public class TerrainBasic : TerrainGeneric
{
    [Header("Tile Movement Difficulty")]
    public float AddMoveDifficulty = 0.0f;    
    public bool Pathable = true;

    [Header("Tile Colors")]
    public Color UnitLocationColor = Color.yellow;
    public Color TargetLocationColor = Color.blue;
    public Color MoveSelectableTileColor = Color.cyan;
    public Color IsUnpathableColor = Color.white;
    public Color NotSpecialColor = Color.clear;
    public Color AttackableColor = Color.red;

    [HideInInspector]
    public bool BlocksLineOfSight = false; //NYI

    void Start()
    {
        IsPathable = Pathable;
        isUnitLocationColor = UnitLocationColor;
        isTargetLocationColor = TargetLocationColor;
        isMoveSelectableColor = MoveSelectableTileColor;
        isPathableColor = IsUnpathableColor;
        isNotSpecialColor = NotSpecialColor;
        isAttackableColor = AttackableColor;
        addTileMoveDifficulty = AddMoveDifficulty;
    }
}
