using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour
{
    const float DisplayDurationSec = 10.0f;
    const float FadeoutDurationSec = 0.4f;

    [SerializeField]
    Text textObj;

    Image image;

    public void Initialize(string text)
    {
        textObj.text = text;
    }

    void Awake()
    {
        image = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(StartFadeout), DisplayDurationSec);
    }

    void StartFadeout()
    {
        image.CrossFadeAlpha(0, FadeoutDurationSec, false);
        Invoke(nameof(FinishedFadeout), FadeoutDurationSec);
    }

    void FinishedFadeout()
    {
        Destroy(gameObject);
    }
}
