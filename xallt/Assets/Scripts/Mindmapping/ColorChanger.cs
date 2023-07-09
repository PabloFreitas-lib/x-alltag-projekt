using UnityEngine;

[ExecuteAlways]
public class ColorChanger : MonoBehaviour
{
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

    public void HighlightObject(bool highlight)
    {
        isHighlighted = highlight;
    }

    public void SetColor(Color color)
    {
        objectColor = color;
    }
}
