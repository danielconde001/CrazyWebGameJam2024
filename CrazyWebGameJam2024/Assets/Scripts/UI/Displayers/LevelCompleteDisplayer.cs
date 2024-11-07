using UnityEngine;

public class LevelCompleteDisplayer : LevelConclusionDisplayer
{
    [SerializeField] private int nextSceneIndex = 0;
    
    protected override void Update()
    {
        if (display.activeSelf && GameManager.Instance().IsLevelFinished())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.Instance().LoadScene(nextSceneIndex);
            }
        }
    }
}
