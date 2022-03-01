using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Humanoid
{
    [SerializeField] int cost;
    public int Cost => cost;
}