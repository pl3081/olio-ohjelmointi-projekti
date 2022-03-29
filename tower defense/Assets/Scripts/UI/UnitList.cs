using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class UnitList : MonoBehaviour
{
    Player player;
    public GameObject cardTemplate;

    void InitCard(Player.UnitContainer container)
    {
        GameObject unitObject = container.unitObject;
        GameObject card = Instantiate(cardTemplate, transform);
        card.name = unitObject.name;
        var button = card.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate
        {
            player.BuyUnit(unitObject);
        });
    }
    
    void Awake()
    {
        player = Player.Instance;
        player.units.ForEach(InitCard);
    }
}
