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

    [Header("References")]
    [SerializeField] GameObject submitChangeButton;
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
    Customer currentCustomerComponent;
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

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;        
    }

    private void Start()
    {
        customerWaitingToSpawn = new Queue<GameObject>(customerQueueList);
        customerQueue = new Queue<GameObject>();

        SpawnCustomer();

        currentCustomer = customerQueue.Peek();
        currentCustomerComponent = currentCustomer.GetComponent<Customer>();

        InvokeRepeating("SpawnCustomer", 5f, 5f);

        submitChangeButton.SetActive(false);
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

        if (currentCustomerComponent.book1Prefab != null)
            book1OnTable = Instantiate(currentCustomerComponent.book1Prefab, book1OnTablePositionPoint.transform);
        if (currentCustomerComponent.book2Prefab != null)
            book2OnTable = Instantiate(currentCustomerComponent.book2Prefab, book2OnTablePositionPoint.transform);
        if (currentCustomerComponent.book3Prefab != null)
            book3OnTable = Instantiate(currentCustomerComponent.book3Prefab, book3OnTablePositionPoint.transform);

        if (currentCustomerComponent.customerType == CustomerType.TalkOnly)
            submitChangeButton.SetActive(false);
        else 
            submitChangeButton.SetActive(true);
    }

    void OnChangeSubmit()
    {
        currentCustomerComponent.FlagHasCheckedOut();
        
        if (currentCustomerComponent.customerType == CustomerType.BuyOnly)
        {
            currentCustomerComponent.EnableInteraction(false);
            GetNextCustomer();
            ClearTable();
            DayEndedBehaviour.Instance.AddCustomersServed(); // correct or wrong is called in CashManager.cs
        }
        else if (currentCustomerComponent.customerType == CustomerType.TalkNBuy && currentCustomerComponent.HasTalkedNHasCheckedOut())
        {
            currentCustomerComponent.EnableInteraction(false);
            GetNextCustomer();
            ClearTable();
            DayEndedBehaviour.Instance.AddCustomersServed(); // correct or wrong is called in CashManager.cs
            DayEndedBehaviour.Instance.AddCustomersTalked(); 
        }
        else Debug.LogError("ERROR: Invalid CustomerType for " + gameObject.name);
    }

    void OnConversationEnded()
    {
        currentCustomerComponent.FlagHasBeenTalkedTo();

        if (currentCustomerComponent.customerType == CustomerType.TalkOnly)
        {
            currentCustomerComponent.EnableInteraction(false);
            GetNextCustomer();
            ClearTable(); 
            DayEndedBehaviour.Instance.AddCustomersServed(); // correct or wrong is called in CashManager.cs
            DayEndedBehaviour.Instance.AddCustomersTalked();
        }
        else if (currentCustomerComponent.customerType == CustomerType.TalkNBuy && currentCustomerComponent.HasTalkedNHasCheckedOut())
        {
            currentCustomerComponent.EnableInteraction(false);
            GetNextCustomer();
            ClearTable();
            DayEndedBehaviour.Instance.AddCustomersServed(); // correct or wrong is called in CashManager.cs
            DayEndedBehaviour.Instance.AddCustomersTalked();
        }
        else Debug.LogError("ERROR: Invalid CustomerType for " + gameObject.name);
    }

    void GetNextCustomer()
    {
        cashierStatus = CurrentCashierStatus.Vacant;

        currentCustomer.GetComponent<Animator>().SetBool("isLeaving", true);

        customerQueue.Dequeue();

        if (customerQueue.TryPeek(out GameObject nextCustomer))
        {
            currentCustomer = nextCustomer;
            currentCustomerComponent = nextCustomer.GetComponent<Customer>();

            Events.onGetNextCustomer.Trigger();
        }
        else
        {
            Debug.Log("Day ended!");

            StartCoroutine(EndDay());
        }

        submitChangeButton.SetActive(false);
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

        book1OnTable = null;
        book2OnTable = null;
        book3OnTable = null;
    }

    IEnumerator EndDay()
    {
        yield return new WaitForSeconds(3f);
        Events.onDayEnded.Trigger();
    }
}
