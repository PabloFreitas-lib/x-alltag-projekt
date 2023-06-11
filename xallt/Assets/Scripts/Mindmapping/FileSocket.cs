using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FileSocket : MonoBehaviour
{
    public File socketFile;
    public Text fileNameText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("File"))
        {
            socketFile = other.GetComponent<File>();
            fileNameText.text = socketFile.name;
            socketFile.isOpen = true;
            Debug.Log(socketFile.name);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("File"))
        {
            socketFile = other.GetComponent<File>();
            socketFile.isOpen = false;
        }
    }

}
