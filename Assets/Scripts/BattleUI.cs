using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public bool MiniMapVisible
    {
        get => miniMap.activeInHierarchy;
        set => miniMap.SetActive(value);
    }

    public Character LeftBaseCharacter = null;

    public Character RightBaseCharacter;

    [SerializeField]
    GameObject miniMap;

    [SerializeField]
    GameObject leftTeamArea;

    [SerializeField]
    Text leftTeamText;

    [SerializeField]
    Image leftTeamHealthBar;

    [SerializeField]
    GameObject rightTeamArea;

    [SerializeField]
    Text rightTeamText;

    [SerializeField]
    Image rightTeamHealthBar;

    [SerializeField]
    Text timerText;

    [SerializeField]
    RectTransform logBox;

    [SerializeField]
    LogText logTextPrefab;

    public void PushLog(string text)
    {
        var newobj = Instantiate(logTextPrefab, logBox);
        newobj.Initialize(text);
        LayoutRebuilder.ForceRebuildLayoutImmediate(logBox);
    }

    void Update()
    {
        if (DevGameManager.Instance.CurrentPhase != GamePhase.Battle)
        {
            return;
        }

        int millisec = DevGameManager.Instance.BattlePhaseTime;

        // Timer
        {
            int sec = millisec / 1000;
            int min = sec / 60;
            sec %= 60;

            timerText.text = $"{min:0}:{sec:00}";
        }

        // HealthBar
        {
            bool left = LeftBaseCharacter is not null;
            leftTeamArea.SetActive(left);
            if (left)
            {
                leftTeamText.text = LeftBaseCharacter.Display switch
                {
                    DisplayKind.None => "",
                    DisplayKind.OwnerName => string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName)
                        ? PhotonNetwork.LocalPlayer.UserId
                        : PhotonNetwork.LocalPlayer.NickName,
                    DisplayKind.Custom => LeftBaseCharacter.DisplayText,
                    _ => ""
                };
                leftTeamHealthBar.fillAmount = Mathf.Clamp01((float)LeftBaseCharacter.HP / LeftBaseCharacter.MaxHP);
            }

            bool right = RightBaseCharacter is not null;
            rightTeamArea.SetActive(right);
            if (right)
            {
                rightTeamText.text = RightBaseCharacter.Display switch
                {
                    DisplayKind.None => "",
                    DisplayKind.OwnerName => string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName)
                        ? PhotonNetwork.LocalPlayer.UserId
                        : PhotonNetwork.LocalPlayer.NickName,
                    DisplayKind.Custom => RightBaseCharacter.DisplayText,
                    _ => ""
                };
                rightTeamHealthBar.fillAmount = Mathf.Clamp01((float)RightBaseCharacter.HP / RightBaseCharacter.MaxHP);
            }
        }
    }
}
