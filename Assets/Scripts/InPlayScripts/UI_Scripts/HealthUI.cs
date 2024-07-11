using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthImage;

    float healthViewValue;

    public float power;

    private void Start()
    {
        healthViewValue = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthViewValue != PlayerHealthManager.Instance.Health)
        {
            healthViewValue = Mathf.Lerp(healthViewValue, PlayerHealthManager.Instance.Health, power);

            if (Mathf.Abs(healthViewValue - PlayerHealthManager.Instance.Health) < 0.01f)
                healthViewValue = PlayerHealthManager.Instance.Health;

            healthImage.rectTransform.localScale = new Vector3(healthViewValue / 10f, 1, 1);
        }
    }
}
