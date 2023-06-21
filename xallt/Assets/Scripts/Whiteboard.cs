using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    private RenderTexture targetTexture;

    [SerializeField] private string m_ID = Guid.NewGuid().ToString();

    //Getter Method
    public string id => m_ID;

    public RenderTexture TargetTexture { get; internal set; }

    [ContextMenu("Generate new ID")]
    private void RegenerateGUID() => m_ID = Guid.NewGuid().ToString();

    public Whiteboard(string idargs)
    {
        m_ID = idargs;
    }

    void Start()
    {
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
    }

}


