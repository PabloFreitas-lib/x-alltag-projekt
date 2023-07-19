using UnityEngine;

/// <summary>
/// changes color of nodes and cube
/// </summary>
/// <author> Dmitry, Mert, Mailin </author>

[ExecuteAlways]
public class ColorChanger : MonoBehaviour
{
    // public variables for the cube color in inspector
    public Color objectColor; // �ffentliche Variable f�r die W�rfelfarbe im Inspector
    public Color highLightColor;
    public bool isHighlighted;
    public Light pointLight; // Referenz auf das Point Light

    private Renderer cubeRenderer;
    private Material cubeMaterial;

    private void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        cubeMaterial = new Material(cubeRenderer.sharedMaterial);
        cubeRenderer.material = cubeMaterial;
        highLightColor = objectColor * objectColor;
        UpdateCubeColor();
    }

    private void Update()
    {
        UpdateCubeColor();
    }

    /// <summary>
    /// changes cube color
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    private void UpdateCubeColor()
    {
        if (!isHighlighted)
        {
            if (cubeMaterial.GetColor("_Color") != objectColor && cubeMaterial.GetColor("_EmissionColor") != objectColor)
            {
                cubeMaterial.SetColor("_Color", objectColor); // �ndert die Hauptfarbe des Materials
                cubeMaterial.SetColor("_EmissionColor", objectColor); // �ndert die Emissionsfarbe des Materials
                cubeRenderer.sharedMaterial.EnableKeyword("_EMISSION"); // Aktiviert die Emission im Material
            }

            if (pointLight != null && pointLight.color != objectColor)
            {
                pointLight.color = objectColor; // Setzt die Farbe des Point Lights entsprechend der W�rfelfarbe
            }
        }
        else
        {
            if (cubeMaterial.GetColor("_Color") != highLightColor && cubeMaterial.GetColor("_EmissionColor") != highLightColor)
            {
                cubeMaterial.SetColor("_Color", highLightColor); // �ndert die Hauptfarbe des Materials
                cubeMaterial.SetColor("_EmissionColor", highLightColor); // �ndert die Emissionsfarbe des Materials
                cubeRenderer.sharedMaterial.EnableKeyword("_EMISSION"); // Aktiviert die Emission im Material
            }

            if (pointLight != null && pointLight.color != highLightColor)
            {
                pointLight.color = highLightColor; // Setzt die Farbe des Point Lights entsprechend der W�rfelfarbe
            }
        }
        
    }

    /// <summary>
    /// highlights selected nodes
    /// </summary
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="bool highlight"> needs to know if node is highlighted </param>
    public void HighlightObject(bool highlight)
    {
        isHighlighted = highlight;
    }

    /// <summary>
    /// sets node color
    /// </summary>
    /// <author> Dmitry, Mert, Mailin </author>
    /// <param name="Color color"> requires a color </param>
    public void SetColor(Color color)
    {
        objectColor = color;
    }
}
