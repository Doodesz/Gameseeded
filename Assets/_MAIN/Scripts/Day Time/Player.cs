using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Tooltip("References")]
    [SerializeField] GameObject camObj;

    [Tooltip("Debugging")]
    [SerializeField] GameObject selectedObj;
    CashManager runManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runManager = CashManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Always raycast to find an interactable object on reticle
        bool isHit = Physics.Raycast(camObj.transform.position, camObj.transform.forward, out RaycastHit hitInfo);

        // If selected object on reticle is interactable, select it
        if (isHit && hitInfo.collider.gameObject.CompareTag("Interactable"))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<Interactable>(out Interactable interactableScript) == false)
                Debug.LogError("ERROR: Cannot get Interactable script Component of " + hitInfo.collider.gameObject.name);

            selectedObj = hitInfo.collider.gameObject;
        }

        // Else if not selecting any object, deselect previous selected object
        else if (selectedObj != null)
        {
            // remove outline of object

            selectedObj = null;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (selectedObj != null && context.started)
        {
            if (selectedObj.TryGetComponent<Interactable>(out Interactable interactableScript) == false)
            {
                Debug.LogError("ERROR: Cannot get Interactable script Component of " + selectedObj.name);
                return;
            }

            InteractType interactType = interactableScript.thisTypeOfInteractable;
            selectedObj.TryGetComponent<CashObject>(out CashObject cash);

            switch (interactType)
            {
                case InteractType.AddCash:
                    if (cash == null) Debug.LogError("ERROR: Cannot get CashObject script component of " + selectedObj.name);
                    runManager.AddCash(cash.cashAmount);
                    break;

                case InteractType.RemoveCash:
                    if (cash == null) Debug.LogError("ERROR: Cannot get CashObject script component of " + selectedObj.name);
                    runManager.RemoveCash(cash.cashAmount);
                    break;

                case InteractType.ReturnCashButton:
                    // submit change
                    break;
            }
        }
    }
}
