using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodesToPrompt : MonoBehaviour
{

    //mindmap muss selected werden ggf als constructor argument ? 
    //hab ich iwo in whiteboard gemacht

    //prompt for image generation
    public string prompt;
    //muss die länge festgelegt werden iwo ? 

    //mindmap given as argument ? or by findobject ? 
    public Mindmap mindmap = new Mindmap();

    void NodestoPromt()
    {
        //findObject of Type -> get mindmap and select certain mindmap 
        mindmap = FindObjectOfType<Mindmap>().;
        
    }

    //basic version just take all label entrys of mindmap and add them together
    void connectPromptTexts()
    {
        for(int i= 0; i < mindmap.nodes.Count; i++)
        {
            prompt = prompt + mindmap.nodes[i] + " ";
        }
        Console.WriteLine(prompt);
        //texts[]
        //for (i < texts[].length){
        //prompt = prompt + texts[i]
        //oder was mit append
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
