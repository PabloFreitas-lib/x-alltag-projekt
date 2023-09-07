using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Clock : UnityEngine.MonoBehaviour
{
    public GameObject bigZeiger;
    public GameObject smallZeiger;
    public GameObject secondZeiger;
    private Quaternion bigRot;
    private Quaternion smallRot;

    private System.DateTime time = System.DateTime.Now;


    // Start is called before the first frame update
    void Start()
    {
        ClockMove();
    }

    private void ClockMove()
    {
        StartCoroutine("UpdateBig");
        StartCoroutine("UpdateSmall");
        StartCoroutine("UpdateSecond");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator UpdateBig()
    {
        while (true)
        {
            // Die jetzige Uhrzeit
            System.DateTime time = System.DateTime.Now;

            // Die Minutenzahl der Uhrzeit
            float minutes = time.Minute;

            // Rotationswinkel des großen Zeigers
            Vector3 currentRotationBig = bigZeiger.transform.eulerAngles;

            // Aktualisieren der Rotation vom großen Zeiger
            currentRotationBig.x = minutes * 6f;

            // Rotation auf den großen Zeiger übernehmen
            bigZeiger.transform.eulerAngles = currentRotationBig;

            // Warte eine Sekunde.
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator UpdateSmall()
    {
        while(true) {

            // Die jetzige Uhrzeit
            System.DateTime time = System.DateTime.Now;

            // Die Stundenzahl der Uhrzeit
            float hours = time.Hour;

            // Rotationswinkel des kleinen Zeigers
            Vector3 currentRotationSmall = smallZeiger.transform.eulerAngles;

            // Aktualisieren der Rotation vom kleinen Zeiger
            currentRotationSmall.x = hours * 30f;

            // Rotation auf den kleinen Zeiger übernehmen
            smallZeiger.transform.eulerAngles = currentRotationSmall;

            // Warte eine Sekunde.
            yield return new WaitForSeconds(1);
        }
    }


    IEnumerator UpdateSecond()
    {
        while (true) {

            // Die jetzige Uhrzeit
            System.DateTime time = System.DateTime.Now;

            // Die Sekundenzahl der Uhrzeit
            float seconds = time.Second;

            // Rotationswinkel des großen Zeigers
            Vector3 currentRotationSeconds = secondZeiger.transform.eulerAngles;

            // Aktualisieren der Rotation vom großen Zeiger
            currentRotationSeconds.x = seconds * 6f;

            // Rotation auf den kleinen Zeiger übernehmen
            secondZeiger.transform.eulerAngles = currentRotationSeconds;

            // Warte eine Sekunde.
            yield return new WaitForSeconds(1);
        }
    }
}
