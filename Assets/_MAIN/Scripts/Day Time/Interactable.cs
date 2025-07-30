using UnityEngine;
using UnityEngine.Rendering;

public enum InteractType { AddCash, RemoveCash, SubmitChangeButton, TalkToCustomer };
public class Interactable : MonoBehaviour
{
    [Header("Parameters")]
    public InteractType thisTypeOfInteractable;

    [Header("References")]
    [SerializeField] private Outline outline;

    private void Update()
    {
        OutlineBehaviour();
    }

    void OutlineBehaviour()
    {
        if (Player.Instance.selectedObj == null) return;

        // De-outline the selected obj      
        if (outline.enabled && Player.Instance.selectedObj != this.gameObject) outline.enabled = false; 

    }
}
