using UnityEngine;

public class RunManager : MonoBehaviour
{
    public int currentChangeAmount;

    public static RunManager Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
