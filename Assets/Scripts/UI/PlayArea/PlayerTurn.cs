using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

enum Teams
{
    Player0, // Do not use. This is a placeholder that will work with player team tags.
    Player1,
    Player2
}

public class PlayerTurn : MonoBehaviour
{
    public Color Player1Color = Color.blue;
    public Color Player2Color = Color.red;
    public string Team1Name = "ChangeMe1";
    public string Team2Name = "ChangeMe2";
    
    [HideInInspector]
    private Teams currentTeam;
    [HideInInspector]
    private string currentTeamName;
    [HideInInspector]
    private Color currentTeamColor;
    
    [HideInInspector]
    public Text playerTurnText;
    [HideInInspector]
    public TurnManager turnManager;
    
    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        playerTurnText = GetComponent<Text>();
    }

    void Update()
    {
        if (currentTeam != (Teams)int.Parse(turnManager._CurrentlyActiveUnit.tag.Last().ToString()))
        {
            UpdatePlayerTurnUiBanner();
        }
    }

    public void UpdatePlayerTurnUiBanner()
    {
        SetCurrentTeam();
        playerTurnText.text = $"{currentTeamName}'s Turn";
        playerTurnText.color = currentTeamColor;
    }

    private void SetCurrentTeam()
    {
        currentTeam = (Teams)int.Parse(turnManager._CurrentlyActiveUnit.tag.Last().ToString());
        switch (currentTeam)
        {
            case Teams.Player0:
                throw new System.Exception("no such thing as player 0. update to different player tag.");
            case Teams.Player1:
                currentTeamColor = Player1Color;
                currentTeamName = Team1Name;
                break;
            case Teams.Player2:
                currentTeamColor = Player2Color;
                currentTeamName = Team2Name;
                break;
            default:
                throw new System.Exception("Unable to use this unit's tag to determine team. Ensure the unit has the correct team/player tag.");
        }
    }
}
