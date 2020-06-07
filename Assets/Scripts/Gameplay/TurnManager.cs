using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    // Edit these
    public float turnSpeedSensitivityMultiplier = 1f;

    public Image winnerScreenImage;
    public Image playerTurnBannerImage;
    

    // ==========Do not change these variables=============
    [HideInInspector]
    public bool Quit = false;
    [HideInInspector]
    public bool GameHasBeenWon = false;
    [HideInInspector]
    public bool GameDone = false;
    private List<string> playerTagCheckList = new List<string>();
    [HideInInspector]
    public static List<Tuple<string, UnitCharacter>> AllUnits = new List<Tuple<string, UnitCharacter>>();
    [HideInInspector]
    public static Queue<Tuple<string, UnitCharacter>> UnitTurnOrder = new Queue<Tuple<string, UnitCharacter>>();
    private static bool _creatingTurnQueue = false;
    private static bool turnInProgress = false;
    [HideInInspector]
    public static CameraController MainCamera;
    private bool RunningTurnCountdown = false;
    [HideInInspector]
    public static int _TurnCounter = 1;
    [HideInInspector]
    public static int _LivingUnits = 0;    
    public UnitCharacter _CurrentlyActiveUnit;
    [HideInInspector]
    public CurrentCharacterInformation UiCharInfo;
    [HideInInspector]
    public UnitLists UiUnitLists;
    [HideInInspector]
    public ActionButtons actionButtons;
    //=====================================================

    void Start()
    {
        UiUnitLists = GameObject.FindObjectOfType<UnitLists>();
        UiCharInfo = GameObject.FindObjectOfType<CurrentCharacterInformation>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponentInParent<CameraController>();
        actionButtons = GameObject.FindObjectOfType<ActionButtons>();
        Init();
    }

    void Init()
    {
        MainCamera.ZoomOut();
        MainCamera.TiltDown();
        UpdateUiInfo();
    }

    void Update()
    {
        if (!GameHasBeenWon)
        {
            if (turnInProgress || _creatingTurnQueue || RunningTurnCountdown)
            {
                return;
            }
            else if (UnitTurnOrder.Count < AllUnits.Count)
            {
                CreateTurnQueue();
                CheckForWinCondition();
                TurnCountdown();
            }
        }
        if (GameHasBeenWon && !GameDone)
        {
            DisplayWinnerScreen();
        }
        if (GameDone)
        {
            
        }
    }
    private void LateUpdate()
    {
        if (!GameHasBeenWon)
        {
            if (UnitTurnOrder.Count == AllUnits.Count && !turnInProgress)
            {                
                StartTurn();
                UpdateUiInfo();
            }
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
                unit.Item2._FullTurnCounter -= unit.Item2._Speed * turnSpeedSensitivityMultiplier;
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

    private void CheckForWinCondition()
    {
        var UnitCheckAgainst = UnitTurnOrder.First().Item1;
        foreach (var unit in UnitTurnOrder)
        {
            if (!unit.Item1.Equals(UnitCheckAgainst))
            {
                return;
            }
        }
        GameHasBeenWon = true;
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

    #region ClickableActions
    
    public void DisplayWinnerScreen()
    {
        StartCoroutine(FadeImage(false, winnerScreenImage));
        GameDone = true;
    }    

    public void DisplayPlayerTurnBannerAndFade()
    {        
        StartCoroutine(FadeImage(false, playerTurnBannerImage));
        StartCoroutine(FadeImage(true, playerTurnBannerImage));
    }

    #endregion

    public void UpdateUiInfo()
    {
        UiCharInfo.UpdateCurrentCharacter(_CurrentlyActiveUnit);
        UiUnitLists.UpdateAllLists(AllUnits);
        actionButtons.currentUnit = _CurrentlyActiveUnit;
    }

    public IEnumerator<Coroutine> FadeImage(bool fadeAway, Image imageToFade)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                imageToFade.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                imageToFade.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }    
}
