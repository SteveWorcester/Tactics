﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtons : MonoBehaviour
{
    public UnitCharacter currentUnit;
    public MeleeAttack abilitySelection;

    public TurnManager turnManager;

    public void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }

    public void Update()
    {
        currentUnit = turnManager._CurrentlyActiveUnit;
    }

    public void InitiateMove()
    {
        if (currentUnit._MovesLeftThisTurn > 0)
        {
            currentUnit.unitMove.StartMovePhase();
        }        
    }

    public void InitiateAttack()
    {
        if (currentUnit._AttacksLeftThisTurn > 0)
        {
            currentUnit.unitAttack.StartAttackPhase();
            abilitySelection = currentUnit.GetComponent<MeleeAttack>();
            abilitySelection.BasicAttack();
        }        
    }

    public void InitiateAbility()
    {
        //NOP: abilities NYI
    }

    public void InitiatePass()
    {
        currentUnit.EndTurn();
    }
}