using UnityEngine;

public class BookstoreStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField][Range(0f, 10f)] int trust;
    [SerializeField][Range(0f, 10f)] int money;
    [SerializeField][Range(0f, 20f)] int stock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseTrust(int amount)
    {

    }

}
