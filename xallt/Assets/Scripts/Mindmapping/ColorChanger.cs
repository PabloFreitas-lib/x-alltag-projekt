using UnityEngine;

[ExecuteAlways]
public class ColorChanger : MonoBehaviour
{
    public Color objectColor; // Öffentliche Variable für die Würfelfarbe im Inspector
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

    private void UpdateCubeColor()
    {
        if (!isHighlighted)
        {
            if (cubeMaterial.GetColor("_Color") != objectColor && cubeMaterial.GetColor("_EmissionColor") != objectColor)
            {
                cubeMaterial.SetColor("_Color", objectColor); // Ändert die Hauptfarbe des Materials
                cubeMaterial.SetColor("_EmissionColor", objectColor); // Ändert die Emissionsfarbe des Materials
                cubeRenderer.sharedMaterial.EnableKeyword("_EMISSION"); // Aktiviert die Emission im Material
            }

            if (pointLight != null && pointLight.color != objectColor)
            {
                pointLight.color = objectColor; // Setzt die Farbe des Point Lights entsprechend der Würfelfarbe
            }
        }
        else
        {
            if (cubeMaterial.GetColor("_Color") != highLightColor && cubeMaterial.GetColor("_EmissionColor") != highLightColor)
            {
                cubeMaterial.SetColor("_Color", highLightColor); // Ändert die Hauptfarbe des Materials
                cubeMaterial.SetColor("_EmissionColor", highLightColor); // Ändert die Emissionsfarbe des Materials
                cubeRenderer.sharedMaterial.EnableKeyword("_EMISSION"); // Aktiviert die Emission im Material
            }

            if (pointLight != null && pointLight.color != highLightColor)
            {
                pointLight.color = highLightColor; // Setzt die Farbe des Point Lights entsprechend der Würfelfarbe
            }
        }
        
    }

    public void HighlightObject(bool highlight)
    {
        isHighlighted = highlight;
    }

    public void SetColor(Color color)
    {
        objectColor = color;
    }
}
