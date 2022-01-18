using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Color BarColor { get; set; } = new Color(0.45f, 0.89f, 0.26f);

    public short HP { get; set; } = 100;

    public short MaxHP { get; set; } = 100;

    public bool Visible { get; set; } = true;

    [SerializeField]
    RectTransform root;

    [SerializeField]
    Text hpText;

    [SerializeField]
    RectTransform hpBarParent;

    [SerializeField]
    RectTransform hpBarContent;

    Image hpBarContentImage;

    Canvas canvas;

    Transform targetPos;

    bool used = false;

    float ofssetY = 60;

    public void Initialize(Transform targetPos, Camera overlayCam, float ofssetY = 60)
    {
        this.targetPos = targetPos;
        this.ofssetY = ofssetY;

        canvas = GetComponent<Canvas>();
        hpBarContentImage = hpBarContent.GetComponent<Image>();

        canvas.worldCamera = overlayCam;
        used = true;
    }

    public void Unuse()
    {
        used = false;
    }

    void Update()
    {
        if(!used)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos.position);
        bool display = Visible;
        display &= 0 <= screenPos.x && screenPos.x < Screen.width;
        display &= 0 <= screenPos.y && screenPos.y < Screen.height;

        canvas.enabled = display;
        if (display)
        {
            // 座標
            screenPos.y = Mathf.Min(screenPos.y + ofssetY, Screen.height - root.sizeDelta.y);
            canvas.planeDistance = screenPos.z;
            root.anchoredPosition = screenPos;

            // テキスト
            hpText.text = $"{HP}/{MaxHP}";

            // HPバー横幅
            float hpPer = MaxHP == 0 ? 0F : Mathf.Clamp01((float)HP / MaxHP);
            hpBarContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpBarParent.rect.width * hpPer);

            // HPバー色
            hpBarContentImage.color = BarColor;
        }
    }
}
