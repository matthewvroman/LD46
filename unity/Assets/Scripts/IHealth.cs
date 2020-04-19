using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float MaxHealth { get; }
    float CurrentHealth  { get; }
    bool Dead  { get; }
    Vector3 HealthBarOffset { get; }
    Action OnDamaged { get; set; }
}
