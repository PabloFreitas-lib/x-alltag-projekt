using UnityEngine;

public class CanvasHider : MonoBehaviour
{
    public Transform player; // Referenz auf den Spieler oder ein anderes GameObject
    public float hideDistance = 1f; // Abstand, ab dem das Canvas versteckt wird
    public Canvas canvas; // Referenz auf das zu versteckende Canvas

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        // Überprüfe, ob der Abstand den Schwellenwert überschreitet
        if (distance >= hideDistance)
        {
            // Verstecke das Canvas
            canvas.enabled = false;
        }
        else
        {
            // Zeige das Canvas an
            canvas.enabled = true;
        }
    }
}
