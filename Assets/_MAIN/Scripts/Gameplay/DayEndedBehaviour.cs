using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    BookstoreStatsManager bookstoreStatsManager;

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
        bookstoreStatsManager = BookstoreStatsManager.Instance;

        // get initial parameters stats
        customersServed = 0;
        customersCorrectChange = 0;
        customersWrongChange = 0;
        customersTalked = 0;

        dayEndedScreen.SetActive(false);

        UpdateParametersBars();
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
        trustBar.value = bookstoreStatsManager.GetTrustStat();

        moneyBar.maxValue = moneyBarRef.maxValue;
        moneyBar.minValue = moneyBarRef.minValue;
        moneyBar.value = bookstoreStatsManager.GetMoneyStat();

        stockBar.maxValue = stockBarRef.maxValue;
        stockBar.minValue = stockBarRef.minValue;
        stockBar.value = bookstoreStatsManager.GetStockStat();
    }

    void UpdateParametersBars()
    {
        trustBar.value = bookstoreStatsManager.GetTrustStat();
        moneyBar.value = bookstoreStatsManager.GetMoneyStat();
        stockBar.value = bookstoreStatsManager.GetStockStat();
    }

    // Called by buy more stock button
    public void OnBuyStockClick()
    {
        if (bookstoreStatsManager.GetMoneyStat() > 0 && bookstoreStatsManager.GetStockStat() < bookstoreStatsManager.maxStock)
        {
            bookstoreStatsManager.AdjustStock(2);
            bookstoreStatsManager.AdjustMoney(-1);

            UpdateParametersBars();
        }
        else
            Debug.Log("Not enough money or stock is already at max!");
    }

    // Called by save n continue button
    public void OnSaveNContinueClick()
    {
        SaveCurrentGame();
        SceneManager.LoadScene("Gameplay");

        Debug.Log("Save and continuing...");
    }

    public void OnSaveNExitClick()
    {
        SaveCurrentGame();
        SceneManager.LoadScene("Main Menu");
        
        Debug.Log("Save and quitting...");
    }

    void SaveCurrentGame()
    {
        // save parameters and last day completed
        SaveGame.Save<int>("lastDay", CustomerNDayManager.Instance.GetCurrentDayIndex());
        SaveGame.Save<int>("trustStat", BookstoreStatsManager.Instance.GetTrustStat());
        SaveGame.Save<int>("moneyStat", BookstoreStatsManager.Instance.GetMoneyStat());
        SaveGame.Save<int>("stockStat", BookstoreStatsManager.Instance.GetStockStat());
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
