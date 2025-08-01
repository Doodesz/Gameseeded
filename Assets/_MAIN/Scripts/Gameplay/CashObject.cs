using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class CashObject : MonoBehaviour
{
    public int cashAmount;

    private void Start()
    {
        if (!TryGetComponent<Interactable>(out Interactable interactable))
            Debug.LogError("ERROR: GameObject " + gameObject.name + " has no Interactable script!");
    }
}
