using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GDEvent
{
    public static class EventManager
    {
        // Player
        public static UnityAction<Component, int> OnHealthChanged;
    }
}
