using UnityEngine;

public enum InteractType { AddCash, RemoveCash, ReturnCashButton };
public class Interactable : MonoBehaviour
{
    public InteractType thisTypeOfInteractable;
}
