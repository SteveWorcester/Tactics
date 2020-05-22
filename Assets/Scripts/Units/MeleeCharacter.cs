using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCharacter : UnitCharacter
{
    public float StartingHealth = 10;
    public float Speed = 10; // higher = faster turn counter
    public float DamageBonus = 0;
    public float DamageResistance = 0; // flat negative to all damage.
    public float JumpHeight = 2.0f;
    public int MoveDistance = 5;
    
    public float StartingOfGameTurnCounter = 10; // adds *once* at the beginning of the game
    public float TurnCostToMove = 20f;
    public float TurnCostToStartTurn = 10f;

    void Start()
    {
        _TurnCostStart = TurnCostToStartTurn;
        _TurnCostMove = TurnCostToMove;
        _CurrentHealth = StartingHealth;
        _Speed= Speed;
        _DamageResistance= DamageResistance;
        _FullTurnCounter= StartingOfGameTurnCounter;
        _DamageBonus = DamageBonus;        
        Init();
    }

    void Update()
    {
        if (!_CurrentlyTakingTurn)
        {
            return;
        }
        if (!unitMove.currentlyMoving && !_HasMovedThisTurn)
        {
            unitMove.SetAdjacencyList(_JumpHeight);
            unitMove.SetSelectableTiles(JumpHeight, MoveDistance);
            unitMove.CheckMouseToMove();
        }
        if (unitMove.currentlyMoving)
        {
            unitMove.Move();
        }
        if (_HasMovedThisTurn)  // and has attacked this turn....
        {
            Debug.Log($"Ending turn for {this.tag} :: {this}.");
            EndTurn();
        }
    }


}
