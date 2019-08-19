using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject HB;
    private Transform bar;
    
    public float healthbar_state;

    // Start is called before the first frame update
    private void Start()
    {
        bar = HB.transform;
    }

    public void SetHealthState(float hb_state)
    {
        healthbar_state = hb_state;
        bar.localScale = new Vector3(healthbar_state, 1f);
    }
}
