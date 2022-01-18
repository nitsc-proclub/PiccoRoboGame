using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public enum DisplayKind : byte
{
    None,
    OwnerName,
    Custom
}

public enum Team : byte
{
    Unassigned,
    Observer,
    TeamA,
    TeamB,
    TeamC,
    NPC,
    Count
}

[RequireComponent(typeof(PhotonView))]
public class Character : MonoBehaviourPunCallbacks, IPunObservable
{
    // --- ステータス ---

    /// <summary>
    /// チームID
    /// </summary>
    public Team Team = Team.Unassigned;

    /// <summary>
    /// HP
    /// </summary>
    public short HP = 100;

    /// <summary>
    /// 表示名の種類
    /// </summary>
    public DisplayKind Display = DisplayKind.None;

    /// <summary>
    /// DisplayKindがCustomのときに表示するテキスト
    /// </summary>
    public string DisplayText = "";

    /// <summary>
    /// キルされた回数
    /// </summary>
    public byte DeadCount { get; private set; } = 0;

    // --- 設定 ---

    /// <summary>
    /// キルされた時に呼び出すVisualScriptingのCustomEventの名前
    /// </summary>
    public string KilledEventName = null;

    /// <summary>
    /// ダメージを受けた時に呼び出すVisualScriptingのCustomEventの名前
    /// </summary>
    public string DamagedEventName = null;

    /// <summary>
    /// 最大HP
    /// </summary>
    public short MaxHP = 100;

    /// <summary>
    /// キルされた場合、PhotonNetwork.Destroyを自動で呼ぶ
    /// </summary>
    public bool AutoDestroy = true;

    // --------

    private bool statusChanged = true;

    private DisplayKind _Display = DisplayKind.None;

    private Team _Team = 0;

    private short _HP = 100;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            bool displayChanged = Display != _Display;

            // 値が変わったときのみ同期
            if (statusChanged ||
               displayChanged ||
               Team != _Team || 
               HP != _HP)
            {
                Team = _Team;
                HP = _HP;
                Display = _Display;

                stream.SendNext((byte)Team);
                stream.SendNext(HP);
                stream.SendNext((byte)Display);

                bool sendDisplayText = displayChanged && Display == DisplayKind.Custom;
                stream.SendNext(sendDisplayText);
                if (sendDisplayText)
                {
                    stream.SendNext(DisplayText);
                }
            }
        }
        else
        {
            Team = (Team)(byte)stream.ReceiveNext();
            HP = (short)stream.ReceiveNext();
            Display = (DisplayKind)(byte)stream.ReceiveNext();

            bool recieveDisplayText = (bool)stream.ReceiveNext();
            if (recieveDisplayText)
            {
                DisplayText = (string)stream.ReceiveNext();
            }
        }
    }

    /// <summary>
    /// このキャラクターにダメージを与える
    /// </summary>
    /// <param name="damage">ダメージ量(負の値で回復)</param>
    public void GiveDamage(short damage)
    {
        this.photonView.RPC(
            nameof(GetDamage),
            this.photonView.Owner,
            damage);
    }

    /// <summary>
    /// このオブジェクトを破壊する
    /// </summary>
    public void Destroy()
    {
        PhotonNetwork.Destroy(photonView);
    }

    [PunRPC]
    private void GetDamage(short damage)
    {
        int newHP = HP;
        newHP -= damage;

        // HPを更新
        HP = (short)Math.Min(Math.Max(newHP, 0), MaxHP);

        if (!string.IsNullOrEmpty(DamagedEventName))
        {
            CustomEvent.Trigger(gameObject, DamagedEventName);
        }

        // キルされたとき
        if (newHP <= 0)
        {
            DeadCount++;
            if (!string.IsNullOrEmpty(KilledEventName))
            {
                CustomEvent.Trigger(gameObject, KilledEventName);
            }
            if(AutoDestroy)
            {
                Destroy();
            }
            statusChanged = true;
            return;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
