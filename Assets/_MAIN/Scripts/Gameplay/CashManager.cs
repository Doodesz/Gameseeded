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
    [SerializeField] TextMeshPro cashRegPriceDisplayText;
    [SerializeField] TextMeshPro cashRegPaidDisplayText;

    [Header("Parameters")]
    [SerializeField] float stackGap;

    [Header("Debugging")]
    public int currentChangeAmount;
    public int currentChangeNeeded;
    [SerializeField] int randomPrice;
    public int cash1Amount;
    public int cash10Amount;
    public int cash100Amount;
    
    [SerializeField] Stack<GameObject> cash1Stacks = new Stack<GameObject>();
    [SerializeField] Stack<GameObject> cash10Stacks = new Stack<GameObject>();
    [SerializeField] Stack<GameObject> cash100Stacks = new Stack<GameObject>();

    public static CashManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;        
    }

    void Start()
    {
        UpdateCashRegisterPriceDisplay("...");
        UpdateCashRegisterOnCounterDisplay("Menunggu...");
        UpdateCashRegisterPaidDisplay("...");

        currentChangeAmount = 0;
    }

    public void SubmitCash()
    {
        if(currentChangeAmount > currentChangeNeeded)      // More change given
        {
            BookstoreStatsManager.Instance.AdjustMoney(-1);
            BookstoreStatsManager.Instance.AdjustTrust(0);
            DayEndedBehaviour.Instance.AddCustomersWrongChange();
        }
        else if (currentChangeAmount < currentChangeNeeded) // Less change given
        {
            BookstoreStatsManager.Instance.AdjustMoney(2);
            BookstoreStatsManager.Instance.AdjustTrust(-1);
            DayEndedBehaviour.Instance.AddCustomersWrongChange();
        }
        else                                                // Correct
        {
            BookstoreStatsManager.Instance.AdjustMoney(1);
            BookstoreStatsManager.Instance.AdjustTrust(1);
            DayEndedBehaviour.Instance.AddCustomersCorrectChange();
        }

        Events.onChangeSubmit.Trigger();
        RemoveAllCashStacks();

        UpdateCashRegisterPriceDisplay("...");
        UpdateCashRegisterOnCounterDisplay("Menunggu...");
        UpdateCashRegisterPaidDisplay("...");
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
        UpdateCashRegisterOnCounterDisplay();
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
        UpdateCashRegisterOnCounterDisplay();
    }

    #region Cash register display and Add/Remove Cash from stacks
    void UpdateCashRegisterOnCounterDisplay()
    {
        cashRegOnCounterDisplayText.text = currentChangeAmount.ToString();
    }
    void UpdateCashRegisterOnCounterDisplay(string displayText)
    {
        cashRegOnCounterDisplayText.text = displayText;
    }
    public void UpdateCashRegisterPriceDisplay()
    {
        randomPrice = Random.Range(0, 1001);

        cashRegPriceDisplayText.text = randomPrice.ToString();
    }
    public void UpdateCashRegisterPriceDisplay(string displayText)
    {
        cashRegPriceDisplayText.text = displayText;
    }
    public void UpdateCashRegisterPaidDisplay()
    {
        cashRegPaidDisplayText.text = (randomPrice + currentChangeNeeded).ToString();
    }
    public void UpdateCashRegisterPaidDisplay(string displayText)
    {
        cashRegPaidDisplayText.text = displayText;
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
    #endregion

    void RemoveAllCashStacks()
    {
        currentChangeAmount = 0;

        // Destroy all the GameObjects in stacks content
        foreach(GameObject cash in cash1Stacks)
            Destroy(cash);
        foreach(GameObject cash in cash10Stacks)
            Destroy(cash);
        foreach (GameObject cash in cash100Stacks)
            Destroy(cash);

        // Clear all stacks
        cash1Stacks.Clear();
        cash10Stacks.Clear();
        cash100Stacks.Clear();
    }
}
