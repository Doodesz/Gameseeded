using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    private event Action action = delegate { };

    public void Trigger()
    {
        action?.Invoke();
    }

    public void Add(Action subscriber)
    {
        action += subscriber;
    }

    public void Remove(Action subscriber)
    {
        action -= subscriber;
    }
}

public class GameEvent<T>
{
    private event Action<T> action = delegate { };

    public void Publish(T param)
    {
        action?.Invoke(param);
    }

    public void Add(Action<T> subscriber)
    {
        action += subscriber;
    }

    public void Remove(Action<T> subscriber)
    {
        action -= subscriber;
    }
}
public partial class Events
{
    public static readonly GameEvent<float> onChangeSubmit = new();
    public static readonly GameEvent onGetNextCustomer = new();
    public static readonly GameEvent onCustomerCome = new();
    public static readonly GameEvent onCustomerLeave = new();
    public static readonly GameEvent onSelectNewInteractable = new();
    public static readonly GameEvent onDialogueStart = new();

    // public static readonly GameEvent onChangeSubmit = new();
    // public static readonly GameEvent<GameObject> onPickup = new();
}