using UnityEngine;

[ExecuteAlways]
public class ColorChanger : MonoBehaviour
{
    public Color cubeColor; // �ffentliche Variable f�r die W�rfelfarbe im Inspector
    public Light pointLight; // Referenz auf das Point Light

    private Renderer cubeRenderer;
    private Material cubeMaterial;

    private void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        cubeMaterial = new Material(cubeRenderer.sharedMaterial);
        cubeRenderer.material = cubeMaterial;

        UpdateCubeColor();
    }

    private void Update()
    {
        UpdateCubeColor();
    }

    private void UpdateCubeColor()
    {
        if (cubeMaterial.GetColor("_Color") != cubeColor && cubeMaterial.GetColor("_EmissionColor") != cubeColor)
        {
            cubeMaterial.SetColor("_Color", cubeColor); // �ndert die Hauptfarbe des Materials
            cubeMaterial.SetColor("_EmissionColor", cubeColor); // �ndert die Emissionsfarbe des Materials
            cubeRenderer.sharedMaterial.EnableKeyword("_EMISSION"); // Aktiviert die Emission im Material
        }

        if (pointLight != null && pointLight.color != cubeColor)
        {
            pointLight.color = cubeColor; // Setzt die Farbe des Point Lights entsprechend der W�rfelfarbe
        }
    }
}
