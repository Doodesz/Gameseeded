using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public void OnSkipClicked()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
