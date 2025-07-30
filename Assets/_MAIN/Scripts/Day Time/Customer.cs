using DialogueEditor;
using UnityEngine;

public enum CustomerType { BuyNTalk, TalkOnly };
[RequireComponent(typeof(Interactable))]
public class Customer : MonoBehaviour
{
    [Header("References")]
    public NPCConversation conversation;

    [Header("Parameters")]
    // public List<GameObject> books;
    public int change;
    public CustomerType customerType;

    private void OnEnable()
    {
        Events.onCustomerCome.Add(OnCustomerCome);
        Events.onCustomerLeave.Add(OnCustomerLeave);
        Events.onGetNextCustomer.Add(OnCustomerSpawn);
    }
    private void OnDisable()
    {
        Events.onCustomerCome.Remove(OnCustomerCome);
        Events.onCustomerLeave.Remove(OnCustomerLeave);
        Events.onGetNextCustomer.Remove(OnCustomerSpawn);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent<Interactable>(out Interactable interactable))
            Debug.LogError("ERROR: GameObject " + gameObject.name + " has no Interactable script!");
    }

    void OnCustomerSpawn()
    {
        // walk around first
    }

    void OnCustomerCome()
    {
        // put books on table

        CashManager.Instance.UpdateCashRegisterChangeDisplay(change.ToString());
    }

    void OnCustomerLeave()
    {
        // reset cash register display
    }
}
