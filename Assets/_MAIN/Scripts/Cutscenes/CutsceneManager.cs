using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] bool hasTimeout;
    [SerializeField] float timeoutDuration = 75f;
    [SerializeField] string timeoutTargetScene;
    float timeoutCurrentDuration;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (hasTimeout) timeoutCurrentDuration += Time.deltaTime;
        if (hasTimeout && timeoutCurrentDuration > timeoutDuration) GoToScene(timeoutTargetScene);
    }

    public void GoToScene()
    {
        SceneTransitionManager.Instance.StartTransitionToScene("Main Menu");
    }
    public void GoToScene(string targetScene)
    {
        SceneTransitionManager.Instance.StartTransitionToScene(targetScene);
    }
}
