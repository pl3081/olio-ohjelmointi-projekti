using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    [SerializeField] Text nameText;
    
    public void Load(GameObject unitObject)
    {
        nameText.text = unitObject.name;
    }
}
