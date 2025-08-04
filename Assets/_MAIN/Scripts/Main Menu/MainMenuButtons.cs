using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] Button loadSaveButton;

    private void Start()
    {
        if (SaveGame.Load<int>("lastDay") <= 0)
        {
            loadSaveButton.interactable = false;
        }
    }

    public void OnNewGameClicked()
    {
        SaveGame.Save<int>("lastDay", -1);
        SaveGame.Save<int>("trustStat", 5);
        SaveGame.Save<int>("moneyStat", 5);
        SaveGame.Save<int>("stockStat", 10);

        SceneTransitionManager.Instance.StartTransitionToScene("Cutscene Prologue");
    }

    public void OnContinueClicked()
    {
        SceneTransitionManager.Instance.StartTransitionToScene("Gameplay");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
