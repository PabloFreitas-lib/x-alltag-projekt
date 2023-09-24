using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform bigZeiger;
    public Transform smallZeiger;
    public Transform secondZeiger;

    private void Start()
    {
        StartCoroutine(UpdateClock());
    }

    private IEnumerator UpdateClock()
    {
        while (true)
        {
            System.DateTime time = System.DateTime.Now;

            float seconds = time.Second;
            float minutes = time.Minute;
            float hours = time.Hour % 12; // Modulo 12, um eine 12-Stunden-Uhr zu haben

            float secondRotation = seconds * 6f; // 360 Grad / 60 Sekunden = 6 Grad pro Sekunde
            float minuteRotation = (minutes + seconds / 60f) * 6f; // 360 Grad / 60 Minuten = 6 Grad pro Minute
            float hourRotation = (hours + minutes / 60f) * 30f; // 360 Grad / 12 Stunden = 30 Grad pro Stunde

            // Groﬂer Zeiger (Minute)
            bigZeiger.localRotation = Quaternion.Euler(0f, minuteRotation, 0f);

            // Kleiner Zeiger (Stunde)
            smallZeiger.localRotation = Quaternion.Euler(0f, hourRotation, 0f);

            // Sekundenzeiger
            if(secondZeiger!= null)
            {
                secondZeiger.localRotation = Quaternion.Euler(0f, secondRotation, 0f);
            }

            yield return new WaitForSeconds(1f); // Warte eine Sekunde.
        }
    }
}
