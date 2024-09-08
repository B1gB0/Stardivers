using System;
using System.Collections;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

public class MoneyStorage : MonoBehaviour
{
    public event Action<int> OnStateChanged;
    
    public int Money { get; private set; }

    public void SetMoney(int money)
    {
        Money = money;
        OnStateChanged?.Invoke(Money);
    }

    public void AddMoney(int money)
    {
        Money += money;
        OnStateChanged?.Invoke(Money);
    }

    public void SpendMoney(int money)
    {
        Money -= money;
        OnStateChanged?.Invoke(Money);
    }
}
