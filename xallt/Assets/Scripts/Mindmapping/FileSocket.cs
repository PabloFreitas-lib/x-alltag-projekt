using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// file socket detects mind map
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>

public class FileSocket : MonoBehaviour
{
    // file and file name
    public File socketFile;
    public Text fileNameText;
    [HideInInspector]public Mindmap map;

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
            socketFile.isOpen = true;
            map = socketFile.map;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("File"))
        {
            fileNameText.text = "";
            socketFile = other.GetComponent<File>();
            socketFile.isOpen = false;
            map = null;
        }
    }

    /// <summary>
    /// calls upon functions from hand UI
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void CreateNodeUI()
    {
        if(map != null)
        {
            map.CreateNode();
        }
    }
    public void DeleteNodeUI()
    {
        if (map != null)
        {
            map.changeMode(3);
            map.DeleteNode();
        }
    }
    public void CreateConnectionUI()
    {
        if (map != null)
        {
            map.changeMode(1);
        }
    }
    public void DeleteConnectionUI()
    {
        if (map != null)
        {
            map.changeMode(3);
        }
    }

    /// <summary>
    /// changes the color
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="Color color"> requires color </param>
    public void ChangeColorUI(Color color)
    {
        if (map != null && map.selected != null)
        {
            map.selected.GetComponent<ColorChanger>().objectColor = color;
        }
    }

}
