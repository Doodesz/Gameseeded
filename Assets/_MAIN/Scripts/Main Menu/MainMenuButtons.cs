using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
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
