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


        public Color newcolor;
        [SerializeField] Image button;
        [SerializeField] Image cursorColor;

        public int xcol = 0;
        public int ycol = 0;

        public void PickColor(BaseEventData data)
        {
            PointerEventData pointer = data as PointerEventData;

            cursor.position = pointer.position;

            rayinteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(colorChartRect, raycastHit.point, Camera.main, out Vector2 localPoint);
            Debug.Log(localPoint.x);
            Debug.Log(localPoint.y);
            //Vector2 cursorRealPosition = new Vector2(colorChartRect.rect.width / 2 + cursor.localPosition.x, colorChartRect.rect.height / 2 + cursor.localPosition.y);

            //Color pickedColor = colorChart.GetPixel((int)(cursorRealPosition.x * (colorChart.width / colorChartRect.rect.width)), (int)(cursorRealPosition.y * (colorChart.height / colorChartRect.rect.height)));
            
            Color pickedColor = colorChart.GetPixel((int) (localPoint.x * (colorChart.width / colorChartRect.rect.width)), (int)( localPoint.y * (colorChart.height / colorChartRect.rect.height)));
            newcolor = pickedColor;

            //Debug.Log(pickedColor);
            button.color = pickedColor;
            cursorColor.color = pickedColor;
            ColorPickerEvent.Invoke(pickedColor);
        }
    }
}