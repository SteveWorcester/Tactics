using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    // ===========Do not modify these===========
    protected int _AvailableAttacks;
    protected float abilityDamage = 0f;
    protected float abilityTurnCost = 0f;
    protected float abilityMinRange = 0f;
    protected float abilityMaxRange = 0f;
    protected float abilityHeightRestriction = 0f;

    protected bool _IsCurrentlyAttacking = false;
    protected bool _AttackSelected = false;
    protected bool _HasAttacked = false;

    protected UnitCharacter unitCharacter;
    protected UnitMove unitMove;
    protected TerrainGeneric attackLocation;

    protected List<TerrainGeneric> _AttackableTiles = new List<TerrainGeneric>();
    protected GameObject[] _AllTiles;
    public TerrainGeneric _currentTile;

    // ===========================================

    protected void Init()
    {
        unitCharacter = gameObject.GetComponent<UnitCharacter>();
        unitMove = gameObject.GetComponent<UnitMove>();
        _AllTiles = GameObject.FindGameObjectsWithTag("Terrain Tile");
    }

    public void StartAttackPhase()
    {
        Debug.Log("UnitAttack starting attack phase");
        _HasAttacked = false;
        _AttackSelected = false;
        unitCharacter._InAttackPhase = true;        
    }

    public void CheckMouseToAttack()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray mouseCheck = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit tileCheck;
            if (Physics.Raycast(mouseCheck, out tileCheck) && tileCheck.collider.tag.Equals("Terrain Tile"))
            {
                TerrainGeneric clickedTile = tileCheck.collider.GetComponent<TerrainGeneric>();
                if (clickedTile.AttackableTile)
                {
                    AttackTile(clickedTile);
                }
            }
        }
    }

    public void AttackTile(TerrainGeneric tile)
    {
        attackLocation = tile;
        tile.TargetLocation = true;
        _IsCurrentlyAttacking = true;
    }

    public void UseAbility()
    {
        var enemyUnit = GetUnitOnTile(attackLocation);
        _AttackSelected = false;
        Vector3 attackDirection = new Vector3(attackLocation.transform.position.x, gameObject.transform.position.y, attackLocation.transform.position.z);
        
        if (Vector3.Distance(transform.position, enemyUnit.transform.position) >= abilityMinRange && Vector3.Distance(transform.position, enemyUnit.transform.position) <= abilityMaxRange)
        {
            Debug.Log($"");
            unitMove.SetHeadingDirection(attackDirection);
            transform.forward = unitMove.moveHeading;
            // ATTACK ANIMATION HERE
            DealDamage(enemyUnit);
        }

        ClearAttackableTiles();
       
        _IsCurrentlyAttacking = false;
        unitCharacter._AttacksLeftThisTurn--;
        Debug.Log($"Enemy health is now {enemyUnit._CurrentHealth}");

        _HasAttacked = true;
    }

    public void EndAttackPhase()
    {
        Debug.Log($"Ending attack phase for {unitCharacter}. changing to character control.");
        ClearAttackableTiles();
        unitCharacter._AttacksLeftThisTurn--;
        _AttackSelected = false;
        SetCurrentTile();
        unitCharacter._InAttackPhase = false;
    }

    public UnitCharacter GetUnitOnTile(TerrainGeneric tile)
    {
        UnitCharacter unitOnTile;

        RaycastHit unitCheck;
        Physics.Raycast(tile.transform.position, Vector3.up, out unitCheck, 4.0f);
        unitCheck.collider.TryGetComponent<UnitCharacter>(out unitOnTile);
        return unitOnTile;
    }

    public void DealDamage(TerrainGeneric tileWhereEnemyIs)
    {
        RaycastHit unitFinder;
        Physics.Raycast(tileWhereEnemyIs.transform.position, Vector3.up, out unitFinder, 4.0f);
        DealDamage(unitFinder.collider.GetComponent<UnitCharacter>());
    }

    public void DealDamage(UnitCharacter enemy)
    {
        var actualDamageDealt = abilityDamage - enemy._DamageResistance;
        if (actualDamageDealt > 0)
        {
            enemy._CurrentHealth -= actualDamageDealt;
        }        
    }

    #region Tile finding/marking

    public void ClearAttackableTiles()
    {
        if (_currentTile != null)
        {
            _currentTile.UnitLocation = false;
            _currentTile = null;

        }
        foreach (TerrainGeneric tile in _AttackableTiles)
        {
            tile.ResetTile();
        }
        _AttackableTiles.Clear();
    }

    public void SetAttackAdjacencyList(float abilityHeightRestriction)
    {
        foreach (var tile in _AllTiles)
        {
            TerrainGeneric checkTile = tile.GetComponent<TerrainGeneric>();
            checkTile.FindAdjacentAttackableTiles(abilityHeightRestriction);
        }
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

    public void SetCurrentTile()
    {
        _currentTile = GetTargetTile(gameObject);
        _currentTile.UnitLocation = true;
    }

    public void SetAttackableTiles()
    {
        Debug.Log("SPAM OKAY: setting attackable tiles");
        SetAttackAdjacencyList(abilityHeightRestriction);
        SetCurrentTile();

        Queue<TerrainGeneric> process = new Queue<TerrainGeneric>();
        process.Enqueue(_currentTile);
        _currentTile.VisitedTile = true;

        while (process.Count > 0)
        {
            TerrainGeneric t = process.Dequeue();
            _AttackableTiles.Add(t);
            t.AttackableTile = true;
            if (t.Distance < abilityMaxRange)
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

    #endregion
}
