using UnityEngine;
using TMPro;

public class ColorSwapper : MonoBehaviour
{
    public Color[] colors;
    public TMP_Text[] text;

    void Start()
    {
        int index = 0;
        for (int i = 0; i < text.Length; i++)
        {
            string formattedText = "";
            for (int j = 0; j < text[i].text.Length; j++)
            {
                formattedText += "<color=#" + ColorUtility.ToHtmlStringRGBA(colors[index++ % colors.Length]) + ">" + text[i].text[j] + "</color>";
            }
            text[i].text = formattedText;
        }
    }
}