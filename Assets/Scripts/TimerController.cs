using UnityEngine;
using UnityEngine.UI;


public class TimerController : MonoBehaviour
{
    // 結果表示用UIのPreafab
    public GameObject ResultTextPrefab;

    // 計測時間
    public float TimeCounte { get; private set; } = 10f;

    // 開始されたかどうかのフラグ
    public bool IsStarted { get; set; } = false;

    // 経過時間を表示するテキストオブジェクト
    public GameObject TimerText { get; private set; }

    public GameDirector GameDirector { get; set; }

    void Start()
    {
        // テキストオブジェクトを取得しておく
        this.TimerText = GameObject.Find("Timer");

        // 計測開始
        this.IsStarted = true;
    }

    void Update()
    {
        if (this.IsStarted)
        {
            // タイマーが計測開始中の場合のみ
            // 経過時間を加え、テキストに表示
            this.TimeCounte -= Time.deltaTime;
            this.TimerText.GetComponent<Text>().text = this.TimeCounte.ToString("F2");

            if(this.TimeCounte <= 0)
            {
                // 一度ボタン等のオブジェクト全削除
                var buttons = GameObject.FindGameObjectsWithTag("NumberButton");
                foreach (var item in buttons) Destroy(item);

                // タイマーを止めて結果を表示
                var timerObject = GameObject.Find("Timer");
                var timerController = timerObject.GetComponent<TimerController>();
                this.IsStarted = false;
                var resultTime = timerController.TimeCounte;

                // 結果を表示
                var resultText = $"選択個数が不足...\nResult\n{ListManager.list_string.TrimEnd(',')}";
                var resultTextObject = Instantiate(this.ResultTextPrefab) as GameObject;
                resultTextObject.GetComponent<Text>().text = resultText;

                // キャンバスの子として表示
                resultTextObject.GetComponent<Transform>().SetParent(
                    GameObject.Find("Canvas").GetComponent<Transform>()
                    );
                resultTextObject.transform.localPosition = new Vector3(0, 0, 0);

                // 右上の数字は非表示にする
                timerObject.SetActive(false);
            }
        }
    }
}
