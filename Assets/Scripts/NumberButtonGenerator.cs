using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NumberButtonGenerator : MonoBehaviour
{
    // ボタンのプレファブ
    public GameObject NumberButtonPrefab;

    public Sprite img1;
    public Sprite img2;
    public Sprite img3;
    public Sprite img4;

    // Spriteの配列を持ったSerializableな型を作り...
    [Serializable]
    private struct SpriteSet
    {
        public Sprite[] sprites;
    }

    // それの配列を使う
    [SerializeField] private SpriteSet[] spriteSets;



    void Start()
    {
        // 5 * 5 でボタンを生成してみる
        //this.GenerateNumberButtons(5, 5);
    }

    public void GenerateNumberButtons(int rowCount, int colCount)
    {
        var canvas = GameObject.Find("Canvas");

        // 番号をシャッフル
        //var numbers = Enumerable.Range(1, rowCount * colCount)
        //    .OrderBy(i => Guid.NewGuid()).ToArray();
        var numbers = Enumerable.Range(1, rowCount * colCount).ToArray();
        //Debug.Log(string.Join(", ", numbers));

        // 一度ボタン等のオブジェクト全削除
        var buttons = GameObject.FindGameObjectsWithTag("NumberButton");
        foreach (var item in buttons) Destroy(item);

        // ボタンの位置調整用
        float offsetCountX = (colCount - 1) / 2.0f;
        float offsetCountY = (rowCount - 1) / 2.0f;

        int index = 0;

        var spriteSet = this.spriteSets[0];

        //だめだった...
        //if(rowCount*colCount > spriteSet.sprites.Length)
        //{
        //    colCount = spriteSet.sprites.Length % rowCount;
        //}

        for (int y = 0; y < rowCount; y++)
        {
            for (int x = 0; x < colCount; x++)
            {
                var sprite = spriteSet.sprites[index];
                
                // ボタンに設定される番号
                int number = numbers[index++];

                // ボタンを生成
                var button = Instantiate(this.NumberButtonPrefab) as GameObject;

                // ボタンの情報を設定
                var controller = button.GetComponent<NumberButtonController>();
                controller.SetButtonInfos(number, number.ToString());

                // 画面中央に配置されるようにする
                button.transform.SetParent(canvas.GetComponent<RectTransform>());

                //ボタンの画像を設定
                button.GetComponent<Image>().sprite = sprite;

                // (0, 0. 0) からボタンサイズ*個数分ずらして配置すると、中央から右上に偏る
                //button.transform.localPosition = new Vector3(
                //    controller.Width * x,
                //    controller.Heigh * y,
                //    0);

                // したがって、表示する行数列数から中央に来るように調整した位置とする
                button.transform.localPosition = new Vector3(
                    controller.Width * x - controller.Width * offsetCountX,
                    controller.Heigh * y - controller.Heigh * offsetCountY,
                    0);
            }
        }
    }
}
