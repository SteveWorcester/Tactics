using UnityEngine;

public class MeleeMove : UnitMove
{
    // Edit these defaults    
    [Header("Moving")]
    public float MoveSpeed = 10.0f; // how fast the unit traverses the map. This has nothing to do with turn order speed.

    [Header("Jumping")]
    public float MoveSlowdownMultiplier = 1000.0f; // higher = slower movement towards the jump
    public float JumpSlowdownMultiplier = 1000.0f; // higher = slower jump speed.
    public float JumpVelocity = 0.005f; // "power" of the jump.

    void Start()
    {
        jumpVelocity = JumpVelocity;
        jumpHeightSlowdown = JumpSlowdownMultiplier;
        jumpMoveSlowdown = MoveSlowdownMultiplier;
        _unitMoveSpeed = MoveSpeed;
        allTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
        halfUnitHeight = GetComponent<Collider>().bounds.extents.y;
        Init();
    }

    private void Update()
    {
        if (!unitCharacter._InMovePhase)
        {
            return;
        }
        if (!currentlyMoving)
        {
            SetAdjacencyList(unitCharacter._JumpHeight);
            SetSelectableTiles(unitCharacter._JumpHeight, unitCharacter._MoveDistance);
            CheckMouseToMove();
        }
        if (currentlyMoving)
        {
            Move();
        }
        if (_hasMoved)
        {
            EndMovePhase();
        }
    }
}
