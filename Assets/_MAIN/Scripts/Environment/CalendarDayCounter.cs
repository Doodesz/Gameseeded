using TMPro;
using UnityEngine;

public class CalendarDayCounter : MonoBehaviour
{
    [SerializeField] TextMeshPro text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.text = CustomerNDayManager.Instance.GetCurrentDayIndex().ToString() + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
