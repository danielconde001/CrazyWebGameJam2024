using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Hurtable : MonoBehaviour
{
    [SerializeField] private Color hurtColor;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hurt()
    {
        spriteRenderer.color = hurtColor;
        
        Invoke("ReturnToWhite", .2f);
    }

    private void ReturnToWhite()
    {
        spriteRenderer.color = Color.white;
    }
}
