using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsernamePanel : MonoBehaviour
{
    [SerializeField]
    private Text usernameText;

    [SerializeField]
    private GameObject crownIconObj;

    [SerializeField]
    private GameObject personIconObj;

    public void UpdateState(string name, bool crownIcon, bool personIcon)
    {
        usernameText.text = name;
        crownIconObj.SetActive(crownIcon);
        personIconObj.SetActive(personIcon);
    }
}
