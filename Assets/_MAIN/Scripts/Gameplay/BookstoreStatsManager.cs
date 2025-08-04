using BayatGames.SaveGameFree;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    enum GameOverType { noTrust, noMoney, noStock }
    [Header("Debugging")]
    GameOverType gameOverType;

    public static BookstoreStatsManager Instance;

    private void OnEnable()
    {
        Events.onGameOver.Add(OnGameOver);
    }
    private void OnDisable()
    {
        Events.onGameOver.Remove(OnGameOver);
    }

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

    #region Adjust Parameters
    public void AdjustTrust(int amount)
    {
        trust += amount;
        if (trust > maxTrust) trust = maxTrust;

        UpdateParametersBars();

        if (IsAnyStatDepleted()) Events.onGameOver.Trigger();
    }
    public void AdjustMoney(int amount)
    {
        money += amount;
        if (money > maxMoney) money = maxMoney;

        UpdateParametersBars();

        if (IsAnyStatDepleted()) Events.onGameOver.Trigger();
    }
    public void AdjustStock(int amount)
    {
        stock += amount;
        if (stock > maxStock) stock = maxStock;

        UpdateParametersBars();

        if (IsAnyStatDepleted()) Events.onGameOver.Trigger();
    }
    #endregion

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

    #region GetStats
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
    #endregion

    void LoadParametersStats()
    {
        trust = SaveGame.Load<int>("trustStat");
        money = SaveGame.Load<int>("moneyStat");
        stock = SaveGame.Load<int>("stockStat");
    }

    void OnGameOver()
    {
        if (trust <= 0)
        {
            StartCoroutine(GameOverNoTrust());
        }
        else if (money <= 0)
        {
            StartCoroutine(GameOverNoMoney());
        }
        else
        {
            StartCoroutine(GameOverNoStock());
        }
    }

    IEnumerator GameOverNoTrust()
    {
        yield return new WaitForSeconds(3f);
        SceneTransitionManager.Instance.StartTransitionToScene("Game Over No Trust");
    }

    IEnumerator GameOverNoMoney()
    {
        yield return new WaitForSeconds(3f);
        SceneTransitionManager.Instance.StartTransitionToScene("Game Over No Money");
    }

    IEnumerator GameOverNoStock()
    {
        yield return new WaitForSeconds(3f);
        SceneTransitionManager.Instance.StartTransitionToScene("Game Over No Stock");
    }

    bool IsAnyStatDepleted()
    {
        if (trust <= 0 || money <= 0 || stock <= 0) return true;
        return false;
    }
}
