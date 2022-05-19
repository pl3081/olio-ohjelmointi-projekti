using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyList : UnitList
{
    [SerializeField] UnitViewer viewer;

    protected override void InitCard(Player.UnitContainer container)
    {
        GameObject clone = CreateCard(container);
        print(container.unitObject.name);
        var button = clone.GetComponent<Button>();
        button.onClick.AddListener(() => 
        {
            viewer.Buy(container);
        });
        
        var eventType = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };

        Unit targetUnit = container.unitObject.GetComponent<Unit>();
        eventType.callback.AddListener((eventData) => { viewer.Target = targetUnit; });
        
        EventTrigger trigger = clone.AddComponent<EventTrigger>();
        trigger.triggers.Add(eventType);
    }
}
