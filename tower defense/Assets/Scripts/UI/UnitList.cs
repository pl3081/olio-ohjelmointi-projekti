using UnityEngine;

public abstract class UnitList : MonoBehaviour
{
    Player _player;
    [SerializeField] protected UnitCard cardTemplate;

    protected GameObject CreateCard(Player.UnitContainer container)
    {
        GameObject clone = Instantiate(cardTemplate.gameObject, transform);
        clone.name = container.unitObject.name;
        clone.GetComponent<UnitCard>().Load(container.unitObject);
        return clone;
    }

    protected abstract void InitCard(Player.UnitContainer container);

    void Start()
    {
        _player = Player.Instance;
        _player.units.ForEach(InitCard);
    }
}
