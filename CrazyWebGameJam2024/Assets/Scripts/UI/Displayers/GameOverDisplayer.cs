using UnityEngine;

public class GameOverDisplayer : LevelConclusionDisplayer
{
    [SerializeField] private bool reloadCurrentScene = true;
    [SerializeField] private string goToSceneName;
    protected override void Update()
    {
        if (display.activeSelf && GameManager.Instance().IsGameOver())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (reloadCurrentScene)
                {
                    SceneManager.Instance().LoadCurrentSceneWithFade();
                }
                else
                {
                    SceneManager.Instance().LoadSceneWithFade(goToSceneName);
                }
            }
        }
    }
}
