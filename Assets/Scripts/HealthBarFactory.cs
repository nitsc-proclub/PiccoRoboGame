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

    public void AssignToCharacter(Character character, float offsetY = 60)
    {
        var barInstance = Instantiate(healthbarPrefab);

        barInstance.Initialize(character.transform, overlayCamera);
        barInstance.Offset = offsetY;

        character.gameObject
            .AddComponent<HealthBarClient>()
            .Initialize(barInstance);
    }
}
