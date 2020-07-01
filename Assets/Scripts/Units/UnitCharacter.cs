using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitCharacter : MonoBehaviour
{
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
    public bool alreadyDied = false;
    [HideInInspector]
    public bool _InMovePhase = false;
    [HideInInspector]
    public bool _InAttackPhase = false;
    [HideInInspector]
    public TerrainGeneric _currentTile;

    [HideInInspector]
    public UnitMove unitMove;
    [HideInInspector]
    public UnitAttack unitAttack;
    [HideInInspector]
    public TurnManager turnManager;
    [HideInInspector]
    public CurrentCharacterInformation _Ui;


    public void Init()
    {
        unitMove = gameObject.GetComponent<UnitMove>();
        unitAttack = gameObject.GetComponent<UnitAttack>();
        turnManager = FindObjectOfType<TurnManager>();
        _Ui = FindObjectOfType<CurrentCharacterInformation>();
        TurnManager.AddUnitToGame(gameObject.tag, this);
    }

    public void BeginTurn()
    {
        if (_IsDead)
        {
            _InMovePhase = false;
            _InAttackPhase = false;
            _AttacksLeftThisTurn = 0;
            _MovesLeftThisTurn = 0;
            _CurrentlyTakingTurn = true;
            turnManager._CurrentlyActiveUnit = this;
            _FullTurnCounter += 1000;
            turnManager.UpdateSidebarUi();
        }
        else
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
            _Ui.UpdateCurrentCharacter(this);
            turnManager.UpdateSidebarUi();
        }
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
            var rotate90Degrees = Quaternion.Euler(transform.rotation.x - 90, transform.rotation.y, transform.rotation.z).eulerAngles;
            gameObject.transform.Rotate(rotate90Degrees, Space.Self);
            //gameObject.transform.position -= new Vector3(0, 0, Math.Abs(transform.position.y - unitMove.halfUnitHeight + .1f));
            _IsDead = true;
            _FullTurnCounter += 1000;
            alreadyDied = true;
        }
    }
}
