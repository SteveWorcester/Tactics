using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : UnitAttack
{
    // Can edit these
    public const string Ability1Name = "Basic Attack";
    public const string Ability2Name = "TEMP";

    [Header(Ability1Name)]
    public KeyCode HotKeyAbility1 = KeyCode.G;    
    public float Ability1HeightRestriction = 3f;
    public float Ability1MaxRange = 3f;
    public float Ability1MinRange = 0f;
    public float Ability1Damage = 3f;
    public float Ability1TurnCost = 10f;

    [Header(Ability2Name)]
    public KeyCode HotKeyAbility2 = KeyCode.H;    
    public float Ability2HeightRestriction = 10f;
    public float Ability2MaxRange = 5f;
    public float Ability2MinRange = 3f;
    public float Ability2Damage = 25f;
    public float Ability2TurnCost = 50f;    

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!unitCharacter._InAttackPhase)
        {
            return;
        }
        if (_IsCurrentlyAttacking)
        {
            UseAbility();
        }
        if (_AttackSelected && !_IsCurrentlyAttacking)
        {
            unitMove.SetAdjacencyList(abilityHeightRestriction);
            SetAttackableTiles();
            CheckMouseToAttack();
        }
        if (Input.GetKeyUp(HotKeyAbility1) && !_IsCurrentlyAttacking)
        {
            BasicAttack();
        }
        if (Input.GetKeyUp(HotKeyAbility2) && !_IsCurrentlyAttacking)
        {
            TempAttack();
        }
        if (_HasAttacked && !_IsCurrentlyAttacking)
        {
            EndAttackPhase();
        }

    }

    public void BasicAttack()
    {
        Debug.Log($"{Ability1Name} selected\nMin/Range: {Ability1MinRange}/{Ability1MaxRange}\nDamage: {Ability1Damage}\nTurn Cost: {Ability1TurnCost}\nHeight Restriction {Ability1HeightRestriction}");
        abilityHeightRestriction = Ability1HeightRestriction;
        abilityMaxRange     =      Ability1MaxRange;
        abilityMinRange     =      Ability1MinRange;
        abilityDamage       =      Ability1Damage;
        abilityTurnCost     =      Ability1TurnCost;
        _AttackSelected     =      true;
    }

    public void TempAttack()
    {
        Debug.Log($"{Ability2Name} selected\nMin/Range: {Ability2MinRange}/{Ability2MaxRange}\nDamage: {Ability2Damage}\nTurn Cost: {Ability2TurnCost}\nHeight Restriction {Ability2HeightRestriction}");
        abilityHeightRestriction = Ability2HeightRestriction;
        abilityMaxRange     =      Ability2MaxRange;
        abilityMinRange     =      Ability2MinRange;
        abilityDamage       =      Ability2Damage;
        abilityTurnCost     =      Ability2TurnCost;
        _AttackSelected     =      true;
    }
}
