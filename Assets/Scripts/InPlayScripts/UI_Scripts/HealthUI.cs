using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthImage;

    public float healthViewValue;

    // Update is called once per frame
    void Update()
    {
        if (healthViewValue != PlayerHealthManager.Instance.Health)
        {

        }
    }
}
