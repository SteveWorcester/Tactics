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
    [HideInInspector]
    public static int _TurnCounter = 1;
    [HideInInspector]
    public static int _LivingUnits = 0;
    //=====================================================

    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInParent<CameraController>();
        Init();
    }

    void Init()
    {
        MainCamera.ZoomOut();
        MainCamera.TiltDown();
    }

    void Update()
    {
        if (turnInProgress || _creatingTurnQueue || RunningTurnCountdown)
        {
            return;
        }
        else if (UnitTurnOrder.Count < AllUnits.Count)
        {
            CreateTurnQueue();
            TurnCountdown();
        }
    }
    private void LateUpdate()
    {
        if (UnitTurnOrder.Count == AllUnits.Count && !turnInProgress)
        {
            StartTurn();
        }
    }

    private void TurnCountdown()
    {
        Debug.Log($"TurnManager running turn countdown...");
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
        Debug.Log("TurnManager Creating Turn Queue...");
        _creatingTurnQueue = true;

        UnitTurnOrder.Clear();
        AllUnits.Sort((unit1, unit2) => unit1.Item2._FullTurnCounter.CompareTo(unit2.Item2._FullTurnCounter));

        _LivingUnits = 0;
        foreach (var unit in AllUnits)
        {
            if (!unit.Item2._IsDead)
            {
                _LivingUnits++;
                UnitTurnOrder.Enqueue(unit);
            }
        }

        _creatingTurnQueue = false;        
    }

    public static void StartTurn()
    {
        turnInProgress = true;
        var nextUnit = UnitTurnOrder.Peek();        
        Debug.Log($"TurnManager starting turn." +
            $"\nPlayer Tag: {nextUnit.Item1}");
        nextUnit.Item2.BeginTurn();
        Debug.Log($"auto-panning camera to {nextUnit.Item2}");
        MainCamera.PanCameraToLocation(nextUnit.Item2.transform.position);
    }

    public static void EndTurn()
    {        
        var currentUnit = UnitTurnOrder.Dequeue();
        Debug.Log($"TurnManager Ending turn for \n{currentUnit.Item1}\n{currentUnit.Item2}");
        _TurnCounter++;
        turnInProgress = false;
    }

    public static void AddUnitToGame(string unitTag, UnitCharacter unitCharacter)
    {
        var unitToAdd = Tuple.Create(unitTag, unitCharacter);
        
        if (!AllUnits.Contains(unitToAdd))
        {
            _LivingUnits++;
            AllUnits.Add(unitToAdd);
        }
    }
}
