using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    [SerializeField]
    private DevGameManager gameMgr;

    [SerializeField]
    private Text timerText;

    void Update()
    {
        float sec = (DevGameManager.CountDownDurationMS - gameMgr.MatchTime) / 1000F;
        timerText.text = $"{sec:0.0}";
    }
}
