using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StartStableDiffCMD : MonoBehaviour
{

    /// <summary>
    /// Starting webui-user.bat through a script. 
    ///The bat is necessary for all stable diffusion generation proccesses
    /// </summary>
    void startStableDiffusion()
    {
        Process stableDiffusion = new Process();

        //Path should be the home path eventhen just works for windows now 
        stableDiffusion.StartInfo.FileName = "C:\\USERS\\ml\\stable-diffusion-webui\\webui-user.bat";

        stableDiffusion.Start();
    }
    // Start is called before the first frame update
    void Start()
    {
        startStableDiffusion();
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
