﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitCharacter : MonoBehaviour
{
    [HideInInspector]
    public UnitMove unitMove;
    [HideInInspector]
    public UnitAttack unitAttack;

    public Image UnitPortraitAlive;
    public Image UnitPortraitDead;

    [HideInInspector]
    public string _UnitName;
    [HideInInspector]
    public float _CurrentHealth;
    [HideInInspector]
    public float _MaximumHealth;
    [HideInInspector]
    public float _Speed;
    [HideInInspector]
    public float _JumpHeight;
    [HideInInspector]
    public int _MoveDistance;
    [HideInInspector]
    public float _DamageResistance;
    [HideInInspector]
    public float _DamageBonus;
    [HideInInspector]
    public float _FullTurnCounter;

    [HideInInspector]
    public float _TurnCostStart = 10f;
    [HideInInspector]
    public float _TurnCostMove = 20f;
    [HideInInspector]
    public float _TurnCostAbility;

    [HideInInspector]
    public int _AvailableAttacksPerTurn = 1;
    [HideInInspector]
    public int _AttacksLeftThisTurn;
    [HideInInspector]
    public int _AvailableMovesPerTurn = 1;
    [HideInInspector]
    public int _MovesLeftThisTurn;

    [HideInInspector]
    public bool _CurrentlyTakingTurn = false;
    [HideInInspector]
    public bool _IsDead = false;
    [HideInInspector]
    private bool alreadyDied = false;
    [HideInInspector]
    public bool _InMovePhase = false;
    [HideInInspector]
    public bool _InAttackPhase = false;
    [HideInInspector]
    public TerrainGeneric _currentTile;
    [HideInInspector]
    public TurnManager turnManager;

    public void Init()
    {
        unitMove = gameObject.GetComponent<UnitMove>();
        unitAttack = gameObject.GetComponent<UnitAttack>();
        turnManager = GameObject.FindObjectOfType<TurnManager>();
        TurnManager.AddUnitToGame(gameObject.tag, this);
    }

    public void BeginTurn()
    {
        Debug.Log($"UnitCharacter starting turn for {this.tag}");
        _FullTurnCounter += _TurnCostStart;
        _InMovePhase = false;
        _InAttackPhase = false;
        _AttacksLeftThisTurn = _AvailableAttacksPerTurn;
        _MovesLeftThisTurn = _AvailableMovesPerTurn;
        _CurrentlyTakingTurn = true;
        turnManager._CurrentlyActiveUnit = this;
        unitMove.SetCurrentTile();
    }

    public void EndTurn()
    {
        _CurrentlyTakingTurn = false;
        _currentTile.UnitLocation = false;

        Debug.Log($"{this.tag} unit ending turn with turn counter amount: {_FullTurnCounter}");
        TurnManager.EndTurn();
    }

    public void ActivateDeath()
    {
        if (!alreadyDied)
        {
            var rotate90Degrees = new Vector3(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);
            gameObject.transform.Rotate(rotate90Degrees, Space.Self);
            alreadyDied = true;
        }
    }
}
