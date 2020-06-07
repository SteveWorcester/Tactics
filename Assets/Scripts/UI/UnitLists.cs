using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class UnitLists : MonoBehaviour
{
    public List<UnitCharacter> TurnOrderList = new List<UnitCharacter>();
    public List<UnitCharacter> AllDeadUnits = new List<UnitCharacter>();

    public List<GameObject> sidebarSections;
    public List<Text> sidebarUnitNames; 
    public List<Image> sidebarUnitPortraits;
    public List<Slider> sidebarUnitHealths;

    private void Start()
    {
        sidebarSections = GameObject.FindGameObjectsWithTag("SidebarUnitSection").ToList();
        UpdateSidebarUi();
    }

    public void UpdateAllLists(List<Tuple<string, UnitCharacter>> allUnitsInGame, bool updateSidebar = true)
    {
        UpdateTurnOrderList(allUnitsInGame);
        UpdateDeadUnitsList(allUnitsInGame);

        if (updateSidebar)
        {
            UpdateSidebarUi();
        }        
    }

    private void UpdateSidebarUi()
    {
        sidebarUnitNames.Clear();
        sidebarUnitHealths.Clear();
        sidebarUnitPortraits.Clear();

        sidebarSections.ForEach(s => sidebarUnitNames.Add(s.GetComponentInChildren<Text>()));
        sidebarSections.ForEach(s => sidebarUnitPortraits.Add(s.GetComponentInChildren<Image>()));
        sidebarSections.ForEach(s => sidebarUnitHealths.Add(s.GetComponentInChildren<Slider>()));
        
        //foreach (var unitPlacement in sidebarSections)
        //{
        //    sidebarUnitNames.Add(unitPlacement.GetComponentInChildren<Text>());
        //    sidebarUnitPortraits.Add(unitPlacement.GetComponentInChildren<Image>());
        //    sidebarUnitHealths.Add(unitPlacement.GetComponentInChildren<Slider>());
        //}
        for (int i = 0; i < TurnOrderList.Count ; i++)
        {            
            sidebarUnitNames.ElementAt(i).text = TurnOrderList[i]._UnitName;
            sidebarUnitPortraits.ElementAt(i).sprite = TurnOrderList[i].UnitPortraitAlive.sprite;
            sidebarUnitHealths.ElementAt(i).maxValue = TurnOrderList[i]._MaximumHealth;
            sidebarUnitHealths.ElementAt(i).minValue = 0;
            sidebarUnitHealths.ElementAt(i).value = TurnOrderList[i]._CurrentHealth;
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
        foreach (var deadUnit in allUnitsInGame)
        {
            if (deadUnit.Item2._IsDead && !AllDeadUnits.Contains(deadUnit.Item2))
            {
                AllDeadUnits.Add(deadUnit.Item2);
            }
        }
    }


}
