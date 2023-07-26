using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Label : MonoBehaviour
{
    public TextMeshProUGUI inputText;
    public Text text;
    public Node node;

    private void Start()
    {
        node = GetComponent<Node>();
        inputText = GameObject.FindGameObjectWithTag("Keyboard").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (node.mindmap.selected != null)
        {
            if (node.mindmap.selected == node)
            {
                text.text = inputText.text;
            }
        }
        
    }
}
