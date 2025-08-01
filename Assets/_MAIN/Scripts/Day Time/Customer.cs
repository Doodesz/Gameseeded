using DialogueEditor;
using UnityEngine;
using System.Collections.Generic;

public enum CustomerType { BuyOnly, TalkOnly, TalkNBuy };
[RequireComponent(typeof(Interactable))]
public class Customer : MonoBehaviour
{
    [Header("Parameters")]
    // public List<GameObject> books;
    public int change;
    public CustomerType customerType;
    [Space(10f)]
    public GameObject book1Prefab;
    public GameObject book2Prefab;
    public GameObject book3Prefab;

    [Header("References")]
    public NPCConversation conversation;
    public Animator animator;
    public Animator avatarAnimator;
    [SerializeField] GameObject booksContainer;
    [SerializeField] Collider interactCollider;
    [SerializeField] Outline outline;
    [Space(10f)]
    [SerializeField] GameObject book1PositionPoint;
    [SerializeField] GameObject book2PositionPoint;
    [SerializeField] GameObject book3PositionPoint;

    [Header("Debugging")]
    [SerializeField] private bool isCheckingOut;

    private void OnEnable()
    {
        Events.onCustomerCome.Add(OnCustomerCome);
        Events.onGetNextCustomer.Add(OnGetNextCustomer);

        //ConversationManager.OnConversationEnded += OnConversationEnded;
    }
    private void OnDisable()
    {
        Events.onCustomerCome.Remove(OnCustomerCome);
        Events.onGetNextCustomer.Remove(OnGetNextCustomer);

        //ConversationManager.OnConversationEnded -= OnConversationEnded;
    }

    void Start()
    {
        if (!TryGetComponent<Interactable>(out Interactable interactable))
            Debug.LogError("ERROR: GameObject " + gameObject.name + " has no Interactable script!");

        isCheckingOut = false;

        if (CustomerQueueManager.Instance.GetCurrentCustomer() == gameObject)
            animator.SetBool("isCheckingOut", true);

        if(book1Prefab != null)
            Instantiate(book1Prefab, book1PositionPoint.transform);
        if (book2Prefab != null)
            Instantiate(book2Prefab, book2PositionPoint.transform);
        if (book3Prefab != null)
            Instantiate(book3Prefab, book3PositionPoint.transform);

        EnableInteraction(false);

        booksContainer.SetActive(false);
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting") || animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting Checkout"))
            avatarAnimator.SetBool("isWalking", false);
        else
            avatarAnimator.SetBool("isWalking", true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Checkout") && !booksContainer.activeSelf)
            booksContainer.SetActive(true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting Checkout") && !isCheckingOut)
        {
            Events.onCustomerCome.Trigger();
            isCheckingOut = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Exit"))
            Destroy(gameObject);
    }

    void OnCustomerCome()
    {
        booksContainer.SetActive(false);

        if (CustomerQueueManager.Instance.GetCurrentCustomer() == gameObject)
        {
            EnableInteraction(true);
            
            if (customerType == CustomerType.BuyOnly)
            {
                CashManager.Instance.UpdateCashRegisterChangeDisplay(change.ToString());
                CashManager.Instance.currentChangeNeeded = change;
            }
            else
            {
                // hint player using outline to interact
            }
        }
    }

    void OnGetNextCustomer()
    {
        if(CustomerQueueManager.Instance.GetCurrentCustomer() == gameObject)
        {
            animator.SetBool("isCheckingOut", true);
        }
    }

    public void EnableInteraction(bool toggle)
    {
        interactCollider.enabled = toggle;

        if (!toggle) outline.enabled = false; // Fix bug
            
    }

    // These 3 gets called in dialogue
    public void AdjustTrust(int amount)
    {
        BookstoreStats.Instance.AdjustTrust(amount);
    }
    public void AdjustMoney(int amount)
    {
        BookstoreStats.Instance.AdjustMoney(amount);
    }
    public void AdjustStock(int amount)
    {
        BookstoreStats.Instance.AdjustStock(amount);
    }
}
