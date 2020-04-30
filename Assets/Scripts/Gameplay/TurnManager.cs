using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static List<KeyValuePair<string, UnitMove>> AllUnits = new List<KeyValuePair<string, UnitMove>>();
    static Queue<string> UnitKey = new Queue<string>();
    static Queue<UnitMove> UnitTurnOrder = new Queue<UnitMove>();

    void Start()
    {
        
    }

    void Update()
    {
        if (UnitTurnOrder.Count < AllUnits.)
        {
            CreateTurnQueue();
        }
    }

    static void CreateTurnQueue()
    {
        UnitTurnOrder.;
        AllUnits.ForEach(c => c.Value.FullTurnCounter)
        List<UnitMove> teamList = AllUnits[UnitKey.Peek()];
        foreach (var unit in teamList)
        {
            UnitTurnOrder.Enqueue(unit);
        }

        // StartTurn();
    }

    static void StartTurn()
    {
        if (UnitTurnOrder.Count < AllUnits.Count)
        {
            UnitTurnOrder.Peek().BeginTurn();
        }
    }

    static void EndTurn()
    {
        UnitMove currentUnit = UnitTurnOrder.Dequeue();
        currentUnit.EndTurn();

        string unitFinished = UnitKey.Dequeue();
        UnitKey.Enqueue(unitFinished);
        CreateTurnQueue();
    }

    public static void AddUnitToGame(UnitMove unit)
    {
        if (!AllUnits.Key.Equals(unit.tag) && !AllUnits.Value.Equals(unit))
        {

        }
    }
}
