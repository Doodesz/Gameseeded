using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource selectInteraction;
    [SerializeField] AudioSource submitChange;
    [SerializeField] AudioSource cashClick;
    [SerializeField] AudioSource uiButtonClick;

    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    public void PlaySelectInteractionSFX() {  selectInteraction.Play(); }
    public void PlaySubmitChangeSFX() { submitChange.Play(); }
    public void PlayCashClickSfx() { cashClick.Play(); }
    public void PlayUIButtonClickSfx() { uiButtonClick.Play(); }
}
