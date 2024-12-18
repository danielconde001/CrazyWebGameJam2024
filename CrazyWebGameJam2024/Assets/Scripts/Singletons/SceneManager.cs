using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    public static SceneManager Instance()
    {
        if (instance == null)
        {
            GameObject sceneManager = Instantiate(Resources.Load("Prefabs/Singletons/SceneManager", typeof(GameObject)) as GameObject);
            instance = sceneManager.GetComponent<SceneManager>();
        }
        return instance;
    }

    [SerializeField] private Image sceneFader;
    [SerializeField] private float defaultTransitionDuration;

    private bool isTransitioning = false;
    
    private void Awake()
    {
        instance = this;

        //DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(int pIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(pIndex);
    }

    public void LoadSceneWithFade(string sceneName)
    {
        LoadSceneWithFade(sceneName, defaultTransitionDuration);
    }

    public void LoadSceneWithFade(string sceneName, float transitionDuration)
    {
        if(isTransitioning == false)
        {
            isTransitioning = true;
            sceneFader.DOColor(Color.black, transitionDuration / 2.0f).SetEase(Ease.Linear).OnComplete(()=>{
                StartCoroutine(LoadSceneWithFadeAsync(sceneName, transitionDuration));
            });
        }
    }

    public void LoadCurrentScene()
    {
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        LoadScene(sceneIndex);
    }
    
    public void LoadCurrentSceneWithFade()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        LoadSceneWithFade(sceneName);
    }

    private IEnumerator LoadSceneWithFadeAsync(string sceneName, float transitionDuration)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);

        while(asyncLoad.isDone == false)
        {
            yield return null;
        }
        
        sceneFader.DOColor(Color.clear, transitionDuration / 2.0f).SetEase(Ease.Linear).OnComplete(()=>{
            isTransitioning = true;
        });
    }
}
