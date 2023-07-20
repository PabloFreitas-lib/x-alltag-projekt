using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace InfoGamerHubAssets
{

/// <summary>
/// This class represents a UI color pick button that allows users to select colors from a color chart.
/// </summary>
/// <author> Maya, Dmitry, Collin, (Buesra) </author>

    public class UIColorPickButton : MonoBehaviour
    {

        
        public UnityEvent<Color> ColorPickerEvent;

        public Texture2D colorChart;
        public RectTransform colorChartRect;

        public RectTransform cursor;

        public XRRayInteractor rayinteractor;
        public XRSimpleInteractable interactable;

        public Color newcolor;
        [SerializeField] Image button;
        [SerializeField] Image cursorColor;

        public int xcol = 0; // The x-coordinate of the color chart.
        public int ycol = 0; // The y-coordinate of the color chart.
        
        
        /// <summary>
        ///Picks a color based on the cursor position on the color chart and invokes the ColorPickerEvent.
        /// </summary>
        /// <author> Dmitry </author>
        public void PickColor()
        {
            // Only if an interactor has been found, the code can work -> is automatically added.
            if(rayinteractor == null) 
            {
                return; 
            }
            
            // where does this interactor go with the ray? give me the hit point (UI has a collider now)
            // Cast a ray from the cursor to the color chart to find the hit point.
            rayinteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit); 
            
            // use the HitPoint to set the position of the cursor.
            // Update the cursor position based on the hit point and clamp it within a range
            cursor.SetPositionAndRotation(new Vector3(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z), cursor.rotation); 
            
            
            // A bit of vector math with the local position of the cursor.
            cursor.SetLocalPositionAndRotation(new Vector3(Math.Clamp(cursor.localPosition.x, -10, 10), Math.Clamp(cursor.localPosition.y, -10, 10), cursor.localPosition.z),cursor.localRotation); 
            
            
            //Determine the color of the pixel at the cursor position on the color chart.
            Color pickedColor = colorChart.GetPixel((int) ((cursor.localPosition.x + 10) * (colorChart.width / colorChartRect.rect.width)), (int)((cursor.localPosition.y + 10) * (colorChart.height / colorChartRect.rect.height))); 
            newcolor = pickedColor; 

            Debug.Log(pickedColor);
            // Update the color of the UI button to the picked color.
            button.color = pickedColor;
            
            // Update the cursor color to the picked color.
            cursorColor.color = pickedColor;
            
            // Trigger the ColorPickerEvent with the picked color.
            ColorPickerEvent.Invoke(pickedColor);
        }
        
       /// <summary>
        /// Called when the button is started, sets up the interaction events for color picking.
        /// </summary>
        /// <author> Dmitry </author>
        private void Start()
        {
            // Get the XR simple interactable component.
            interactable = GetComponent<XRSimpleInteractable>(); 
            
            // Register the OnSelectEntered method to be called when the button is selected.
            interactable.selectEntered.AddListener(OnSelectEntered);
        }
        
        /// <summary>
        /// Called when the button is destroyed, cleans up the interaction events.
        /// </summary>
        private void OnDestroy()
        {
            // Unregister the OnSelectEntered method when the button is destroyed.
            interactable.selectEntered.RemoveListener(OnSelectEntered);// XR stuff
        }

        /// <summary>
        /// Called when the button is selected, sets the rayinteractor used for color picking.
        /// </summary>
        /// <author> Dmitry </author>
        /// <param name="args"> The event data for the select enter event. </param>
        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            // Get the XR ray interactor used to select the UI button.
            rayinteractor = (XRRayInteractor)args.interactor;
        }
    }
}