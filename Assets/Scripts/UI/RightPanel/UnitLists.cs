using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class UnitLists : MonoBehaviour
{
    [HideInInspector]
    public List<Tuple<string, UnitCharacter>> allUnits;

    [HideInInspector]
    public List<UnitCharacter> TurnOrderList = new List<UnitCharacter>();
    [HideInInspector]
    public List<UnitCharacter> AllDeadUnits = new List<UnitCharacter>();

    [HideInInspector]
    public List<GameObject> sidebarSections;

    [HideInInspector]
    public List<Tuple<Text, Image, Slider>> sideBarDisplay = new List<Tuple<Text, Image, Slider>>();

    private void Start()
    {
        sidebarSections = GameObject.FindGameObjectsWithTag("SidebarUnitSection").ToList();
        foreach (var section in sidebarSections)
        {
            var name = section.GetComponentInChildren<Text>();
            var sprite = section.GetComponentsInChildren<Image>().First();
            var health = section.GetComponentInChildren<Slider>();
            sideBarDisplay.Add(new Tuple<Text, Image, Slider>(name, sprite, health));
        }
    }

    public void UpdateAllLists(List<Tuple<string, UnitCharacter>> allUnitsInGame)
    {
        allUnits = allUnitsInGame;
        UpdateTurnOrderList(allUnitsInGame);
        UpdateDeadUnitsList(allUnitsInGame);
        UpdateSidebarUi();       
    }

    private void UpdateSidebarUi()
    {
        var aliveDeadList = new List<UnitCharacter>();
        TurnOrderList.ForEach(u => aliveDeadList.Add(u));
        AllDeadUnits.ForEach(u => aliveDeadList.Add(u));
        for (int i = 0; i < aliveDeadList.Count; i++)
        {
            sideBarDisplay[i].Item1.text = aliveDeadList[i]._UnitName;
            sideBarDisplay[i].Item3.minValue = 0;
            sideBarDisplay[i].Item3.maxValue = aliveDeadList[i]._MaximumHealth;
            sideBarDisplay[i].Item3.value = aliveDeadList[i]._CurrentHealth;
            if (aliveDeadList[i]._IsDead)
            {
                sideBarDisplay[i].Item2.enabled = true;
                sideBarDisplay[i].Item2.sprite = aliveDeadList[i].UnitPortraitDead.sprite;
                
            }
            else if (!aliveDeadList[i]._IsDead)
            {
                sideBarDisplay[i].Item2.enabled = true;
                sideBarDisplay[i].Item2.sprite = aliveDeadList[i].UnitPortraitAlive.sprite;
            }
        }
    }

    private void UpdateTurnOrderList(List<Tuple<string, UnitCharacter>> allUnitsInGame)
    {
        TurnOrderList.Clear();

        for (int i = 0; i < allUnitsInGame.Count; i++)
        {
            if (!allUnitsInGame[i].Item2._IsDead)
            {
                TurnOrderList.Add(allUnitsInGame[i].Item2);
            }
        }
    }

    private void UpdateDeadUnitsList(List<Tuple<string, UnitCharacter>> allUnitsInGame)
    {
        AllDeadUnits.Clear();

        foreach (var deadUnit in allUnitsInGame)
        {
            if (deadUnit.Item2._IsDead && !AllDeadUnits.Contains(deadUnit.Item2))
            {
                AllDeadUnits.Add(deadUnit.Item2);
            }
        }
    }


}
