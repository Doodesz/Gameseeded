using UnityEngine;
using UnityEngine.Rendering;

public enum InteractType { AddCash, RemoveCash, SubmitChangeButton, TalkToCustomer };
[RequireComponent(typeof(Outline))]
public class Interactable : MonoBehaviour
{
    [Header("Parameters")]
    public InteractType thisTypeOfInteractable;

    [Header("References")]
    [SerializeField] private Outline outline;

    private void OnEnable()
    {
        Events.onSelectNewInteractable.Add(OutlineBehaviour);
    }
    private void OnDisable()
    {
        Events.onSelectNewInteractable.Remove(OutlineBehaviour);
    }

    void OutlineBehaviour()
    {
        // De-outline the selected obj      
        if (Player.Instance.selectedObj == this.gameObject) 
            outline.enabled = true;
        else if (outline.enabled && (Player.Instance.selectedObj != this.gameObject || Player.Instance.selectedObj == null)) 
            outline.enabled = false;
        //else Debug.LogWarning("WARNING: Unable to enable/disable Outline component of " + gameObject.name + "GameObject");
    }
}
