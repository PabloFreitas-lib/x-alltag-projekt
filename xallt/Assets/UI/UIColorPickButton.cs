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

            //int x = (int)(cursor.localPosition.x * (colorChart.width / transform.GetChild(0).GetComponent<RectTransform>().rect.width));
            // int y = (int)(cursor.localPosition.y * (colorChart.height / transform.GetChild(0).GetComponent<RectTransform>().rect.height));
            //Debug.Log("X:\t" +  x + "\ty:\t" +
            //  y);
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
