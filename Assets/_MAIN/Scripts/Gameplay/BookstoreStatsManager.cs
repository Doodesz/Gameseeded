using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.UI;

public class BookstoreStatsManager : MonoBehaviour
{
    [Header("Parameters")]
    public int maxTrust;
    public int maxMoney;
    public int maxStock;

    [Header("References")]
    [SerializeField] Slider trustBarSlider;
    [SerializeField] Slider moneyBarSlider;
    [SerializeField] Slider stockBarSlider;

    [Header("Stats")]
    [SerializeField][Range(0f, 10f)] int trust;
    [SerializeField][Range(0f, 10f)] int money;
    [SerializeField][Range(0f, 20f)] int stock;

    public static BookstoreStatsManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        SetupParametersBars();
        LoadParametersStats();
        UpdateParametersBars();
    }

    public void AdjustTrust(int amount)
    {
        trust += amount;
        if (trust > maxTrust) trust = maxTrust;

        UpdateParametersBars();
    }
    public void AdjustMoney(int amount)
    {
        money += amount;
        if (money > maxMoney) money = maxMoney;

        UpdateParametersBars();
    }
    public void AdjustStock(int amount)
    {
        stock += amount;
        if (stock > maxStock) stock = maxStock;

        UpdateParametersBars();
    }

    void UpdateParametersBars()
    {
        trustBarSlider.value = trust;
        moneyBarSlider.value = money;
        stockBarSlider.value = stock;
    }

    void SetupParametersBars()
    {
        trustBarSlider.minValue = 0f;
        moneyBarSlider.minValue = 0f;
        stockBarSlider.minValue = 0f;

        trustBarSlider.maxValue = maxTrust;
        moneyBarSlider.maxValue = maxMoney;
        stockBarSlider.maxValue = maxStock;
    }

    public int GetTrustStat()
    {
        return trust;
    }
    public int GetMoneyStat()
    {
        return money;
    }
    public int GetStockStat()
    {
        return stock;
    }

    void LoadParametersStats()
    {
        trust = SaveGame.Load<int>("trustStat");
        money = SaveGame.Load<int>("moneyStat");
        stock = SaveGame.Load<int>("stockStat");
    }
}
