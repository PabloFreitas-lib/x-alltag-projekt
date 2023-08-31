using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NodesToPrompt : MonoBehaviour
{

    //mindmap muss selected werden ggf als constructor argument ? 
    //hab ich iwo in whiteboard gemacht

    //prompt for image generation
    public string prompt;
    //muss die länge festgelegt werden iwo ? 

    //mindmap given as argument ? or by findobject ? 
    public Mindmap mindmap;

    //Gameobject that was fileSocket script as component
    public GameObject objectFileSocket;

   

    //basic version just take all label entrys of mindmap and add them together
    void connectPromptTexts()
    {
        if (objectFileSocket != null)
        {
            FileSocket fileSocket = objectFileSocket.GetComponent<FileSocket>();
            if (fileSocket != null)
            {
                mindmap = fileSocket.map;


                for (int i = 0; i < mindmap.nodes.Count; i++)
                {
                    prompt = prompt + mindmap.nodes[i] + " ";
                }
                Console.WriteLine(prompt);
                UnityEngine.Debug.Log("ergebnis:" + prompt);


            }
            else
            {
                UnityEngine.Debug.LogError("Script wasnt found on gameobject");
            }
        }
        else 
        {
            UnityEngine.Debug.LogError("Gameobject wasnt found");
        }
    }

    void startStableDiffusion()
    {
        Process stableDiffusion = new Process();

        stableDiffusion.StartInfo.FileName = "C:\\USERS\\ml\\stable-diffusion-webui\\webui-user.bat";
        
        stableDiffusion.Start();
    }

    // Start is called before the first frame update
    void Start()
    {
        startStableDiffusion();
        connectPromptTexts();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
