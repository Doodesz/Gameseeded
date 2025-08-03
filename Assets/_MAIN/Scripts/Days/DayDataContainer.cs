using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DayDataContainer : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] List<Day> days;
    [SerializeField] int lastDayIndex;

    [Header("Debugging")]
    [SerializeField] Day dayToLoad;


    public static DayDataContainer Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    public Day GetDayData(int dayIndex)
    {
        return days[dayIndex];
    }

    public int GetLastDayIndex()  { return lastDayIndex; }
}
