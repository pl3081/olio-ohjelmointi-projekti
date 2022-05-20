using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeployList : UnitList
{
    [SerializeField] UnitPlacementSystem placementSystem;

    protected override void InitCard(Player.UnitContainer container)
    {
        GameObject clone = base.CreateCard(container);
        var button = clone.GetComponent<Button>();
        button.onClick.AddListener(() => 
        {
            placementSystem.SetTarget(container.unitObject);
        });
    }
}
