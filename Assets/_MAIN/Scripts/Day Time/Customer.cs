using DialogueEditor;
using UnityEngine;

public enum CustomerType { BuyNTalk, TalkOnly };
[RequireComponent(typeof(Interactable))]
public class Customer : MonoBehaviour
{
    [Header("References")]
    public NPCConversation conversation;
    public Animator animator;
    public Animator avatarAnimator;
    [SerializeField] Collider interactCollider;

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
    }
    private void OnDisable()
    {
        Events.onCustomerCome.Remove(OnCustomerCome);
        Events.onGetNextCustomer.Remove(OnGetNextCustomer);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent<Interactable>(out Interactable interactable))
            Debug.LogError("ERROR: GameObject " + gameObject.name + " has no Interactable script!");

        isCheckingOut = false;

        if (CustomerQueueManager.Instance.GetCurrentCustomer() == gameObject)
            animator.SetBool("isCheckingOut", true);

        interactCollider.enabled = false;
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting") || animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting Checkout"))
            avatarAnimator.SetBool("isWalking", false);
        else
            avatarAnimator.SetBool("isWalking", true);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Waiting Checkout") && !isCheckingOut)
        {
            Events.onCustomerCome.Trigger();
            isCheckingOut = true;
        }
    }

    void OnCustomerCome()
    {
        if (CustomerQueueManager.Instance.GetCurrentCustomer() == gameObject)
        {
            interactCollider.enabled = true;

            // put books on table

            CashManager.Instance.UpdateCashRegisterChangeDisplay(change.ToString());
            CashManager.Instance.currentChangeNeeded = change;
        }
    }

    void OnGetNextCustomer()
    {
        if(CustomerQueueManager.Instance.GetCurrentCustomer() == gameObject)
            animator.SetBool("isCheckingOut", true);
    }
}
