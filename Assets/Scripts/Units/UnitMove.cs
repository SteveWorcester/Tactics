using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{

    //==========Do not change these variables=============
    [HideInInspector]
    public float halfUnitHeight = 0.0f;
    protected float _unitMoveSpeed = 2.0f;
    protected float jumpMoveSlowdown = 3.0f;
    private float tileCenterFudge = .05f; // if you are this close to where you are supposed to be, then you "pop" to that location
    private float jumpWiggleRoom = .15f; // The distance before the tile when you jump; also the distance above the destination you jump over and fall back down

    protected List<TerrainGeneric> _selectableTiles = new List<TerrainGeneric>();
    protected Stack<TerrainGeneric> _movePath = new Stack<TerrainGeneric>();
    protected GameObject[] allTiles;
    
    [HideInInspector]
    public bool currentlyMoving = false;
    protected Vector3 moveVelocity = new Vector3();
    public Vector3 moveHeading = new Vector3();
    private Vector3 jumpPosition;
    private Vector3 heightToJumpOrFall;
    private bool jumpingToTarget = false;
    private bool movingToEdge = false;    

    [HideInInspector]
    public bool _hasMoved = false;

    [HideInInspector]
    public UnitCharacter unitCharacter;

    //=====================================================

    public void Init()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
        halfUnitHeight = gameObject.GetComponent<Collider>().bounds.extents.y;
        unitCharacter = gameObject.GetComponent<UnitCharacter>();
    }

    public void StartMovePhase()
    {
        Debug.Log("UnitMove starting move phase");
        _hasMoved = false;
        unitCharacter._InMovePhase = true;
    }

    public void EndMovePhase()
    {
        Debug.Log($"unit moved: {unitCharacter}. changing to character control.");
        unitCharacter._FullTurnCounter += (unitCharacter._TurnCostMove);
        unitCharacter._MovesLeftThisTurn--;
        SetCurrentTile();
        unitCharacter._InMovePhase = false;
    }

    public void Move()
    {     
        if (_movePath.Count > 0)
        {
            TerrainGeneric nextTile = _movePath.Peek();         
            Vector3 moveTarget = nextTile.transform.position;
            moveTarget.y += halfUnitHeight + nextTile.GetComponent<Collider>().bounds.extents.y;
            if (Vector3.Distance(transform.position, moveTarget) >= tileCenterFudge - .01f) // .01f because we want the unit to "get there" before anything else happens
            {
                var jump = transform.position.y != moveTarget.y;
                if (jump)
                {
                    Jump(moveTarget);
                }
                else
                {
                    SetHeadingDirection(moveTarget);
                    SetMoveVelocity();
                }

                transform.forward = moveHeading;
                transform.position += moveVelocity * Time.deltaTime;
            }
            else
            {                
                transform.position = moveTarget;
                _movePath.Pop();
            }
        }
        else
        {
            ClearSelectableTiles();
            currentlyMoving = false;
            _hasMoved = true;
        }
    }
    

    #region Tile-specific

    public void SetCurrentTile()
    {
        unitCharacter._currentTile = GetTargetTile(gameObject);
        unitCharacter._currentTile.UnitLocation = true;
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
            checkTile.FindAdjacentTiles(jumpHeight, checkTile.IsPathable);
        }
    }

    public void SetSelectableTiles(float jumpHeight, int move)
    {
        SetAdjacencyList(jumpHeight);
        SetCurrentTile();
        

        // BFS Algorithm - B.readth F.irst S.earch to find selectable tiles.
        Queue<TerrainGeneric> process = new Queue<TerrainGeneric>();
        process.Enqueue(unitCharacter._currentTile);
        unitCharacter._currentTile.VisitedTile = true;

        while (process.Count > 0)
        {
            TerrainGeneric t = process.Dequeue();
            _selectableTiles.Add(t);
            t.SelectableTile = true;
            if (t.Distance < move && t.IsPathable)
            {
                foreach (TerrainGeneric tile in t.AdjacencyList)
                {
                    if (!tile.VisitedTile)
                    {
                        tile.ParentTile = t;
                        tile.VisitedTile = true;
                        tile.Distance = 1 + t.Distance + tile.addTileMoveDifficulty;
                        process.Enqueue(tile);
                    }
                }
            }
        }
    }

    public void ClearSelectableTiles()
    {
        if (unitCharacter._currentTile != null)
        {
            unitCharacter._currentTile.UnitLocation = false;
            unitCharacter._currentTile = null;
            
        }
        foreach (TerrainGeneric tile in _selectableTiles)
        {
            tile.ResetTile();
        }
        _selectableTiles.Clear();
    }

    #endregion

    #region Move-specific

    public void SetMoveVelocity()
    {
        moveVelocity = moveHeading * _unitMoveSpeed;
    }

    public void CheckMouseToMove()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray mouseCheck = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit tileCheck;
            if (Physics.Raycast(mouseCheck, out tileCheck) && tileCheck.collider.tag.Equals("Terrain Tile"))
            {
                TerrainGeneric clickedTile = tileCheck.collider.GetComponent<TerrainGeneric>();
                if (clickedTile.SelectableTile)
                {
                    SetMovementPath(clickedTile);
                }
            }
        }
    }

    public void SetMovementPath(TerrainGeneric tile)
    {
        _movePath.Clear();
        tile.TargetLocation = true;        

        TerrainGeneric nextTile = tile;
        while (nextTile != null)
        {
            _movePath.Push(nextTile);
            nextTile = nextTile.ParentTile;
        }
        currentlyMoving = true;
    }

    public void SetHeadingDirection(Vector3 targetDirection)
    {
        moveHeading = targetDirection - transform.position;
        moveHeading.Normalize();
    }

    #endregion

    #region Jump

    public void Jump(Vector3 jumptile)
    {
        if (jumpingToTarget)
        {
            JumpToTarget(jumptile);
        }
        else if (movingToEdge)
        {
            MoveToEdge();
        }
        else
        {
            PrepareToJump(jumptile);
        }
    }

    private void PrepareToJump(Vector3 target)
    {
        var targetHeight = target.y;
        target.y = transform.position.y;

        SetHeadingDirection(target);
        if (transform.position.y > targetHeight)
        {
            jumpingToTarget = false;
            movingToEdge = true;

            jumpPosition = transform.position + (target - transform.position) / jumpMoveSlowdown;
        }
        else
        {
            jumpingToTarget = true;
            movingToEdge = false;

            moveVelocity = (moveHeading * _unitMoveSpeed) / jumpMoveSlowdown;
            moveVelocity.y = targetHeight - transform.position.y;
        }
    }

    private void MoveToEdge()
    {
        if (Vector3.Distance(transform.position, jumpPosition) >= tileCenterFudge)
        {
            SetMoveVelocity();
        }
        else
        {
            movingToEdge = false;
            jumpingToTarget = true;

            moveVelocity /= jumpMoveSlowdown;
            moveVelocity.y = jumpWiggleRoom; // the little hop before you fall.
        }
    }

    private void JumpToTarget(Vector3 target)
    {
        moveVelocity *= Time.deltaTime;
        if (transform.position.y > target.y)
        {
            jumpingToTarget = false;
        }
    }

    #endregion
}
