using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "Scriptable Objects/Day")]
public class Day : ScriptableObject
{
    [System.Serializable]
    public class CustomerList
    {
        public List<GameObject> customerList;
    }

    public CustomerList customersInDay;
}
