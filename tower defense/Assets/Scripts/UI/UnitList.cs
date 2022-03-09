using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnitList : MonoBehaviour
{
    Player player;
    public GameObject cardTemplate;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            print("Is down");
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }
    
    void InitCard(GameObject unitObject)
    {
        GameObject card = Instantiate(cardTemplate, transform);
        card.name = unitObject.name;
        var unit = unitObject.GetComponent<Unit>();
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
        player.BuyableUnits.ForEach(InitCard);
    }
}
