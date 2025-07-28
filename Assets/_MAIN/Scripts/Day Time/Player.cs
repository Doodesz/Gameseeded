using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Tooltip("References")]
    [SerializeField] GameObject camObj;

    [Tooltip("Debugging")]
    public GameObject selectedObj;
    CashManager runManager;

    public static Player Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runManager = CashManager.Instance;

        if (Instance != null) Destroy(this);
        Instance = this;
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

            // Select obj as current interactable
            selectedObj = hitInfo.collider.gameObject;

            // Outline the selected obj
            if (selectedObj.TryGetComponent<Outline>(out Outline outline) == false)
                Debug.LogWarning("WARNING: Cannot get Outline component of " + selectedObj.name);
            else outline.enabled = true;
        }

        // Else if not selecting any object, deselect previous selected object
        else if (selectedObj != null)
        {
            // De-outline the selected obj
            if (selectedObj.TryGetComponent<Outline>(out Outline outline) == false)
                Debug.LogWarning("WARNING: Cannot get Outline component of " + selectedObj.name);
            else outline.enabled = false;

            selectedObj = null;
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

            switch (interactType)
            {
                case InteractType.AddCash:
                    if (cash == null)
                    {
                        Debug.LogError("ERROR: Cannot get CashObject script component of " + selectedObj.name);
                        break;
                    }
                    runManager.AddCash(cash.cashAmount);
                    break;

                case InteractType.RemoveCash:
                    if (cash == null)
                    {
                        Debug.LogError("ERROR: Cannot get CashObject script component of " + selectedObj.name);
                        break;
                    }
                    runManager.RemoveCash(cash.cashAmount);
                    break;

                case InteractType.SubmitChangeButton:
                    // submit change
                    break;
            }
        }
    }
}
