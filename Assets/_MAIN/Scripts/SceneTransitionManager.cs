using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Animator sceneTransitionAnimator;

    public static SceneTransitionManager Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    private void Start()
    {
        sceneTransitionAnimator.SetBool("isTransitioning", false);
    }

    public void StartTransitionToScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    public IEnumerator TransitionToScene(string sceneName)
    {
        sceneTransitionAnimator.SetBool("isTransitioning", true);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
