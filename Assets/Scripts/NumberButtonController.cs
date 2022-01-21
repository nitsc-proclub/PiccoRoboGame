using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NumberButtonController : MonoBehaviour
{
    public GameDirector GameDirector { get; set; }

    public static List<int> dataList = new List<int>();
    public string dataText;


    // ボタンに表示されるテキストと数値
    public string Text { get; private set; }
    public int Number { get; private set; }

    // ボタンのサイズ
    public int Width = 70;
    public int Heigh = 70;

    void Start()
    {
        this.GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        // ボタンクリック時の処理を追加
        this.GetComponent<Button>().onClick.AddListener(OnClick);

        // ボタンのサイズを設定
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.Width, this.Heigh);
    }

    // ボタンの情報を設定する
    public void SetButtonInfos(int number, string text)
    {
        this.Text = text;
        this.Number = number;

        this.GetComponentsInChildren<Text>()[0].text = text;
    }

    // クリック時の処理
    private void OnClick()
    {
        ListManager.list_string += this.Number.ToString();
        ListManager.list_string += ",";


        dataList.Add(this.Number);
        //Debug.Log(string.Join(",", dataList));

        int listCount = dataList.Count;
        Debug.Log(listCount);

        this.GameDirector.ChangeNextValue();
        Destroy(gameObject);

        // クリア判定と処理を呼び出し
        this.GameDirector.ClearGameIfAllButtonClicked();

    }
}
