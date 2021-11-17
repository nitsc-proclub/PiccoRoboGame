using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class DevGameManager : MonoBehaviour
{
    private void Awake()
    {
        foreach(var i in Enumerable.Range(1, 20))
        {
            PhotonNetwork.Instantiate(
                "DevEnemy",
                new(Random.Range(-12, 12), 1, Random.Range(-12, 12)),
                Quaternion.identity);
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
