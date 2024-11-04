using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    public static SceneManager Instance()
    {
        if (instance == null)
        {
            GameObject sceneManager = new GameObject("SceneManager");
            instance = sceneManager.AddComponent<SceneManager>();
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
