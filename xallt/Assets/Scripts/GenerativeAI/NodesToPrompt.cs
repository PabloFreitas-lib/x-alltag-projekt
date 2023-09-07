using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NodesToPrompt : MonoBehaviour
{

    

    //prompt for image generation
    public string prompt;
    //muss die länge festgelegt werden iwo ? 

    //mindmap given as argument ? or by findobject ? 
    public Mindmap mindmap;

    //Gameobject that was fileSocket script as component
    public GameObject objectFileSocket;

   //was soll objectFileSocket hier überhaupt sein ? 

    /// <summary>
    /// Function is called through a button on hand UI
    /// Checks whether FileSocket exists, checks if a mindmap is open (saved in fileSocket)
    /// If Mindmap is open connects all nodes to a string prompt and sets it to the prompt in textToMaterial.cs
    /// </summary>
    /// <author>
    /// Noah Horn
    /// </author>
    
    public void connectPromptTexts()
    {
        FileSocket fileSocket = GameObject.Find("File-Socket").GetComponent<FileSocket>();
            if (fileSocket != null)
            {
                if (fileSocket.socketFile.isOpen)
                {
                    mindmap = fileSocket.map;


                    for (int i = 0; i < mindmap.nodes.Count; i++)
                    {
                        prompt = prompt + mindmap.nodes[i] + " ";
                    }
                    Console.WriteLine(prompt);
                    UnityEngine.Debug.Log("ergebnis:" + prompt);

                    if (!string.IsNullOrEmpty(prompt))
                    {
                    setPromptStartGenerate();
                    }
                    else
                    {
                        UnityEngine.Debug.LogError("Prompt is empty");
                    }
                }
                else
                {
                    UnityEngine.Debug.LogError("Keine Mindmap aktiv. FileCube auf FileSocket ziehen");
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Script or Gameobject wasnt found");
            }
        
    }


    /// <summary>
    /// sets prompt on StableDiffusionImage2Material instance on ImageKICube gameobject and starts the generation of the Image 
    /// </summary>
    /// <author>Noah Horn</author>
    public void setPromptStartGenerate()
    {
        //get class instance from gameobject ImageKICUbe 
       StableDiffusionImage2Material stableSkript = GameObject.Find("ImageKICube").GetComponent<StableDiffusionImage2Material>();
           
      if (stableSkript != null)
      {
        stableSkript.prompt = prompt;
        stableSkript.Generate();
      }
      else
      {
        UnityEngine.Debug.LogError("ImageKICube GameObject or StableDiffusionImage2Material-component not found.");
      }

    }
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
