using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Character : MonoBehaviourPunCallbacks, IPunObservable
{
    // --- ステータス ---

    /// <summary>
    /// チームID
    /// </summary>
    public byte TeamID = 0;

    /// <summary>
    /// HP
    /// </summary>
    public short HP = 100;

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

    private byte _TeamID = 0;

    private short _HP = 100;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // 値が変わったときのみ同期
            if(statusChanged || 
               TeamID != _TeamID || 
               HP != _HP)
            {
                TeamID = _TeamID;
                HP = _HP;

                stream.SendNext(TeamID);
                stream.SendNext(HP);
            }
        }
        else
        {
            TeamID = (byte)stream.ReceiveNext();
            HP = (short)stream.ReceiveNext();
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
