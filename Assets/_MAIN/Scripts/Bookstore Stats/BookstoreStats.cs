using UnityEngine;

public class BookstoreStats : MonoBehaviour
{
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

    public void AdjustTrust(int amount)
    {
        trust += amount;
    }
    public void AdjustMoney(int amount)
    {
        money += amount;
    }
    public void AdjustStock(int amount)
    {
        stock += amount;
    }
}
