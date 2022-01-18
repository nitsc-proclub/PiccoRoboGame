using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFactory : MonoBehaviour
{
    [SerializeField]
    Canvas targetCanvas;

    [SerializeField]
    HealthBar healthbarPrefab;

    [SerializeField]
    Camera overlayCamera;

    public void AssignToCharacter(GameObject obj, float offsetY = 60)
    {
        var character = obj.GetComponent<Character>();

        var barInstance = Instantiate(healthbarPrefab);

        barInstance.Initialize(character.transform, overlayCamera, offsetY);

        character.gameObject
            .AddComponent<HealthBarClient>()
            .Initialize(barInstance);
    }
}
