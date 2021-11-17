using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviourPunCallbacks, IPunObservable
{
    /// <summary>
    /// チームID
    /// </summary>
    public byte TeamID;

    /// <summary>
    /// HP
    /// </summary>
    public short HP = 100;

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
    /// キルされた回数
    /// </summary>
    public byte DeadCount { get; private set; } = 0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(TeamID);
            stream.SendNext(HP);
        }
        else
        {
            TeamID = (byte)stream.ReceiveNext();
            HP = (byte)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// シーン中のすべてのキャラクターにダメージを与える
    /// </summary>
    /// <param name="damage">ダメージ量(負の値で回復)</param>
    public void GiveDamageToAll(short damage)
    {
        photonView.RPC(nameof(GetDamageToAll), RpcTarget.All, damage);
    }

    /// <summary>
    /// 指定のキャラクターにダメージを与える
    /// </summary>
    /// <param name="character">キャラクター</param>
    /// <param name="damage">ダメージ量(負の値で回復)</param>
    public void GiveDamageToCharacter(Character character, short damage)
    {
        character.photonView.RPC(
            nameof(GetDamageToCharacter),
            character.photonView.Owner,
            character.photonView.ViewID,
            damage);
    }

    /// <summary>
    /// 指定のチーム全員にダメージを与える
    /// </summary>
    /// <param name="teamID">チームID</param>
    /// <param name="damage">ダメージ量(負の値で回復)</param>
    public void GiveDamageToTeam(byte teamID, short damage)
    {
        photonView.RPC(
            nameof(GetDamageToTeam),
            RpcTarget.All,
            teamID,
            damage);
    }

    [PunRPC]
    private void GetDamageToAll(short damage)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        GetDamage(damage);
    }

    [PunRPC]
    private void GetDamageToCharacter(int targetViewID, short damage)
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(photonView.ViewID != targetViewID)
        {
            return;
        }

        GetDamage(damage);
    }

    [PunRPC]
    private void GetDamageToTeam(byte targetTeamID, short damage)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (TeamID != targetTeamID)
        {
            return;
        }

        GetDamage(damage);
    }

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
        if (HP <= 0)
        {
            HP = 0;
            DeadCount++;
            if (!string.IsNullOrEmpty(KilledEventName))
            {
                CustomEvent.Trigger(gameObject, KilledEventName);
            }
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
