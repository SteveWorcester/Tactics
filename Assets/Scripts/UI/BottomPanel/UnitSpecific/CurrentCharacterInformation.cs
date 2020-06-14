using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI; 

public class CurrentCharacterInformation : MonoBehaviour
{
    [HideInInspector]
    public string stat1DescriptionText = "Bonus Damage";
    [HideInInspector]
    public string stat2DescriptionText = "Damage Resistance";
    [HideInInspector]
    public string stat3DescriptionText = "Move";
    [HideInInspector]
    public string stat4DescriptionText = "Speed";

    [HideInInspector]
    public Text stat1Description;
    [HideInInspector]
    public Text stat2Description;
    [HideInInspector]
    public Text stat3Description;
    [HideInInspector]
    public Text stat4Description;

    [HideInInspector]
    public Text stat1Value;
    [HideInInspector]
    public Text stat2Value;
    [HideInInspector]
    public Text stat3Value;
    [HideInInspector]
    public Text stat4Value;

    [HideInInspector]
    public UnitCharacter characterTakingTurn;
    [HideInInspector]
    public Text activeName;
    [HideInInspector]
    public Image activePortrait;
    [HideInInspector]
    public Slider healthSlider;

    [HideInInspector]
    public UnitStats unitStats;
    [HideInInspector]
    public UnitPortrait unitPortrait;
    [HideInInspector]
    public UnitName unitName;
    [HideInInspector]
    public UnitHealth unitHealth;
    [HideInInspector]
    public TurnManager turnManager;

    [HideInInspector]
    public event Action UpdateUI;

    public void Awake()
    {
        turnManager = FindObjectOfType<TurnManager>();
    }
    public void Start()
    {
        unitName = GetComponentInChildren<UnitName>();
        unitPortrait = GetComponentInChildren<UnitPortrait>();
        unitHealth = GetComponentInChildren<UnitHealth>();
        unitStats = GetComponentInChildren<UnitStats>();               

        activeName = unitName.GetComponent<Text>();
        healthSlider = unitHealth.GetComponent<Slider>();
        activePortrait = unitPortrait.GetComponent<Image>();

        stat1Description = unitStats.GetComponentsInChildren<Text>().FirstOrDefault(s => s.text.Equals(stat1DescriptionText));
        stat2Description = unitStats.GetComponentsInChildren<Text>().FirstOrDefault(s => s.text.Equals(stat2DescriptionText));
        stat3Description = unitStats.GetComponentsInChildren<Text>().FirstOrDefault(s => s.text.Equals(stat3DescriptionText));
        stat4Description = unitStats.GetComponentsInChildren<Text>().FirstOrDefault(s => s.text.Equals(stat4DescriptionText));

        stat1Value = stat1Description.GetComponentsInChildren<Text>().FirstOrDefault(s => s.text.Equals("100"));
        stat2Value = stat2Description.GetComponentsInChildren<Text>().FirstOrDefault(s => s.text.Equals("100"));
        stat3Value = stat3Description.GetComponentsInChildren<Text>().FirstOrDefault(s => s.text.Equals("100"));
        stat4Value = stat4Description.GetComponentsInChildren<Text>().FirstOrDefault(s => s.text.Equals("100"));

    }

    public void UpdateCurrentCharacter(UnitCharacter currentUnit)
    {
        characterTakingTurn = currentUnit;
        UpdateStatBlock();
    }

    public void UpdateStatBlock()
    {
        if (characterTakingTurn._IsDead)
        {
            activePortrait.sprite = characterTakingTurn.UnitPortraitDead.sprite;
        }
        else
        {
            activePortrait.sprite = characterTakingTurn.UnitPortraitAlive.sprite;
        }

        activeName.text = characterTakingTurn._UnitName;   
        healthSlider.maxValue = characterTakingTurn._MaximumHealth;
        healthSlider.minValue = 0;
        healthSlider.value = characterTakingTurn._CurrentHealth;

        stat1Description.text = stat1DescriptionText;
        stat1Value.text = characterTakingTurn._DamageBonus.ToString();

        stat2Description.text = stat2DescriptionText;
        stat2Value.text = characterTakingTurn._DamageResistance.ToString();

        stat3Description.text = stat3DescriptionText;
        stat3Value.text = characterTakingTurn._MoveDistance.ToString();

        stat4Description.text = stat4DescriptionText;
        stat4Value.text = characterTakingTurn._Speed.ToString();

        Debug.Log($"Updating UI to reflect {characterTakingTurn}");
        UpdateUI();
    }
}
