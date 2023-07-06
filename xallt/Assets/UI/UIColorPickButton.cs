using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace InfoGamerHubAssets
{


    public class UIColorPickButton : MonoBehaviour
    {
        // Start is called before the first frame update
        public UnityEvent<Color> ColorPickerEvent;

        public Texture2D colorChart;
        public RectTransform colorChartRect;

        public RectTransform cursor;

        public Color newcolor;
        [SerializeField] Image button;
        [SerializeField] Image cursorColor;

        public void PickColor(BaseEventData data)
        {
            PointerEventData pointer = data as PointerEventData;

            cursor.position = pointer.position;

            Vector2 cursorRealPosition = new Vector2(colorChartRect.rect.width / 2 + cursor.localPosition.x, colorChartRect.rect.height / 2 + cursor.localPosition.y);

            Color pickedColor = colorChart.GetPixel((int)(cursorRealPosition.x * (colorChart.width / colorChartRect.rect.width)), (int)(cursorRealPosition.y * (colorChart.height / colorChartRect.rect.height)));
            newcolor = pickedColor;

            Debug.Log(pickedColor);
            button.color = pickedColor;
            cursorColor.color = pickedColor;
            ColorPickerEvent.Invoke(pickedColor);
        }
    }
}