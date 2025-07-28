using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    private event Action action = delegate { };

    public void Publish()
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
    // public static readonly GameEvent onChangeSubmit = new();
    // public static readonly GameEvent<GameObject> onPickup = new();
}