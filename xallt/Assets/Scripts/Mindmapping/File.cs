using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for Files
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>
public class File : MonoBehaviour
{
    // 
    public string fileName;
    public Mindmap map;
    public bool isOpen;


    private void Start()
    {
        isOpen = false;
    }
    private void Update()
    {
        if(map != null)
        {
            map.gameObject.SetActive(isOpen);
        }
        
    }
}
