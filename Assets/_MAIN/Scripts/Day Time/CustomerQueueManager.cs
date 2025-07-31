using DialogueEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CustomerQueueManager : MonoBehaviour
{
    public enum CurrentCashierStatus { Vacant, Occcupied };
    public CurrentCashierStatus status;

    public static CustomerQueueManager Instance;

    [Header("Parameters")]
    public List<GameObject> customerQueueList = new List<GameObject>();
    [SerializeField] private GameObject customerSpawnPoint;

    [Header("Debugging")]
    [SerializeField] GameObject currentCustomer;
    Queue<GameObject> customerQueue;
    Queue<GameObject> customerWaitingToSpawn;

    private void OnEnable()
    {
        Events.onCustomerCome.Add(OnCustomerCome);
        Events.onChangeSubmit.Add(OnChangeSubmit);
    }
    private void OnDisable()
    {
        Events.onCustomerCome.Remove(OnCustomerCome);
        Events.onChangeSubmit.Remove(OnChangeSubmit);
    }

    private void Start()
    {
        if (Instance != null) Destroy(this);
        Instance = this;

        customerWaitingToSpawn = new Queue<GameObject>(customerQueueList);
        customerQueue = new Queue<GameObject>();

        SpawnCustomer();

        currentCustomer = customerQueue.Peek();

        InvokeRepeating("SpawnCustomer", 5f, 5f);
    }

    void SpawnCustomer()
    {
        if (customerWaitingToSpawn.Count > 0)
        {
            GameObject nextCustomerPrefab = customerWaitingToSpawn.Peek();
            customerQueue.Enqueue(Instantiate(nextCustomerPrefab, customerSpawnPoint.transform.position, customerSpawnPoint.transform.rotation));

            customerWaitingToSpawn.Dequeue();
        }
        else
        {
            customerWaitingToSpawn.Clear();
        }
    }

    void OnCustomerCome()
    {
        status = CurrentCashierStatus.Occcupied;
    }

    void OnChangeSubmit()
    {
        GetNextCustomer();
    }

    void GetNextCustomer()
    {
        status = CurrentCashierStatus.Vacant;

        Destroy(currentCustomer);

        customerQueue.Dequeue();

        if (customerQueue.Peek() != null)
        {
            currentCustomer = customerQueue.Peek();
            Events.onGetNextCustomer.Trigger();
        }
        else
        {
            Debug.Log("Day ended!");
            // Events.onLastCustomerDone.Trigger();
        }
    }

    public GameObject GetCurrentCustomer()
    {
        return currentCustomer;
    }
}
