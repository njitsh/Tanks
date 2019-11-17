using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image barImage;
    public GameObject bar;

    // Start is called before the first frame update
    private void Awake()
    {
        barImage = bar.GetComponent<Image>();
    }

    public void SetHealthState(float hb_state)
    {
        barImage.fillAmount = hb_state;
    }
}