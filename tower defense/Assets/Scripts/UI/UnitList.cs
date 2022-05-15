using UnityEngine;

public abstract class UnitList : MonoBehaviour
{
    Player _player;
    [SerializeField] protected UnitCard cardTemplate;

    protected GameObject CreateCard(Player.UnitContainer container)
    {
        GameObject unitObject = container.unitObject;
        GameObject clone = Instantiate(cardTemplate.gameObject, transform);
        cardTemplate.Load(unitObject);
        return clone;
    }
    
    protected virtual void InitCard(Player.UnitContainer container)
    {

    }
    
    void Start()
    {
        _player = Player.Instance;
        _player.units.ForEach(InitCard);
    }
}
