using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Slider healthSlider;

    public void Start()
    {
        unitName = GetComponents<Text>().Where(t => t.text.Equals          ("Character1_Name")).First();
        currentHealth = GetComponents<Text>().Where(t => t.text.Equals     ("currentHealth")).First();
        maximumHealth = GetComponents<Text>().Where(t => t.text.Equals     ("maximumHealth")).First();
        speed = GetComponents<Text>().Where(t => t.text.Equals             ("speed")).First();
        damageBonus = GetComponents<Text>().Where(t => t.text.Equals       ("damageBonus")).First();
        damageResistance = GetComponents<Text>().Where(t => t.text.Equals  ("damageResistance")).First();
        jumpHeight = GetComponents<Text>().Where(t => t.text.Equals        ("jumpHeight")).First();
        moveDistance = GetComponents<Text>().Where(t => t.text.Equals      ("moveDistance")).First();
        attacksPerTurn = GetComponents<Text>().Where(t => t.text.Equals    ("attacksPerTurn")).First();
        movesPerTurn = GetComponents<Text>().Where(t => t.text.Equals      ("movesPerTurn")).First();
        currentTurnCounter = GetComponents<Text>().Where(t => t.text.Equals("currentTurnCounter")).First();
        moveCost = GetComponents<Text>().Where(t => t.text.Equals          ("moveCost")).First();
        startTurnCost = GetComponents<Text>().Where(t => t.text.Equals     ("startTurnCost")).First();
        healthSlider = GetComponent<Slider>();
    }

    public void UpdateCurrentCharacter(UnitCharacter currentUnit, bool updateWithCurrentCharacterInfo = true)
    {
        characterTakingTurn = currentUnit;
        if (updateWithCurrentCharacterInfo)
        {
            UpdateStatBlock();
        }
        else
        {
            throw new System.Exception("No information to update character info. This exception is to be replaced when implemented.");
        }
    }

    public void UpdateStatBlock()
    {
        unitName.text = characterTakingTurn._UnitName;
        speed.text = $"Speed: {characterTakingTurn._Speed.ToString()}";
        damageBonus.text = $"Damage Bonus: {characterTakingTurn._DamageBonus.ToString()}";
        damageResistance.text = $"Damage Resistance: {characterTakingTurn._DamageResistance.ToString()}";
        jumpHeight.text = $"Jump Height: {characterTakingTurn._JumpHeight.ToString()}";
        moveDistance.text = $"Move Distance: {characterTakingTurn._MoveDistance.ToString()}";
        attacksPerTurn.text = $"Attacks Per Turn: {characterTakingTurn._AvailableAttacksPerTurn.ToString()}";
        movesPerTurn.text = $"Moves Per Turn: {characterTakingTurn._AvailableMovesPerTurn.ToString()}";
        currentTurnCounter.text = $"Current Turn Counter: {characterTakingTurn._FullTurnCounter.ToString()}";
        moveCost.text = $"Move Cost: {characterTakingTurn._TurnCostMove.ToString()}";
        startTurnCost.text = $"Start Turn Cost: {characterTakingTurn._TurnCostStart.ToString()}";
        currentHealth.text = $"Current Health: {characterTakingTurn._CurrentHealth.ToString()}";
        maximumHealth.text = $"Max Health: {characterTakingTurn._MaximumHealth.ToString()}";
        healthSlider.maxValue = characterTakingTurn._MaximumHealth;
        healthSlider.minValue = 0;
        healthSlider.value = characterTakingTurn._CurrentHealth;
    }
}
