using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    [SerializeField] KeyCode key;
    [SerializeField] GameObject target;
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            target.SetActive(!target.activeInHierarchy);
        }
    }
}
