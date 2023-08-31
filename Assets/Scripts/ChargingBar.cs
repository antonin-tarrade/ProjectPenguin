using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargingBar : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image progressImage;

    protected bool isCharging = false;

    // Start is called before the first frame update
    void Start()
    {
        progressImage.fillAmount = 0;
        this.gameObject.SetActive(false);
    }

    public void SetChargeOn()
    {
        isCharging = true;
        this.gameObject.SetActive(true);
    }

    public void UpdateBar(float number)
    {
        if (isCharging)
        {
            progressImage.fillAmount = Mathf.Min(1f, number);
        }

        if (number >= 1)
        {
            isCharging = false;
        }
    }

    public void ResetBar()
    {
        progressImage.fillAmount = 0;
        this.gameObject.SetActive(false);
    }
}
