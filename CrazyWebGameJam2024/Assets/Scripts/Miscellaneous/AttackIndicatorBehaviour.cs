
using UnityEngine;

public class AttackIndicatorBehaviour : MonoBehaviour
{
    private EnemyAI ai;
    [SerializeField] private GameObject bullseyeIcon;

    private void Awake()
    {
        ai = GetComponentInParent<EnemyAI>();
    }

    private void Update()
    {
        if (ai.CurrentEnemyState == EnemyState.ATTACK)
        {
            bullseyeIcon.SetActive(true);
        }
        else if (ai.CurrentEnemyState != EnemyState.ATTACK)
            bullseyeIcon.SetActive(false);
    }
}
