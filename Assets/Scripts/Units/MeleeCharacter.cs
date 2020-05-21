using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCharacter : UnitCharacter
{
    public float StartingHealth = 10;
    public float Speed = 10; // higher = faster turn counter
    public float DamageBonus = 0;
    public float DamageResistance = 0; // flat negative to all damage.
    public float StartingTurnCounterAdd = 10; // adds *once* at the beginning of the game
    public float JumpHeight = 2.0f;
    public int MoveDistance = 5;

    void Start()
    {
        _CurrentHealth = StartingHealth;
        _Speed= Speed;
        _DamageResistance= DamageResistance;
        _FullTurnCounter= StartingTurnCounterAdd;
        _DamageBonus = DamageBonus;
        Init();
    }

    void Update()
    {
        if (!_CurrentlyTakingTurn)
        {
            return;
        }
        Debug.Log($"SPAM OK: Currently taking turn: {_CurrentlyTakingTurn} {this.tag} :: {this}");
        if (!unitMove.currentlyMoving && !_HasMovedThisTurn)
        {
            Debug.Log($"SPAM OK: checking mouse to move...");
            unitMove.SetAdjacencyList(_JumpHeight);
            unitMove.SetSelectableTiles(JumpHeight, MoveDistance);
            unitMove.CheckMouseToMove();
        }
        if (unitMove.currentlyMoving)
        {
            Debug.Log($"SHORT SPAM OK: Moving....");
            unitMove.Move();
        }
        if (_HasMovedThisTurn)  // and has attacked this turn....
        {
            Debug.Log($"Ending turn for {this.tag} :: {this}.");
            EndTurn();
        }
    }


}
