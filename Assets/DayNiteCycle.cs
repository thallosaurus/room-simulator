using System;
using Unity.Mathematics;
using UnityEngine;

public class DayNiteCycle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    const float SecondsInADay = 86400;
    private Light light;
    void Start()
    {
        light = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        DateTime now = DateTime.Now;
        light.intensity = MapValue(ConvertTimeToSeconds(now.Hour, now.Minute, now.Second));
    }

    private int ConvertTimeToSeconds(int h, int m, int s) {
        return h * 3600 + m * 60 + s;
    }

    float MapValue(int timeInSeconds)
    {
        int halfDay = 43200; // Hälfte von 86400 Sekunden (12 Stunden)

        if (timeInSeconds >= 0 && timeInSeconds <= halfDay)
        {
            // Lineare Interpolation von 0 bis 1.5 für 0 bis 43200
            return Mathf.Lerp(0f, 1.5f, timeInSeconds / (float)halfDay);
        }
        else if (timeInSeconds >= halfDay + 1 && timeInSeconds <= 86400)
        {
            // Lineare Interpolation von 1.5 bis 0 für 43201 bis 86400
            return Mathf.Lerp(1.5f, 0f, (timeInSeconds - halfDay - 1) / (float)halfDay);
        }
        else
        {
            Debug.LogWarning("Wert außerhalb des gültigen Bereichs (0-86400)");
            return 0f; // Rückgabewert für ungültige Zeiten
        }
    }
}
