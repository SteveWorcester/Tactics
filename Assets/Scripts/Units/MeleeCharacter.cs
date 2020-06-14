using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeCharacter : UnitCharacter
{
    [Header("Hotkeys")]
    public KeyCode HotkeyMove = KeyCode.M;
    public KeyCode HotkeyAttack = KeyCode.A;
    public KeyCode HotkeyEndTurn = KeyCode.E;

    [Header("Character Stats")]
    public string UnitName = "Default Name - Change Me";
    public float StartingHealth = 10;
    public float Speed = 10; // higher = faster turn counter
    public float DamageBonus = 0;
    public float DamageResistance = 0; // flat negative to all damage.
    public float JumpHeight = 2.0f;
    public int MoveDistance = 5;
    public int AttacksPerTurn = 1;
    public int MovesPerTurn = 1;
    public float StartingOfGameTurnCounter = 10; // adds *once* at the beginning of the game
    public float TurnCostToMove = 20f;
    public float TurnCostToStartTurn = 10f;

    [HideInInspector]
    public bool InAttackPhase = false;
    [HideInInspector]
    public bool InMovePhase = false;

    [HideInInspector]
    public bool HasAttacked = false;
    [HideInInspector]
    public bool HasMoved = false;

    void Start()
    {
        _UnitName = UnitName;
        _AvailableAttacksPerTurn = AttacksPerTurn;
        _AvailableMovesPerTurn = MovesPerTurn;
        _TurnCostStart = TurnCostToStartTurn;
        _TurnCostMove = TurnCostToMove;
        _CurrentHealth = StartingHealth;
        _Speed= Speed;
        _DamageResistance= DamageResistance;
        _FullTurnCounter= StartingOfGameTurnCounter;
        _DamageBonus = DamageBonus;
        _MoveDistance = MoveDistance;
        _MaximumHealth = StartingHealth;
        Init();
    }

    void Update()
    {
        if (_CurrentHealth <= 0)
        {
            _CurrentHealth = 0;
            _IsDead = true;
        }
        if (_IsDead)
        {
            ActivateDeath();
        }
        if (!_CurrentlyTakingTurn)
        {
            return;
        }
        if (_InMovePhase || _InAttackPhase)
        {
            return;
        }
        if (Input.GetKeyUp(HotkeyMove) && _MovesLeftThisTurn > 0)
        {
            
            unitMove.StartMovePhase();
        }
        if (Input.GetKeyUp(HotkeyAttack) && _AttacksLeftThisTurn > 0)
        {
            unitAttack.StartAttackPhase();
        }
        if (Input.GetKeyUp(HotkeyEndTurn))
        {
            EndTurn();
        }
    }
}
