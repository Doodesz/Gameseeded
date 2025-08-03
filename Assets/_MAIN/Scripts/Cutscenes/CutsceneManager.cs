using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] bool hasTimeout;
    [SerializeField] float timeoutDuration = 75f;
    float timeoutCurrentDuration;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (hasTimeout) timeoutCurrentDuration += Time.deltaTime;
        if (hasTimeout && timeoutCurrentDuration > timeoutDuration) GoToScene("Gameplay");
    }

    public void GoToScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void GoToScene(string targetScene)
    {
        SceneManager.LoadScene(targetScene);
    }
}
