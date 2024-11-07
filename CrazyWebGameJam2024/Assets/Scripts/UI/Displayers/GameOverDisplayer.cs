using UnityEngine;

public class GameOverDisplayer : LevelConclusionDisplayer
{
    protected override void Update()
    {
        if (display.activeSelf && GameManager.Instance().IsGameOver())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.Instance().LoadCurrentScene();
            }
        }
    }
}
