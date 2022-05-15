using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuyList : UnitList
{
    [SerializeField] UnitViewer viewer;

    protected override void InitCard(Player.UnitContainer container)
    {
        GameObject clone = base.CreateCard(container);
        var button = clone.GetComponent<Button>();
        button.onClick.AddListener(() => 
        {
            viewer.Buy(container);
        });
        
        var eventType = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        eventType.callback.AddListener((eventData) => { viewer.Target = clone.GetComponent<Unit>(); });
        
        EventTrigger trigger = clone.AddComponent<EventTrigger>();
        trigger.triggers.Add(eventType);
    }
}
