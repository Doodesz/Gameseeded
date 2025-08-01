using UnityEngine;
using UnityEngine.UI;

public class BookstoreStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Slider trustBarSlider;
    [SerializeField] Slider moneyBarSlider;
    [SerializeField] Slider stockBarSlider;

    [Header("Stats")]
    [SerializeField][Range(0f, 10f)] int trust;
    [SerializeField][Range(0f, 10f)] int money;
    [SerializeField][Range(0f, 20f)] int stock;

    public static BookstoreStats Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        UpdateParametersBars();
    }

    public void AdjustTrust(int amount)
    {
        trust += amount;
        UpdateParametersBars();
    }
    public void AdjustMoney(int amount)
    {
        money += amount;
        UpdateParametersBars();
    }
    public void AdjustStock(int amount)
    {
        stock += amount;
        UpdateParametersBars();
    }

    void UpdateParametersBars()
    {
        trustBarSlider.value = trust;
        moneyBarSlider.value = money;
        stockBarSlider.value = stock;
    }
}
