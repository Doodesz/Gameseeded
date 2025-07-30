using DialogueEditor;
using UnityEngine;
using System.Collections.Generic;

//[CreateAssetMenu(fileName = "Customer", menuName = "Scriptable Objects/Customer")]
public class CustomerQueueManager : MonoBehaviour
{
    [Header("Parameters")]
    public List<GameObject> customerQueueList = new List<GameObject>();
    [SerializeField] private GameObject customerSpawnPoint;

    public Queue<GameObject> customerQueue;

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
        customerQueue = new Queue<GameObject>(customerQueueList);

        Events.onGetNextCustomer.Trigger();
    }

    public void GetNextCustomer()
    {
        if (customerQueue.Count > 0)
        {
            GameObject nextCustomerPrefab = customerQueue.Peek();
            Instantiate(nextCustomerPrefab, customerSpawnPoint.transform.position, customerSpawnPoint.transform.rotation);

            customerQueue.Dequeue();
        }
        else
        {
            Debug.LogError("ERROR: No content found in customerQueue");
        }
    }
}
