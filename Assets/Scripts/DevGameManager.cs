using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public enum GamePhase
{
    None,
    MatchMaking,
    EditScripts,
    Battle,
    GameOver
}

public enum Team : int
{
    Unassigned = 0,
    Observer = 1,
    TeamA = 2,
    TeamB = 3,
    TeamC = 4,
    NPC = 5
}

public class DevGameManager : MonoBehaviourPunCallbacks
{
    public GamePhase CurrentPhase { get; private set; } = GamePhase.None;

    private Dictionary<int, Team> teamAssignDic = new();

    [SerializeField]
    private GameObject matchMakingUI;

    void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var i in Enumerable.Range(1, 20))
            {
                PhotonNetwork.Instantiate(
                    "DevEnemy",
                    new(Random.Range(-12, 12), 1, Random.Range(-12, 12)),
                    Quaternion.identity);
            }
        }
        StartMatchMakingPhase();
    }

    void Start()
    {
        AssignTeam(PhotonNetwork.LocalPlayer, Team.Observer);
    }

    void Update()
    {
        matchMakingUI.SetActive(CurrentPhase == GamePhase.MatchMaking);
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
            photonView.RPC(nameof(AssignTeamImpl), RpcTarget.All, player.ActorNumber, (int)team);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var (playerId, team) in teamAssignDic)
            {
                photonView.RPC(nameof(AssignTeamImpl), newPlayer, playerId, (int)team);
            }
        }
    }

    [PunRPC]
    private void AssignTeamImpl(int playerId, int teamId)
    {
        teamAssignDic[playerId] = (Team)teamId;
    }

    private void StartMatchMakingPhase()
    {
        CurrentPhase = GamePhase.MatchMaking;
    }

    [PunRPC]
    private void StartEditScriptsPhase()
    {
        CurrentPhase = GamePhase.EditScripts;
    }

    [PunRPC]
    private void StartBattlePhase()
    {
        CurrentPhase = GamePhase.Battle;
        CreateOwnPlayer();
    }

    [PunRPC]
    private void GameOver()
    {
        CurrentPhase = GamePhase.GameOver;
    }

    private void CreateOwnPlayer()
    {
        var player = PhotonNetwork.Instantiate(
            "DevPlayer",
            new(0, 0.5f, 0),
            Quaternion.identity);
        var script = player.GetComponent<ScriptMachine>();
        script.enabled = true;
    }

}
