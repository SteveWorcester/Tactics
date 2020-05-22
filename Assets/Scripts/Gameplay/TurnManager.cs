using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // Edit these
    public float turnSpeedSensitivityMultiplier = 1f;

    // ==========Do not change these variables=============
    [HideInInspector]
    public static List<Tuple<string, UnitCharacter>> AllUnits = new List<Tuple<string, UnitCharacter>>();
    [HideInInspector]
    public static Queue<Tuple<string, UnitCharacter>> UnitTurnOrder = new Queue<Tuple<string, UnitCharacter>>();
    private static bool _creatingTurnQueue = false;
    private static bool turnInProgress = false;
    public static CameraController MainCamera;
    private bool RunningTurnCountdown = false;
    //=====================================================

    void Start()
    {
        MainCamera = GameObject.FindObjectOfType<CameraController>();
    }

    void Update()
    {
        if (turnInProgress || _creatingTurnQueue || RunningTurnCountdown)
        {
            return;
        }
        if (UnitTurnOrder.Count < AllUnits.Count)
        {            
            CreateTurnQueue();
            TurnCountdown();
        }
        if (UnitTurnOrder.Count == AllUnits.Count)
        {
            StartTurn();
        }
    }

    private void TurnCountdown()
    {
        RunningTurnCountdown = true;
        while (AllUnits.First().Item2._FullTurnCounter > 0)
        {
            foreach (var unit in AllUnits)
            {
                var countdownUnit = unit.Item2._Speed * turnSpeedSensitivityMultiplier;
                unit.Item2._FullTurnCounter -= countdownUnit;
                if (unit.Item2._FullTurnCounter < 0)
                {
                    unit.Item2._FullTurnCounter = 0;
                }
            }
        }
        RunningTurnCountdown = false;
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
