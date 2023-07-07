using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace InfoGamerHubAssets
{


    public class UIColorPickButton : MonoBehaviour
    {
        // Start is called before the first frame update
        public UnityEvent<Color> ColorPickerEvent;

        public Texture2D colorChart;
        public RectTransform colorChartRect;

        public RectTransform cursor;

        public XRRayInteractor rayinteractor;
        public XRSimpleInteractable interactable;

        public Color newcolor;
        [SerializeField] Image button;
        [SerializeField] Image cursorColor;

        public int xcol = 0;
        public int ycol = 0;

        public void PickColor()
        {
            if(rayinteractor == null) // nur wenn ein Interactor gefunden wurde, kann der Code arbeiten -> wird automatisch hinzugefügt.
            {
                return; 
            }
            rayinteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit); // Wo trifft dieser Interactor mit dem ray hin? gib mir den hit Point (UI hat ein Collider jetzt)
            
            cursor.SetPositionAndRotation(new Vector3(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z), cursor.rotation); // Benutze den HitPoint um die Position des Cursors festzulegen.
            cursor.SetLocalPositionAndRotation(new Vector3(Math.Clamp(cursor.localPosition.x, -10, 10), Math.Clamp(cursor.localPosition.y, -10, 10), cursor.localPosition.z),cursor.localRotation); // Ein bisschen Vektormathematik mit der lokalen Position des Cursors.
            Color pickedColor = colorChart.GetPixel((int) ((cursor.localPosition.x + 10) * (colorChart.width / colorChartRect.rect.width)), (int)((cursor.localPosition.y + 10) * (colorChart.height / colorChartRect.rect.height))); // Ein bisschen mehr Vektormathematik, um die Farbe zu bestimmen
            newcolor = pickedColor; // Na endlich, wir haben eine Farbe gefunden!

            Debug.Log(pickedColor);
            button.color = pickedColor;
            cursorColor.color = pickedColor;
            ColorPickerEvent.Invoke(pickedColor);
        }
        private void Start()
        {
            interactable = GetComponent<XRSimpleInteractable>(); // XR stuff
            interactable.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDestroy()
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);// XR stuff
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            rayinteractor = (XRRayInteractor) args.interactor; // Gib mir doch mal bitte den Interactor, den du benutzt hast, um auf die UI zu klicken. Dann kann auch ColorPicker funktionieren.
        }
    }
}