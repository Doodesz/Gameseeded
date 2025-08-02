using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DayManager : MonoBehaviour
{
    [Header("Days list")]
    [SerializeField] List<Day> days;

    [Header("Debugging")]
    [SerializeField] Day dayToLoad;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // This means all scripts have run Start() right?
    {

    }
}
