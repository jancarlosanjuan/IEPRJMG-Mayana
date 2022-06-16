using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Date : MonoBehaviour
{
    public TextMeshProUGUI dayMonth;
    public TextMeshProUGUI hourMinutesSeconds;
    private float ticks;
    private float maxTicks = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTime();
    }

    // Update is called once per frame
    void Update()
    {
        ticks += Time.deltaTime;
        if(ticks >= maxTicks)
        {
            ticks -= maxTicks;
            UpdateTime();
        }
    }


    public void UpdateTime()
    {
        string date = System.DateTime.UtcNow.ToLocalTime().ToString("dd MMM");
        string time = System.DateTime.UtcNow.ToLocalTime().ToString("hh : mm tt");
        dayMonth.text = date;
        hourMinutesSeconds.text = time;
    }
}
