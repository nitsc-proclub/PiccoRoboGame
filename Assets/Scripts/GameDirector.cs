using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class GameDirector : MonoBehaviour
{
    // 結果表示用UIのPreafab
    public GameObject ResultTextPrefab;

    // 生成するボタンの数
    public static int ButtonRowCount = 3;
    public static int ButtonColCount = 3;
    //public static int ButtonAllCount = ButtonRowCount * ButtonColCount;
    public static int ButtonAllCount = 5;

    // 取得したボタンのデータ
    public static int[] dataArray = new int[5];
    public static List<int> newdataList = new List<int>();
    public static string dataString;

    // 次の数字
    public int NextNumber { get; set; } = 1;



    // クリアの判定
    public bool IsCleared { get { return ButtonAllCount < this.NextNumber; } }

    // クリックした値の正誤判定
    public bool CheckNumber(int number)
    {
        return number == this.NextNumber;
    }


    // 次の値を更新する
    public void ChangeNextValue()
    {
        this.NextNumber++;
    }

    // 開始時にボタンを生成する
    void Start()
    {
        // オブジェクトからスクリプトコンポーネントを取得
        // 生成メソッドを呼び出す
        GameObject.Find("NumberButtonGenerator")
            .GetComponent<NumberButtonGenerator>()
            .GenerateNumberButtons(ButtonRowCount, ButtonColCount);
    }

    void Update()
    {
        if (this.IsCleared && Input.GetMouseButtonDown(0))
        {
            // クリア時にクリックされたらスタート画面へ遷移
            SceneManager.LoadScene("DevScene");
            ListManager.list_string = "";
        }
    }

    // クリア処理
    public void ClearGameIfAllButtonClicked()
    {
        if (this.IsCleared)
        {
            // 一度ボタン等のオブジェクト全削除
            var buttons = GameObject.FindGameObjectsWithTag("NumberButton");
            foreach (var item in buttons) Destroy(item);

            // タイマーを止めて結果を表示
            var timerObject = GameObject.Find("Timer");
            var timerController = timerObject.GetComponent<TimerController>();
            timerController.IsStarted = false;
            var resultTime = timerController.TimeCounte;

            // 結果を表示
            var resultText = $"Result\n{ListManager.list_string.TrimEnd(',')}";
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
