using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    // ==========Do not change these variables=============
    [HideInInspector]
    public static List<KeyValuePair<string, UnitMove>> AllUnits = new List<KeyValuePair<string, UnitMove>>();
    [HideInInspector]
    public static Queue<KeyValuePair<string, UnitMove>> UnitTurnOrder = new Queue<KeyValuePair<string, UnitMove>>();
    private static bool _creatingTurnQueue = false;
    //=====================================================

    void Start()
    {
        
    }

    void Update()
    {
        if (UnitTurnOrder.Count < AllUnits.Count && !_creatingTurnQueue)
        {
            CreateTurnQueue();
        }
        else if (UnitTurnOrder.Count == AllUnits.Count && !_creatingTurnQueue)
        {
            StartTurn();
        }
    }

    static void CreateTurnQueue()
    {
        Debug.Log("Creating Turn Queue...");
        _creatingTurnQueue = true;

        UnitTurnOrder.Clear();
        AllUnits.Sort((KeyValuePair<string, UnitMove> unit1, KeyValuePair<string, UnitMove> unit2) => unit1.Value.FullTurnCounter.CompareTo(unit2.Value.FullTurnCounter));
        AllUnits.ForEach(u => UnitTurnOrder.Enqueue(u));

        _creatingTurnQueue = false;        
    }

    public static void StartTurn()
    {
        var nextUnit = UnitTurnOrder.Peek();
        Debug.Log($"Starting turn.\nPlayer Tag: {nextUnit.Key}\nUnit: {nextUnit.Value.ToString()}");
        nextUnit.Value.BeginTurn();
    }

    public static void EndTurn()
    {
        UnitMove currentUnit = UnitTurnOrder.Dequeue().Value;
        Debug.Log($"Ending turn for {currentUnit}");
    }

    public static void AddUnitToGame(string tag, UnitMove unitMove)
    {
        KeyValuePair<string, UnitMove> unitToAdd = new KeyValuePair<string, UnitMove>(tag, unitMove);
        if (!AllUnits.Contains(unitToAdd))
        {
            AllUnits.Add(unitToAdd);
        }
    }
}
