using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayEndedBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator uiAnimator;
    [SerializeField] GameObject dayEndedScreen;
    [Space(10)]
    [SerializeField] Slider trustBar;
    [SerializeField] Slider moneyBar;
    [SerializeField] Slider stockBar;
    [Space(10)]
    [SerializeField] Slider trustBarRef;
    [SerializeField] Slider moneyBarRef;
    [SerializeField] Slider stockBarRef;
    [Space(10)]
    [SerializeField] TextMeshProUGUI customersServedText;
    [SerializeField] TextMeshProUGUI customersCorrectChangeText;
    [SerializeField] TextMeshProUGUI customersWrongChangeText;
    [SerializeField] TextMeshProUGUI customersTalkedText;

    [Header("Debugging")]
    [SerializeField] int customersServed;
    [SerializeField] int customersCorrectChange;
    [SerializeField] int customersWrongChange;
    [SerializeField] int customersTalked;

    public static DayEndedBehaviour Instance;

    private void OnEnable()
    {
        Events.onDayEnded.Add(OnDayEnded);
    }
    private void OnDisable()
    {
        Events.onDayEnded.Remove(OnDayEnded);
    }

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        // get initial parameters stats
        customersServed = 0;
        customersCorrectChange = 0;
        customersWrongChange = 0;
        customersTalked = 0;

        dayEndedScreen.SetActive(false);
    }

    void OnDayEnded()
    {
        uiAnimator.SetTrigger("displayDayEndedScreen");

        customersServedText.text = customersServed.ToString();
        customersCorrectChangeText.text = customersCorrectChange.ToString();
        customersWrongChangeText.text = customersWrongChange.ToString();
        customersTalkedText.text = customersTalked.ToString();

        trustBar.maxValue = trustBarRef.maxValue;
        trustBar.minValue = trustBarRef.minValue;
        trustBar.value = trustBarRef.value;

        moneyBar.maxValue = moneyBarRef.maxValue;
        moneyBar.minValue = moneyBarRef.minValue;
        moneyBar.value = moneyBarRef.value;

        stockBar.maxValue = stockBarRef.maxValue;
        stockBar.minValue = stockBarRef.minValue;
        stockBar.value = stockBarRef.value;
    }

    // Called by buy more stock button
    public void OnBuyStockClick()
    {
        if (moneyBar.value > 0 && stockBar.value < stockBar.maxValue)
        {
            stockBar.value += 2;
            moneyBar.value -= 1;
        }
        else
            Debug.Log("Not enough money or stock is already at max!");
    }

    // Called by save n continue button
    public void OnSaveNContinueClick()
    {
        SaveGame();
        Debug.Log("Save and continuing...");

        // continue to next day
    }

    public void OnSaveNExitClick()
    {
        SaveGame();
        Debug.Log("Save and quitting...");

        // exit to main menu
    }

    void SaveGame()
    {
        // save parameters and last day completed
    }

    public void AddCustomersServed()
    {
        customersServed++;
    }
    public void AddCustomersCorrectChange()
    {
        customersCorrectChange++;
    }
    public void AddCustomersWrongChange()
    {
        customersWrongChange++;
    }
    public void AddCustomersTalked()
    {
        customersTalked++;
    }
}
