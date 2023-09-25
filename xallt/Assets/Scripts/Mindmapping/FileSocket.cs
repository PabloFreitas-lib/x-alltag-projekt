using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The file socket detects mindmaps and handles the connection between the selected mindmap and the user interface.
/// Here are the methods, that the UI is calling to create, edit and work with mindmaps based on the current socketFile.
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>
public class FileSocket : MonoBehaviour
{
    // file and file name
    public File socketFile;
    public Text fileNameText;
    [HideInInspector]public Mindmap map;

    /// <summary>
    /// Sets the mindmap placed on file socket to active.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
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

    /// <summary>
    /// Sets the mindmap placed on file socket to not active.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
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
    /// Creates a node from the selected mindmap.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void CreateNodeUI()
    {
        if(map != null)
        {
            map.CreateNode();
        }
    }
    /// <summary>
    /// Deletes a node from the selected mindmap.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void DeleteNodeUI()
    {
        if (map != null)
        {
            map.changeMode(3);
            map.DeleteNode();
        }
    }
    /// <summary>
    /// Creates a connection from the selected mindmap.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void CreateConnectionUI()
    {
        if (map != null)
        {
            map.changeMode(1);
        }
    }
    /// <summary>
    /// Deletes a connection from the selected mindmap.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void DeleteConnectionUI()
    {
        if (map != null)
        {
            map.changeMode(3);
        }
    }

    /// <summary>
    /// Changes the color of any object.
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
