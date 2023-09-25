using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Puts files in list.
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>
public class Shelfplate : MonoBehaviour
{
    // file and file name and list of all files
    public File socketFile;
    public Text fileNameText;

    public List<File> filelist;

    /// <summary>
    /// Saves files placed on shelf plate in list.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
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

    /// <summary>
    /// Removes files placed on shelf plate from list.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
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
