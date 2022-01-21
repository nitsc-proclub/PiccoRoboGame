using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingUI : MonoBehaviour
{
    [SerializeField]
    private DevGameManager gameMgr;

    [SerializeField]
    private GameObject usernamePrefab;

    [SerializeField]
    private GameObject teamOvserverUserlist;

    [SerializeField]
    private Text teamATitle;

    [SerializeField]
    private GameObject teamAUserlist;

    [SerializeField]
    private Text teamBTitle;

    [SerializeField]
    private GameObject teamBUserlist;

    [SerializeField]
    private GameObject gameStartButton;

    private void Update()
    {
        UpdateAllUsersState();
        gameStartButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void AssignToTeamOvserver(bool toggle)
    {
        if(toggle)
        {
            gameMgr.AssignTeam(PhotonNetwork.LocalPlayer, Team.Observer);
        }
    }

    public void AssignToTeamA(bool toggle)
    {
        if (toggle)
        {
            gameMgr.AssignTeam(PhotonNetwork.LocalPlayer, Team.TeamA);
        }
    }

    public void AssignToTeamB(bool toggle)
    {
        if (toggle)
        {
            gameMgr.AssignTeam(PhotonNetwork.LocalPlayer, Team.TeamB);
        }
    }

    public void GameStart()
    {
        gameMgr.RequestMatchStart();
    }

    private void SyncPlayerState(GameObject obj, List<Player> list)
    {
        // 子の個数を調整
        if (obj.transform.childCount < list.Count)
        {
            foreach (var _ in Enumerable.Range(0, list.Count - obj.transform.childCount))
            {
                Instantiate(usernamePrefab, obj.transform);
            }
        }
        else if (obj.transform.childCount > list.Count)
        {
            foreach (int idx in Enumerable.Range(list.Count, obj.transform.childCount - list.Count))
            {
                Destroy(obj.transform.GetChild(idx).gameObject);
            }
        }

        // 値を同期
        for (int i = 0; i < list.Count; i++)
        {
            var player = list[i];
            var panel = obj.transform.GetChild(i).gameObject.GetComponent<UsernamePanel>();
            panel.UpdateState(
                name: string.IsNullOrEmpty(player.NickName) ? player.UserId : player.NickName,
                crownIcon: player.IsMasterClient,
                personIcon: player.IsLocal
            );
        }
    }

    private void UpdateAllUsersState()
    {
        List<Player> teamObserverplayers = new();
        List<Player> teamAplayers = new();
        List<Player> teamBplayers = new();

        foreach (var (player, team) in gameMgr.GetAllAssignedTeam())
        {
            switch (team)
            {
                case Team.Observer:
                    teamObserverplayers.Add(player);
                    break;
                case Team.TeamA:
                    teamAplayers.Add(player);
                    break;
                case Team.TeamB:
                    teamBplayers.Add(player);
                    break;
            }
        }

        SyncPlayerState(teamOvserverUserlist, teamObserverplayers);
        SyncPlayerState(teamAUserlist, teamAplayers);
        SyncPlayerState(teamBUserlist, teamBplayers);

        teamATitle.text = $"Aチーム ({teamAplayers.Count}/3)";
        teamBTitle.text = $"Bチーム ({teamBplayers.Count}/3)";
    }

}
