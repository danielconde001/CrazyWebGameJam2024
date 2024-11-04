using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    public static SceneManager Instance()
    {
        if (instance == null)
        {
            GameObject sceneManager = Instantiate(Resources.Load("Prefabs/Singletons/MusicManager", typeof(GameObject)) as GameObject);
            instance = sceneManager.GetComponent<SceneManager>();
        }
        return instance;
    }
    
    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(int pIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(pIndex);
    }
}
