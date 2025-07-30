using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class SubmitChangeButton : MonoBehaviour
{
    CashManager cashManager;

    private void OnEnable()
    {
        Events.onCustomerCome.Add(OnCustomerCome);
        Events.onCustomerLeave.Add(OnCustomerLeave);
    }
    private void OnDisable()
    {
        Events.onCustomerCome.Remove(OnCustomerCome);
        Events.onCustomerLeave.Remove(OnCustomerLeave);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cashManager = CashManager.Instance;
    }

    void OnCustomerCome()
    {

    }

    void OnCustomerLeave()
    {

    }
}
