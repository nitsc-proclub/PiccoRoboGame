using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField]
    Text timerText;

    [SerializeField]
    RectTransform logBox;

    [SerializeField]
    LogText logTextPrefab;

    int nextPushLogTime = 0;

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


        if (millisec >= nextPushLogTime)
        {
            PushLog(
                string.Concat(
                    Enumerable.Repeat(
                        "Hoge",
                        Random.Range(1, 10)
                    )
                )
            );
            nextPushLogTime = millisec + Random.Range(400, 2000);
        }
    }
}
