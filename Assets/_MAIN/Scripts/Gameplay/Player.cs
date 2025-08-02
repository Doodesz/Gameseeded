using DialogueEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Tooltip("References")]
    [SerializeField] GameObject camObj;

    [Tooltip("Debugging")]
    public GameObject selectedObj;
    CashManager runManager;
    CustomerQueueManager customerQueueManager;

    public static Player Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;        
    }

    void Start()
    {
        runManager = CashManager.Instance;
        customerQueueManager = CustomerQueueManager.Instance;
    }

    void Update()
    {
        if (GameStateManager.Instance.gameState == GameStateManager.GameState.Playing)
        {
            // Always raycast to find an interactable object on reticle
            bool isHit = Physics.Raycast(camObj.transform.position, camObj.transform.forward, out RaycastHit hitInfo);

            // If selected object on reticle is interactable, select it
            if (isHit && hitInfo.collider.gameObject.CompareTag("Interactable"))
            {
                if (hitInfo.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactableScript) == false)
                    Debug.LogError("ERROR: Cannot get Interactable script Component of " + hitInfo.collider.gameObject.name);

                // Select obj as current interactable
                selectedObj = hitInfo.collider.gameObject;

                // Outline the selected obj
                if (selectedObj.TryGetComponent<Outline>(out Outline outline) == false)
                    Debug.LogWarning("WARNING: Cannot get Outline component of " + selectedObj.name);
                else outline.enabled = true;

                Events.onSelectNewInteractable.Trigger();
            }

            // Else if not selecting any object, deselect previous selected object
            else if (selectedObj != null)
            {
                // De-outline the selected obj
                if (selectedObj.TryGetComponent<Outline>(out Outline outline) == false)
                    Debug.LogWarning("WARNING: Cannot get Outline component of " + selectedObj.name);
                else outline.enabled = false;

                selectedObj = null;

                Events.onSelectNewInteractable.Trigger();
            }
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        // Interact only when an object is selected and on mouse button down
        if (selectedObj != null && context.started)
        {
            if (selectedObj.TryGetComponent<Interactable>(out Interactable interactableScript) == false)
            {
                Debug.LogError("ERROR: Cannot get Interactable script Component of " + selectedObj.name);
                return;
            }

            InteractType interactType = interactableScript.thisTypeOfInteractable;
            selectedObj.TryGetComponent<CashObject>(out CashObject cash);
            selectedObj.TryGetComponent<Customer>(out Customer customer);

            switch (interactType)
            {
                case InteractType.AddCash:
                    if (cash == null)
                    {
                        Debug.LogError("ERROR: Cannot get CashObject script component of " + selectedObj.name);
                        break;
                    }
                    if (customerQueueManager.cashierStatus == CustomerQueueManager.CurrentCashierStatus.Occcupied)
                        runManager.AddCash(cash.cashAmount);
                    break;

                case InteractType.RemoveCash:
                    if (cash == null)
                    {
                        Debug.LogError("ERROR: Cannot get CashObject script component of " + selectedObj.name);
                        break;
                    }
                    if (customerQueueManager.cashierStatus == CustomerQueueManager.CurrentCashierStatus.Occcupied)
                        runManager.RemoveCash(cash.cashAmount);
                    break;

                case InteractType.SubmitChangeButton:
                    if (customerQueueManager.cashierStatus == CustomerQueueManager.CurrentCashierStatus.Occcupied
                        && customerQueueManager.GetCurrentCustomer().GetComponent<Customer>().customerType != CustomerType.TalkOnly)
                        CashManager.Instance.SubmitCash();
                    else if (customerQueueManager.GetCurrentCustomer().GetComponent<Customer>().customerType == CustomerType.TalkOnly)
                        Debug.Log("Customer is talk only!");
                    else Debug.Log("No customer present");
                    break;

                case InteractType.TalkToCustomer:
                    customer.StartConversation();
                    selectedObj = null;
                    break;
            }
        }
    }
}
