using UnityEngine;

public class UnitList : MonoBehaviour
{
    Player player;
    [SerializeField] UnitViewer viewer;
    [SerializeField] GameObject cardTemplate;

    void InitCard(Player.UnitContainer container)
    {
        GameObject unitObject = container.unitObject;
        GameObject card = Instantiate(cardTemplate, transform);
        card.GetComponent<UnitCard>().Load(unitObject);
    }
    
    void Awake()
    {
        player = Player.Instance;
        player.units.ForEach(InitCard);
    }
}
