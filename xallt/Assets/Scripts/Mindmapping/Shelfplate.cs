using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shelfplate : MonoBehaviour
{
    public File socketFile;
    public Text fileNameText;

    public List<File> filelist;

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
            fileNameText.text = socketFile.fileName;
            filelist.Add(socketFile);
            Debug.Log(socketFile.fileName);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("File"))
        {
            filelist.Remove(socketFile);
            fileNameText.text = "";
            socketFile = other.GetComponent<File>();
        }
    }
}
