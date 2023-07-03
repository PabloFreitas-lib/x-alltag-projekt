

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemoInfoGamerHubAssets
{





    public class DemoColorPicker : MonoBehaviour
    {
        public void SetColor(Color newColor)
        {
            GetComponent<MeshRenderer>().material.color = newColor;
            Debug.Log(newColor);
        }


    }
}
