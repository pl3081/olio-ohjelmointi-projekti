using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar
{
    GameObject Bar;
    GameObject background;
    Image BarImage;

    float maxValue;
    public float Value
    {
        get
        {
            return BarImage.fillAmount;
        }
        set
        {
            if(value >= maxValue)
            {
                BarImage.fillAmount = maxValue;
                background.SetActive(false);
            }
            else if(value <= 0)
            {
                BarImage.fillAmount = 0;
                background.SetActive(false);
            }
            else
            {
                BarImage.fillAmount = value / maxValue;
                background.SetActive(true);
            }
        }
    }
    public HealthBar(float maxValue)
    {
        this.maxValue = maxValue;

        background = new GameObject();
        Image bgImage = background.AddComponent<Image>();
        background.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 7);
        background.GetComponent<RectTransform>().SetParent(GameObject.Find("HPCanvas").transform);
        bgImage.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        bgImage.color = Color.black;
        background.SetActive(false);

        Bar = new GameObject();
        BarImage = Bar.AddComponent<Image>();
        Bar.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 5);
        Bar.GetComponent<RectTransform>().SetParent(background.transform);
        BarImage.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        BarImage.color = new Vector4(0.8f, 0.2f, 0.2f, 1f);
        BarImage.type = Image.Type.Filled;
        BarImage.fillMethod = Image.FillMethod.Horizontal;
    }
    public void Delete()
    {
        Object.Destroy(background);
    }
    public void SetPosition(Vector3 pos)
    {
        background.GetComponent<RectTransform>().position = pos;
    }
}
