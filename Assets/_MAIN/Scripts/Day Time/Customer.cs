using DialogueEditor;
using UnityEngine;
using System.Collections.Generic;

public enum CustomerType { BuyNTalk, TalkOnly };
[RequireComponent(typeof(Interactable))]
public class Customer : MonoBehaviour
{
    [Header("References")]
    public NPCConversation conversation;
    public Animator animator;
    public Animator avatarAnimator;
    [SerializeField] GameObject booksContainer;
    [SerializeField] Collider interactCollider;
    [Space(20f)]
    public GameObject book1Prefab;
    public GameObject book2Prefab;
    public GameObject book3Prefab;
    [Space(10f)]
    [SerializeField] GameObject book1PositionPoint;
    [SerializeField] GameObject book2PositionPoint;
    [SerializeField] GameObject book3PositionPoint;

    [Header("Parameters")]
    // public List<GameObject> books;
    public int change;
    public CustomerType customerType;

    [Header("Debugging")]
    [SerializeField] private bool isCheckingOut;

    private void OnEnable()
    {
        Events.onCustomerCome.Add(OnCustomerCome);
        Events.onGetNextCustomer.Add(OnGetNextCustomer);

        ConversationManager.OnConversationEnded += OnConversationEnded;
    }
    private void OnDisable()
    {
        Events.onCustomerCome.Remove(OnCustomerCome);
        Events.onGetNextCustomer.Remove(OnGetNextCustomer);

        ConversationManager.OnConversationEnded -= OnConversationEnded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent<Interactable>(out Interactable interactable))
            Debug.LogError("ERROR: GameObject " + gameObject.name + " has no Interactable script!");

        isCheckingOut = false;

        if (CustomerQueueManager.Instance.GetCurrentCustomer() == gameObject)
            animator.SetBool("isCheckingOut", true);

        Instantiate(book1Prefab, book1PositionPoint.transform);
        Instantiate(book2Prefab, book2PositionPoint.transform);
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
            
            if (customerType == CustomerType.BuyNTalk)
            {
                CashManager.Instance.UpdateCashRegisterChangeDisplay(change.ToString());
                CashManager.Instance.currentChangeNeeded = change;
            }
            else
            {

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

    void OnConversationEnded()
    {

    }

    public void EnableInteraction(bool toggle)
    {
        interactCollider.enabled = toggle;
    }
}
