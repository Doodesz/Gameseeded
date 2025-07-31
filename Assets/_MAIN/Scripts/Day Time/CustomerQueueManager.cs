using DialogueEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CustomerQueueManager : MonoBehaviour
{
    public enum CurrentCashierStatus { Vacant, Occcupied };
    public CurrentCashierStatus cashierStatus;

    public static CustomerQueueManager Instance;

    [Header("Parameters")]
    public List<GameObject> customerQueueList = new List<GameObject>();
    [Space(10f)]
    [SerializeField] GameObject customerSpawnPoint;
    [Space(10f)]
    [SerializeField] GameObject book1OnTablePositionPoint;
    [SerializeField] GameObject book2OnTablePositionPoint;
    [SerializeField] GameObject book3OnTablePositionPoint;
    [Space(10f)]
    [SerializeField] GameObject book1OnTable;
    [SerializeField] GameObject book2OnTable;
    [SerializeField] GameObject book3OnTable;

    [Header("Debugging")]
    [SerializeField] GameObject currentCustomer;
    Queue<GameObject> customerQueue;
    Queue<GameObject> customerWaitingToSpawn;

    private void OnEnable()
    {
        Events.onCustomerCome.Add(OnCustomerCome);
        Events.onChangeSubmit.Add(OnChangeSubmit);

        ConversationManager.OnConversationEnded += OnConversationEnded;
    }
    private void OnDisable()
    {
        Events.onCustomerCome.Remove(OnCustomerCome);
        Events.onChangeSubmit.Remove(OnChangeSubmit);

        ConversationManager.OnConversationEnded -= OnConversationEnded;
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
        cashierStatus = CurrentCashierStatus.Occcupied;

        book1OnTable = Instantiate(currentCustomer.GetComponent<Customer>().book1Prefab, book1OnTablePositionPoint.transform);
        book2OnTable = Instantiate(currentCustomer.GetComponent<Customer>().book2Prefab, book2OnTablePositionPoint.transform);
        book3OnTable = Instantiate(currentCustomer.GetComponent<Customer>().book3Prefab, book3OnTablePositionPoint.transform);
    }

    void OnChangeSubmit()
    {
        currentCustomer.GetComponent<Customer>().EnableInteraction(false);
        GetNextCustomer();
        ClearTable();
    }

    void OnConversationEnded()
    {
        if (currentCustomer.GetComponent<Customer>().customerType == CustomerType.TalkOnly)
        {
            currentCustomer.GetComponent<Customer>().EnableInteraction(false);
            GetNextCustomer();
            ClearTable();
        }
    }

    void GetNextCustomer()
    {
        cashierStatus = CurrentCashierStatus.Vacant;

        currentCustomer.GetComponent<Animator>().SetBool("isLeaving", true);

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

    void ClearTable()
    {
        Destroy(book1OnTable);
        Destroy(book2OnTable);
        Destroy(book3OnTable);
    }
}
