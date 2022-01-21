using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class HealthBarClient : MonoBehaviour
{
    public bool Visible
    {
        get => healthbar.Visible;
        set => healthbar.Visible = value;
    }

    static readonly Color[] teamColors = new Color[(int)Team.Count]
    {
        new (0.45f, 0.89f, 0.26f),
        new (0.45f, 0.89f, 0.26f),
        new (1, 0, 0),
        new (0, 0, 1),
        new (0, 1, 1),
        new (0.45f, 0.89f, 0.26f),
    };

    HealthBar healthbar;

    Character character;

    public void Initialize(HealthBar healthbar)
    {
        this.healthbar = healthbar;

        character = GetComponent<Character>();
    }

    void Update()
    {
        healthbar.HP = character.HP;
        healthbar.MaxHP = character.MaxHP;
        healthbar.BarColor = teamColors[(byte)character.Team];
        healthbar.DisplayName = character.Display switch
        {
            DisplayKind.None => "",
            DisplayKind.OwnerName => string.IsNullOrEmpty(PhotonNetwork.LocalPlayer.NickName)
                ? PhotonNetwork.LocalPlayer.UserId
                : PhotonNetwork.LocalPlayer.NickName,
            DisplayKind.Custom => character.DisplayText,
            _ => ""
        };
    }

    void OnDestroy()
    {
        healthbar.Unuse();
    }
}
