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

        SceneManager.LoadScene("Cutscene Prologue");
    }

    public void OnContinueClicked()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void OnQuitClicked()
    {
        // quit game
    }
}
