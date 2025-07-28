using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CashManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject cash1Prefab;
    [SerializeField] GameObject cash10Prefab;
    [SerializeField] GameObject cash100Prefab;
    [SerializeField] GameObject cash1StackPos;
    [SerializeField] GameObject cash10StackPos;
    [SerializeField] GameObject cash100StackPos;
    [SerializeField] TextMeshPro cashRegOnCounterDisplayText;
    [SerializeField] TextMeshPro cashRegChangeDisplayText;

    [Header("Parameters")]
    [SerializeField] float stackGap;

    [Header("Debugging")]
    public int currentChangeAmount;
    public int cash1Amount;
    public int cash10Amount;
    public int cash100Amount;
    
    [SerializeField] Stack<GameObject> cash1Stacks = new Stack<GameObject>();
    [SerializeField] Stack<GameObject> cash10Stacks = new Stack<GameObject>();
    [SerializeField] Stack<GameObject> cash100Stacks = new Stack<GameObject>();

    public static CashManager Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        UpdateCashRegisterDisplay();
    }

    public void AddCash(int amount)
    {
        if (amount == 1)
        {
            cash1Amount++;
            AddCash1ToStack();
        }
        else if (amount == 10)
        {
            cash10Amount++;
            AddCash10ToStack();
        }
        else if (amount == 100)
        {
            cash100Amount++;
            AddCash100ToStack();
        }
        else
        {
            Debug.LogError("ERROR: Cash amount is not 1, 10, or 100!");
            return;
        }

        currentChangeAmount += amount;
        UpdateCashRegisterDisplay();
    }

    public void RemoveCash(int amount)
    {
        if (amount == 1)
        {
            cash1Amount--;
            RemoveCash1FromStack();
        }
        else if (amount == 10)
        {
            cash10Amount--;
            RemoveCash10FromStack();
        }
        else if (amount == 100)
        {
            cash100Amount--;
            RemoveCash100FromStack();
        }
        else
        {
            Debug.LogError("ERROR: Cash amount is not 1, 10, or 100!");
            return;
        }

        currentChangeAmount -= amount;
        UpdateCashRegisterDisplay();
    }

    void UpdateCashRegisterDisplay()
    {
        cashRegOnCounterDisplayText.text = currentChangeAmount.ToString();
    }

    void AddCash1ToStack()
    {
        Vector3 newStackPos = cash1StackPos.transform.position + new Vector3(0, stackGap * (cash1Amount - 1), 0);
        cash1Stacks.Push(Instantiate(cash1Prefab, newStackPos, cash1StackPos.transform.rotation));
    }
    void AddCash10ToStack()
    {
        Vector3 newStackPos = cash10StackPos.transform.position + new Vector3(0, stackGap * (cash10Amount - 1), 0);
        cash10Stacks.Push(Instantiate(cash10Prefab, newStackPos, cash10StackPos.transform.rotation));
    }
    void AddCash100ToStack()
    {
        Vector3 newStackPos = cash100StackPos.transform.position + new Vector3(0, stackGap * (cash100Amount - 1), 0);
        cash100Stacks.Push(Instantiate(cash100Prefab, newStackPos, cash100StackPos.transform.rotation));
    }

    void RemoveCash1FromStack()
    {
        GameObject cash = cash1Stacks.Peek();
        cash1Stacks.Pop();
        Destroy(cash);
    }
    void RemoveCash10FromStack()
    {
        GameObject cash = cash10Stacks.Peek();
        cash10Stacks.Pop();
        Destroy(cash);
    }
    void RemoveCash100FromStack()
    {
        GameObject cash = cash100Stacks.Peek();
        cash100Stacks.Pop();
        Destroy(cash);
    }
}
