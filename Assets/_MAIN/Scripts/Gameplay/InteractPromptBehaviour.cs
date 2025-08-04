using DialogueEditor;
using UnityEngine;

public class InteractPromptBehaviour : MonoBehaviour
{
    [SerializeField] GameObject interactPrompt;

    public static InteractPromptBehaviour Instance;

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += OnConversationEnded;
    }
    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= OnConversationEnded;
    }

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        interactPrompt.SetActive(false);
    }

    public void ToggleShowPrompt(bool toggle)
    {
        interactPrompt.SetActive(toggle);
    }

    void OnConversationEnded()
    {
        interactPrompt.SetActive(false );
    }
}
