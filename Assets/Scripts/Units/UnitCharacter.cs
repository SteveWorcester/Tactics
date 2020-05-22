using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCharacter : MonoBehaviour
{
    [HideInInspector]
    protected UnitMove unitMove;

    [HideInInspector]
    public float _CurrentHealth;
    [HideInInspector]
    public float _Speed;
    [HideInInspector]
    public float _DamageResistance;
    [HideInInspector]
    public float _DamageBonus;
    [HideInInspector]
    public float _FullTurnCounter;
    [HideInInspector]
    public bool _CurrentlyTakingTurn = false;
    [HideInInspector]
    public bool _HasMovedThisTurn = false;
    [HideInInspector]
    public float _JumpHeight;
    [HideInInspector]
    public bool _HasAttackedThisTurn = true; // temporarily true until attacks have been added.
    protected float _TurnCostStart = 10f;
    protected float _TurnCostMove = 20f;
    protected float _TurnCostAttack;

    public void Init()
    {
        unitMove = gameObject.GetComponent<UnitMove>();
        // UnitAttack attack = GetComponent<UnitAttack>(); this will be added to AddUnitToGame
        TurnManager.AddUnitToGame(gameObject.tag, this);
    }

    public void BeginTurn()
    {
        Debug.Log($"UnitCharacter starting turn for {this.tag}");
        _FullTurnCounter += _TurnCostStart;        
        _HasMovedThisTurn = false;
        // _HasAttackedThisTurn = false;
        _CurrentlyTakingTurn = true;
    }

    public void EndTurn()
    {
        if (_HasMovedThisTurn)
        {
            _FullTurnCounter += _TurnCostMove;
        }
        if (_HasAttackedThisTurn)
        {
            _FullTurnCounter += _TurnCostAttack;
        }
        _CurrentlyTakingTurn = false;
        Debug.Log($"{this.tag} unit ending turn with turn counter amount: {_FullTurnCounter}");
        TurnManager.EndTurn();
    }
}
