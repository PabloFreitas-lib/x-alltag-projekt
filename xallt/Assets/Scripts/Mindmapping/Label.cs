using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using System;

/// <summary>
/// Class for the labels on mindmap nodes.
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>
public class Label : MonoBehaviour
{
    public TextMeshProUGUI inputText;
    public Text text;
    public Node node;
    private NonNativeKeyboard nonNativeKeyboard;
    private LabelManager labelManager;

    private void Start()
    {
        node = GetComponent<Node>();
        inputText = GameObject.FindGameObjectWithTag("Keyboard").GetComponent<TextMeshProUGUI>();
        nonNativeKeyboard = FindObjectOfType<NonNativeKeyboard>();


        if (nonNativeKeyboard != null)
        {
            labelManager = nonNativeKeyboard.GetComponent<LabelManager>();
            nonNativeKeyboard.OnTextSubmitted += OnTextSubmitted;
        }
    }

    /// <summary>
    /// Label selects himself.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    public void SelectSelf()
    {
        if(labelManager != null && inputText != null)
        {
            labelManager.currentLabel = this;
            inputText.text = text.text;
        }
    }

    /// <summary>
    /// Changes label from input text.
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="object sender"> sender </param>
    private void OnTextSubmitted(object sender, EventArgs e)
    {
        if (labelManager.currentLabel != null)
        {
            if (labelManager.currentLabel == this)
            {
                text.text = inputText.text;
            }
        }
    }

    private void OnDestroy()
    {
        if (nonNativeKeyboard != null)
        {
            nonNativeKeyboard.OnTextSubmitted -= OnTextSubmitted;
        }
    }
}
