using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{

    //==========Do not change these variables=============
    [HideInInspector]
    public float halfUnitHeight = 0.0f;
    protected float _unitMoveSpeed = 2.0f;
    private float tileCenterFudge = .05f; // if you are this close to where you are supposed to be, then you "pop" to that location

    protected List<TerrainGeneric> _selectableTiles = new List<TerrainGeneric>();
    protected Stack<TerrainGeneric> _movePath = new Stack<TerrainGeneric>();
    protected GameObject[] allTiles;
    
    [HideInInspector]
    public bool currentlyMoving = false;
    protected Vector3 moveVelocity = new Vector3();
    [HideInInspector]
    public Vector3 moveHeading = new Vector3();
    [HideInInspector]
    public bool _hasMoved = false;
    private Quaternion originalRotation;

    [HideInInspector]
    public UnitCharacter unitCharacter;

    //=====================================================

    #region Basic Functionality    

    public void Init()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
        halfUnitHeight = gameObject.GetComponent<Collider>().bounds.extents.y;
        unitCharacter = gameObject.GetComponent<UnitCharacter>();
        originalRotation = transform.rotation;
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
            if (Vector3.Distance(transform.position, moveTarget) >= tileCenterFudge)
            {
                SetHeadingDirection(moveTarget);
                SetMoveVelocity();

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
            FixUnitRotation();
            ClearSelectableTiles();
            currentlyMoving = false;
            _hasMoved = true;
        }
    }

    #endregion

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

    private void FixUnitRotation()
    {       
        
        transform.rotation = originalRotation;
        //transform.position.Normalize();
    }

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

}
