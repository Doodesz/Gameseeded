using DialogueEditor;
using UnityEngine;

public enum CustomerType { BuyNTalk, TalkOnly };
[RequireComponent(typeof(Interactable))]
public class Customer : MonoBehaviour
{
    [Header("References")]
    public NPCConversation conversation;

    [Header("Parameters")]
    // public List<GameObject> books;
    public int change;
    public CustomerType customerType;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!TryGetComponent<Interactable>(out Interactable interactable))
            Debug.LogError("ERROR: GameObject " + gameObject.name + " has no Interactable script!");
    }
}
