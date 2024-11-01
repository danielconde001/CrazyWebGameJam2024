using UnityEngine;
using System.Threading.Tasks;

[RequireComponent(typeof(SpriteRenderer))]
public class Hurtable : MonoBehaviour
{
    [SerializeField] private Color hurtColor;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public async void Hurt()
    {
        spriteRenderer.color = hurtColor;
        await Task.Delay(200);
        spriteRenderer.color = Color.white;
    }
}
