using UnityEngine;
using UnityEngine.UI;

public class DataController : MonoBehaviour
{
    // 計測時間
    // public float TimeCounte { get; private set; } = 0f;

    // 開始されたかどうかのフラグ
    public bool IsStarted { get; set; } = false;

    // 経過時間を表示するテキストオブジェクト
    public GameObject DataText { get; private set; }

    void Start()
    {
        // テキストオブジェクトを取得しておく
        this.DataText = GameObject.Find("Data");

        // 計測開始
        this.IsStarted = true;
    }

    void Update()
    {
        if (this.IsStarted)
        {
            // タイマーが計測開始中の場合のみ
            // 経過時間を加え、テキストに表示
            //this.TimeCounte += Time.deltaTime;
            //this.TimerText.GetComponent<Text>().text = this.TimeCounte.ToString("F2");
        }
    }
}
