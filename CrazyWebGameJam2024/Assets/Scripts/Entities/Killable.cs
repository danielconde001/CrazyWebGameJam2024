using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Killable : MonoBehaviour
{
    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Death()
    {
        animator.SetBool("isDead", true);
    }
}
