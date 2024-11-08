using UnityEngine;

public class LevelCompleteDisplayer : LevelConclusionDisplayer
{
    [SerializeField] private string nextSceneName = string.Empty;
    
    protected override void Update()
    {
        if (display.activeSelf && GameManager.Instance().IsLevelFinished())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.Instance().LoadSceneWithFade(nextSceneName);
            }
        }
    }
}
