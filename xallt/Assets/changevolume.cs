using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changevolume : MonoBehaviour
{
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        //Slider verknüpfen
        volumeSlider.onValueChanged.AddListener(ChangeVolume); 
    }

    void ChangeVolume(float newVolume)
    {
        //Systemlautstärke entsprechend des Slider-Werts anpassen
        AudioListener.volume = newVolume;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
