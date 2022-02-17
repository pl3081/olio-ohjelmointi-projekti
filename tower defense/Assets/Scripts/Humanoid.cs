using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [SerializeField] int maxHP;
    public int HP { get; set; }
    public int MaxHP => maxHP;
}
