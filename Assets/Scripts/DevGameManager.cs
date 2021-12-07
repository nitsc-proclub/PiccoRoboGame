using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class DevGameManager : MonoBehaviour
{
    private void Awake()
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
        var player = PhotonNetwork.Instantiate(
            "DevPlayer",
            new(0, 0.5f, 0),
            Quaternion.identity);
        var script = player.GetComponent<ScriptMachine>();
        script.enabled = true;
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
