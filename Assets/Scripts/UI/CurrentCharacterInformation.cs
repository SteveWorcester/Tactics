using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CurrentCharacterInformation : MonoBehaviour
{
    public UnitCharacter characterTakingTurn;
    public Text unitName;
    public Text currentHealth;
    public Text maximumHealth;
    public Text speed;
    public Text damageBonus;
    public Text damageResistance;
    public Text jumpHeight;
    public Text moveDistance;
    public Text attacksPerTurn;
    public Text movesPerTurn;
    public Text currentTurnCounter;
    public Text moveCost;
    public Text startTurnCost;     

    public void UpdateCurrentCharacter(UnitCharacter currentUnit, bool updateWithCurrentCharacterInfo = true)
    {
        characterTakingTurn = currentUnit;
        if (updateWithCurrentCharacterInfo)
        {
            UpdateStatBlock();
        }        
    }

    public void UpdateStatBlock()
    {
        unitName.text = characterTakingTurn._UnitName;
        speed.text = characterTakingTurn._Speed.ToString();
        damageBonus.text = characterTakingTurn._DamageBonus.ToString();
        damageResistance.text = characterTakingTurn._DamageResistance.ToString();
        jumpHeight.text = characterTakingTurn._JumpHeight.ToString();
        moveDistance.text = characterTakingTurn._MoveDistance.ToString();
        attacksPerTurn.text = characterTakingTurn._AvailableAttacksPerTurn.ToString();
        movesPerTurn.text = characterTakingTurn._AvailableMovesPerTurn.ToString();
        currentTurnCounter.text = characterTakingTurn._FullTurnCounter.ToString();
        moveCost.text = characterTakingTurn._TurnCostMove.ToString();
        startTurnCost.text = characterTakingTurn._TurnCostStart.ToString();
        currentHealth.text = characterTakingTurn._CurrentHealth.ToString();
        maximumHealth.text = characterTakingTurn._MaximumHealth.ToString();
    }
}
