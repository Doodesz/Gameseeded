using DialogueEditor;
using UnityEngine;
using System.Collections.Generic;

//[CreateAssetMenu(fileName = "Customer", menuName = "Scriptable Objects/Customer")]
public class CustomerQueueManager : MonoBehaviour
{
    [Header("Parameters")]
    public Queue<GameObject> customerQueueList = new Queue<GameObject>();
    private GameObject customerSpawnPoint;

    private void OnEnable()
    {
        Events.onGetNextCustomer.Add(GetNextCustomer);
    }
    private void OnDisable()
    {
        Events.onGetNextCustomer.Remove(GetNextCustomer);
    }

    private void Start()
    {
        Events.onGetNextCustomer.Trigger();
    }

    public void GetNextCustomer()
    {
        if (customerQueueList.Count > 0)
        {
            GameObject nextCustomerPrefab = customerQueueList.Peek();
            Instantiate(nextCustomerPrefab, customerSpawnPoint.transform.position, customerSpawnPoint.transform.rotation);

            customerQueueList.Dequeue();
        }
        else
        {
            Debug.LogError("ERROR: No content found in customerQueueList");
        }
    }
}
