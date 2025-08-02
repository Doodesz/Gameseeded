using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DayManager : MonoBehaviour
{
    [Header("Days list")]
    [SerializeField] List<Day> days;

    [Header("Debugging")]
    [SerializeField] Day dayToLoad;

    public static DayManager Instance;

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}
    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    public Day GetDayData(int dayIndex)
    {
        return days[dayIndex];
    }
}
