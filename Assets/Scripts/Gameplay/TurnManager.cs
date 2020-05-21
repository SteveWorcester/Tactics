using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    // ==========Do not change these variables=============
    [HideInInspector]
    public static List<Tuple<string, UnitCharacter>> AllUnits = new List<Tuple<string, UnitCharacter>>();
    [HideInInspector]
    public static Queue<Tuple<string, UnitCharacter>> UnitTurnOrder = new Queue<Tuple<string, UnitCharacter>>();
    private static bool _creatingTurnQueue = false;
    private static bool turnInProgress = false;
    public static CameraController MainCamera;
    //=====================================================

    void Start()
    {
        MainCamera = GameObject.FindObjectOfType<CameraController>();
    }

    void Update()
    {
        if (turnInProgress || _creatingTurnQueue)
        {
            return;
        }
        if (UnitTurnOrder.Count < AllUnits.Count)
        {
            CreateTurnQueue();
        }
        if (UnitTurnOrder.Count == AllUnits.Count)
        {
            StartTurn();
        }
    }

    static void CreateTurnQueue()
    {
        Debug.Log("Creating Turn Queue...");
        _creatingTurnQueue = true;

        UnitTurnOrder.Clear();
        AllUnits.Sort((unit1, unit2) => unit1.Item2._FullTurnCounter.CompareTo(unit2.Item2._FullTurnCounter));
        AllUnits.ForEach(unit => UnitTurnOrder.Enqueue(unit));

        _creatingTurnQueue = false;        
    }

    public static void StartTurn()
    {
        turnInProgress = true;
        var nextUnit = UnitTurnOrder.Peek();
        MainCamera.PanCameraToLocation(nextUnit.Item2.transform.position);
        Debug.Log($"TurnManager starting turn." +
            $"\nPlayer Tag: {nextUnit.Item1}");

        nextUnit.Item2.BeginTurn();
    }

    public static void EndTurn()
    {
        
        var currentUnit = UnitTurnOrder.Dequeue();
        Debug.Log($"Ending turn for \n{currentUnit.Item1}\n{currentUnit.Item2}");
        turnInProgress = false;        
    }

    public static void AddUnitToGame(string unitTag, UnitCharacter unitCharacter)
    {
        var unitToAdd = Tuple.Create(unitTag, unitCharacter);
        
        if (!AllUnits.Contains(unitToAdd))
        {
            AllUnits.Add(unitToAdd);
        }
    }
}
