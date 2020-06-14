using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    // ===========Do not modify these===========
    [HideInInspector]
    protected int _AvailableAttacks;
    [HideInInspector]
    protected float abilityDamage = 0f;
    [HideInInspector]
    protected float abilityTurnCost = 0f;
    [HideInInspector]
    protected float abilityMinRange = 0f;
    [HideInInspector]
    protected float abilityMaxRange = 0f;
    [HideInInspector]
    protected float abilityHeightRestriction = 0f;

    [HideInInspector]
    protected bool _IsCurrentlyAttacking = false;
    [HideInInspector]
    protected bool _AttackSelected = false;
    [HideInInspector]
    public bool _HasAttacked = false;

    [HideInInspector]
    protected UnitCharacter unitCharacter;
    [HideInInspector]
    protected UnitMove unitMove;
    [HideInInspector]
    protected TerrainGeneric attackLocation;
    [HideInInspector]
    protected bool isAttackLocationOccupied = false;

    [HideInInspector]
    protected List<TerrainGeneric> _AttackableTiles = new List<TerrainGeneric>();
    [HideInInspector]
    protected GameObject[] _AllTiles;

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
        if (isAttackLocationOccupied)
        {
            if (Vector3.Distance(transform.position, enemyUnit.transform.position) >= abilityMinRange && Vector3.Distance(transform.position, enemyUnit.transform.position) <= abilityMaxRange)
            {
                Debug.Log($"");
                unitMove.SetHeadingDirection(attackDirection);
                transform.forward = unitMove.moveHeading;
                // ATTACK ANIMATION HERE
                DealDamage(enemyUnit);
            }
            
            Debug.Log($"Enemy health is now {enemyUnit._CurrentHealth}");            
        }
        unitCharacter._AttacksLeftThisTurn--;
        _IsCurrentlyAttacking = false;
        _HasAttacked = true;
        ClearAttackableTiles();
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
        isAttackLocationOccupied = Physics.Raycast(tile.transform.position, Vector3.up, out unitCheck, 4.0f);
        if (isAttackLocationOccupied)
        {
            unitCheck.collider.TryGetComponent<UnitCharacter>(out unitOnTile);            
        }
        else
        {
            unitOnTile = null;
        }
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
        if (enemy._CurrentHealth <= 0)
        {
            enemy._CurrentHealth = 0;
        }
    }

    #region Tile finding/marking

    public void ClearAttackableTiles()
    {
        if (unitCharacter._currentTile != null)
        {
            unitCharacter._currentTile.UnitLocation = false;
            unitCharacter._currentTile = null;

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
        unitCharacter._currentTile = GetTargetTile(gameObject);
        unitCharacter._currentTile.UnitLocation = true;
    }

    public void SetAttackableTiles()
    {
        Debug.Log("SPAM OKAY: setting attackable tiles");
        SetAttackAdjacencyList(abilityHeightRestriction);
        SetCurrentTile();

        Queue<TerrainGeneric> process = new Queue<TerrainGeneric>();
        process.Enqueue(unitCharacter._currentTile);
        unitCharacter._currentTile.VisitedTile = true;

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
