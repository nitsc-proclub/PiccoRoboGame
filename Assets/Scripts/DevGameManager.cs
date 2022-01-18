using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.IO;
using UnityEngine.SceneManagement;

public enum GamePhase
{
    None,
    MatchMaking,
    CountDown,
    EditScripts,
    Battle,
    GameOver,
    Count
}

public class DevGameManager : MonoBehaviourPunCallbacks
{
    public const int CountDownDurationMS = 1000 * 1; // 1sec
    public const int EditScriptDurationMS = 1000 * 1; // 1sec

    public int MatchTime
    {
        get
        {
            return matchTime;
        }
    }

    public GamePhase CurrentPhase { get; private set; } = GamePhase.None;

    private int matchTime = 0;

    private int prevServerTime;

    private Dictionary<int, Team> teamAssignDic = new();

    [SerializeField]
    private GameObject matchMakingUI;

    [SerializeField]
    private GameObject countdownUI;

    [SerializeField]
    private GameObject editScriptUI;

    [SerializeField]
    private GameObject battleUI;

    private HealthBarFactory healthBarFactory;

    void OnGUI()
    {
        GUILayout.Window(2, new(10, 220, 200, 1), _ =>
        {
            GUILayout.Label($"Phase: {CurrentPhase}");
            GUILayout.Label($"MatchTime: {MatchTime}");
        }, "GameState");
    }

    void Awake()
    {
        healthBarFactory = GetComponent<HealthBarFactory>();

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var i in Enumerable.Range(1, 20))
            {
                var enemy = PhotonNetwork.Instantiate(
                    "DevEnemy",
                    new(Random.Range(-12, 12), 1, Random.Range(-12, 12)),
                    Quaternion.identity);
                var character = enemy.GetComponent<Character>();
                healthBarFactory.AssignToCharacter(character);
                character.Display = DisplayKind.Custom;
                character.DisplayText = $"Enemy{i}";
            }
        }

        StartMatchMakingPhase();
        
    }

    void Start()
    {
        AssignTeam(PhotonNetwork.LocalPlayer, Team.Observer);
    }

    private void FixedUpdate()
    {
        if (CurrentPhase != GamePhase.MatchMaking)
        {
            int currentServerTime = PhotonNetwork.ServerTimestamp;
            matchTime += unchecked(currentServerTime - prevServerTime);
            prevServerTime = currentServerTime;
        }
    }

    void Update()
    {
        switch(CurrentPhase)
        {
            case GamePhase.MatchMaking:
                matchMakingUI.SetActive(true);
                break;
            case GamePhase.CountDown:
                matchMakingUI.SetActive(false);
                countdownUI.SetActive(true);
                if (matchTime >= CountDownDurationMS)
                {
                    StartEditScriptsPhase();
                }
                break;
            case GamePhase.EditScripts:
                countdownUI.SetActive(false);
                editScriptUI.SetActive(true);
                if (matchTime >= CountDownDurationMS + EditScriptDurationMS)
                {
                    StartBattlePhase();
                }
                break;
            case GamePhase.Battle:
                editScriptUI.SetActive(false);
                battleUI.SetActive(true);
                break;
            case GamePhase.GameOver:
                break;
        }
    }

    public Team GetAssignedTeam()
    {
        return GetAssignedTeam(PhotonNetwork.LocalPlayer);
    }

    public Team GetAssignedTeam(Player player)
    {
        return teamAssignDic.GetValueOrDefault(player.ActorNumber, Team.Unassigned);
    }

    public IEnumerable<(Player, Team)> GetAllAssignedTeam()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            yield return (player, GetAssignedTeam(player));
        }
    }

    public void AssignTeam(Player player, Team team)
    {
        if (CurrentPhase == GamePhase.MatchMaking)
        {
            photonView.RPC(nameof(AssignTeamImpl), RpcTarget.All, player.ActorNumber, (byte)team);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var (playerId, team) in teamAssignDic)
            {
                photonView.RPC(nameof(AssignTeamImpl), newPlayer, playerId, (byte)team);
            }
        }
    }

    public void RequestMatchStart()
    {
        photonView.RPC(nameof(MatchStartImpl), RpcTarget.AllViaServer);
    }

    public void RequestGameOver(Team winner)
    {
        photonView.RPC(nameof(GameOverImpl), RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void MatchStartImpl(PhotonMessageInfo info)
    {
        StartCountDownPhase();
        matchTime = 0;
        prevServerTime = info.SentServerTimestamp;
    }

    [PunRPC]
    private void GameOverImpl()
    {
        GameOver();
    }

    [PunRPC]
    private void AssignTeamImpl(int playerId, byte teamId)
    {
        teamAssignDic[playerId] = (Team)teamId;
    }

    private void StartMatchMakingPhase()
    {
        Debug.Log("[Phase] MatchMaking");
        CurrentPhase = GamePhase.MatchMaking;
    }

    private void StartCountDownPhase()
    {
        Debug.Log("[Phase] CountDown");
        CurrentPhase = GamePhase.CountDown;
    }

    private void StartEditScriptsPhase()
    {
        Debug.Log("[Phase] EditScripts");
        CurrentPhase = GamePhase.EditScripts;
    }

    private void StartBattlePhase()
    {
        Debug.Log("[Phase] Battle");
        CurrentPhase = GamePhase.Battle;
        var team = GetAssignedTeam();
        if (team == Team.TeamA || team == Team.TeamB)
        {
            CreateOwnPlayer();
        }
        ActivateBattleManager();
    }

    [PunRPC]
    private void GameOver()
    {
        Debug.Log("[Phase] GameOver");
        CurrentPhase = GamePhase.GameOver;
    }

    private void ActivateBattleManager()
    {
        foreach (var gameObj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (gameObj.name == "BattleManager")
            {
                gameObj.SetActive(true);
            }
        }
    }

    private void CreateOwnPlayer()
    {
        
    }
}
