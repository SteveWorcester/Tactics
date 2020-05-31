using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class UnitLists : MonoBehaviour
{
    public List<UnitCharacter> TurnOrderList = new List<UnitCharacter>();
    public List<UnitCharacter> AllDeadUnits = new List<UnitCharacter>();

    public void UpdateAllLists(List<Tuple<string, UnitCharacter>> allUnitsInGame)
    {
        UpdateTurnOrderList(allUnitsInGame);
        UpdateDeadUnitsList(allUnitsInGame);
    }

    private void UpdateTurnOrderList(List<Tuple<string, UnitCharacter>> allUnitsInGame)
    {
        TurnOrderList.Clear();
        foreach (var aliveUnit in allUnitsInGame)
        {
            if (!aliveUnit.Item2._IsDead)
            {
                TurnOrderList.Add(aliveUnit.Item2);
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
