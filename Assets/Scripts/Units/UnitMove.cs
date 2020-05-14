using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{

    //==========Do not change these variables=============
    [HideInInspector]
    public float halfUnitHeight = 0;
    protected float _unitMoveSpeed = 2;

    protected List<TerrainGeneric> _selectableTiles = new List<TerrainGeneric>();
    protected Stack<TerrainGeneric> _movePath = new Stack<TerrainGeneric>();
    protected GameObject[] allTiles;
    protected TerrainGeneric _currentTile;

    [HideInInspector]
    public bool currentlyMoving = false;
    protected Vector3 moveVelocity = new Vector3();
    protected Vector3 moveHeading = new Vector3();

    [HideInInspector]
    public float FullTurnCounter = 1;
    [HideInInspector]
    public bool CurrentlyTakingTurn = false;

//=====================================================

public void Init()
    {
        allTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
        halfUnitHeight = GetComponent<Collider>().bounds.extents.y;
        TurnManager.AddUnitToGame(gameObject.tag, this);
    }
    
    public void BeginTurn()
    {
        FullTurnCounter += 1;
        CurrentlyTakingTurn = true;
    }

    public void EndTurn()
    {
        CurrentlyTakingTurn = false;
        TurnManager.EndTurn();
    }

    public void Move()
    {     
        if (_movePath.Count > 0)
        {
            TerrainGeneric nextTile = _movePath.Peek();         
            Vector3 moveTarget = nextTile.transform.position;
            moveTarget.y += halfUnitHeight + nextTile.GetComponent<Collider>().bounds.extents.y;
            if (Vector3.Distance(transform.position, moveTarget) >= .005f)
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
            ClearSelectableTiles();
            currentlyMoving = false;

            EndTurn();
        }
    }
    

    #region Tile-specific

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
            checkTile.FindAdjacentTiles(jumpHeight, checkTile.IsPathable);
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

    public void ClearSelectableTiles()
    {
        if (_currentTile != null)
        {
            _currentTile.UnitLocation = false;
            _currentTile = null;
            
        }
        foreach (TerrainGeneric tile in _selectableTiles)
        {
            tile.ResetTile();
        }
        _selectableTiles.Clear();
    }

    #endregion

    #region Move-specific

    private void SetMoveVelocity()
    {
        moveVelocity = moveHeading * _unitMoveSpeed;
    }

    protected void CheckMouseToMove()
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

    private void SetHeadingDirection(Vector3 targetDirection)
    {
        moveHeading = targetDirection - transform.position;
        moveHeading.Normalize();
    }

    #endregion
}
