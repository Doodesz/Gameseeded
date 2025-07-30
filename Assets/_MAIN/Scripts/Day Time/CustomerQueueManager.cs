using DialogueEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//[CreateAssetMenu(fileName = "Customer", menuName = "Scriptable Objects/Customer")]
public class CustomerQueueManager : MonoBehaviour
{
    public enum CurrentCustomerStatus { None, Waiting };
    public CurrentCustomerStatus status;

    public static CustomerQueueManager Instance;

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

        if (Instance != null) Destroy(this);
        Instance = this;
    }

    public void GetNextCustomer()
    {
        if (customerQueue.Count > 0)
        {
            GameObject nextCustomerPrefab = customerQueue.Peek();
            StartCoroutine(InstantiateCustomerObject(nextCustomerPrefab));

            customerQueue.Dequeue();
        }
        else
        {
            Debug.LogError("ERROR: No content found in customerQueue");
        }

        status = CurrentCustomerStatus.None;
    }

    IEnumerator InstantiateCustomerObject(GameObject objectToInstantiate)
    {
        yield return new WaitForSeconds(3f);

        Instantiate(objectToInstantiate, customerSpawnPoint.transform.position, customerSpawnPoint.transform.rotation);
        Events.onCustomerCome.Trigger();
        status = CurrentCustomerStatus.Waiting;
    }
}
