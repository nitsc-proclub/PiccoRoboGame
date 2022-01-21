using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Color BarColor
    {
        get => hpBarContentImage.color;
        set => hpBarContentImage.color = value;
    }

    public float BarWidth
    {
        get => hpBar.rect.width;
        set => hpBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
    }

    private short _HP = 100;

    public short HP
    {
        get => _HP;
        set
        {
            _HP = value;
            UpdateHP();
        }
    }

    private short _MaxHP = 100;

    public short MaxHP
    {
        get => _MaxHP;
        set
        {
            _MaxHP = value;
            UpdateHP();
        }
    }

    public string DisplayName
    {
        get => displayNameText.text;
        set
        {
            bool empty = string.IsNullOrEmpty(value);
            displayNameText.enabled = !empty;
            displayNameText.text = empty ? "" : value;
        }
    }

    public bool Visible { get; set; } = true;

    public float Offset { get; set; } = 60;

    [SerializeField]
    RectTransform root;

    [SerializeField]
    Text hpText;

    [SerializeField]
    Text displayNameText;

    [SerializeField]
    RectTransform hpBar;

    [SerializeField]
    RectTransform hpBarParent;

    [SerializeField]
    RectTransform hpBarContent;

    Image hpBarContentImage;

    Canvas canvas;

    Transform targetPos;

    bool used = false;

    public void Initialize(Transform targetPos, Camera overlayCam)
    {
        this.targetPos = targetPos;

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
            screenPos.y = Mathf.Min(screenPos.y + Offset, Screen.height - root.sizeDelta.y);
            canvas.planeDistance = screenPos.z;
            root.anchoredPosition = screenPos;
        }
    }

    void UpdateHP()
    {
        // テキスト
        hpText.text = $"{HP}/{MaxHP}";

        // HPバー横幅
        float hpPer = MaxHP == 0 ? 0F : Mathf.Clamp01((float)HP / MaxHP);
        hpBarContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hpBarParent.rect.width * hpPer);
    }
}
