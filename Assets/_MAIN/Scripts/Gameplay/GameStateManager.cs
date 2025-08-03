using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public enum GameState { Playing, OnConversation, Paused, DayEnded };
    public GameState gameState;
    [Header("References")]
    [SerializeField] Animator uiAnimator;
    public static GameStateManager Instance;

    private void OnEnable()
    {
        Events.onDayEnded.Add(OnDayEnded);

        ConversationManager.OnConversationStarted += ConversationStartBehavior;
        ConversationManager.OnConversationEnded += ConversationEndBehavior;
    }
    private void OnDisable()
    {
        Events.onDayEnded.Remove(OnDayEnded);

        ConversationManager.OnConversationStarted -= ConversationStartBehavior;
        ConversationManager.OnConversationEnded -= ConversationEndBehavior;
    }

    void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    void ConversationStartBehavior()
    {
        gameState = GameState.OnConversation;

        ActivateCursor(true);
    }

    void ConversationEndBehavior()
    {
        gameState = GameState.Playing;

        ActivateCursor(false);
    }

    void ActivateCursor(bool toggle)
    {
        FirstPersonController.Instance.cameraCanMove = !toggle;
        FirstPersonController.Instance.enableZoom = !toggle;
        FirstPersonController.Instance.lockCursor = !toggle;
        Cursor.visible = toggle;
        if(toggle)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDayEnded()
    {
        gameState = GameState.DayEnded;

        ActivateCursor(true);
    }

    public void PauseGame()
    {
        gameState = GameState.Paused;

        ActivateCursor(true);
        uiAnimator.SetBool("isPaused", true);
    }
    public void ResumeGame()
    {
        gameState = GameState.Playing;

        ActivateCursor(false);
        uiAnimator.SetBool("isPaused", false);
    }

    // Called when pressing esc
    public void TogglePauseGame()
    {
        if (gameState == GameState.Paused) { ResumeGame(); }
        else if (gameState == GameState.Playing || gameState == GameState.OnConversation) { PauseGame(); }
    }

    public void ExitGame()
    {
        SceneTransitionManager.Instance.StartTransitionToScene("Main Menu");
    }
}
