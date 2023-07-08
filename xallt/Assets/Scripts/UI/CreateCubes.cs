using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCubes : MonoBehaviour
{
    public GameObject cubePrefab;
        public Transform cubeParent;
        public Vector3 cubeScale = Vector3.one; // Variable für die Würfelgröße

        private Button button;
        private bool buttonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
               button.onClick.AddListener(TriggerButtonPress);    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
                {
                    CreateCube();
                    buttonPressed = false;
                }
    }
    private void TriggerButtonPress()
        {
            buttonPressed = true;
        }

        private void CreateCube()
        {
            GameObject newCube = Instantiate(cubePrefab, cubeParent);
            newCube.transform.localScale = cubeScale; // Setze die Skalierung des Würfels
        }

}
