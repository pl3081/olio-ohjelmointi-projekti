using UnityEngine;
using UnityEngine.UI;

public class UnitViewer : MonoBehaviour
{
    [SerializeField] Text unitName;
    [SerializeField] Text cost;
    [SerializeField] Text moneyText;
    Unit target;
    Player player;

    public Unit Target
    {
        get => target;
        set
        {
            target = value;
            if (value == null)
            {
                unitName.text = "";
                cost.text = "";
            }
            else
            {
                unitName.text = value.name;
                cost.text = "Price: " + value.Cost;
            }
        }
    }

    void UpdateMoneyText()
    {
        moneyText.text = "$" + Player.Instance.money;
    }

    void Start()
    {
        UpdateMoneyText();
        player = Player.Instance;
    }
    
    public void Buy(Player.UnitContainer toBuy)
    {
        player.BuyUnit(toBuy);
        UpdateMoneyText();
    }
}
