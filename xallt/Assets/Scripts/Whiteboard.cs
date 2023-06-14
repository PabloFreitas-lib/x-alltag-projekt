using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    public string id { get; }


    public Whiteboard(string idargs)
    {
      id = idargs;
    } 

    void Start()
    {
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
    }

}
