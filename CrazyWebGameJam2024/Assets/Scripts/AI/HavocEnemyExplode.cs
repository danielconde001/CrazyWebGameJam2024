using UnityEngine;

public class HavocEnemyExplode : MonoBehaviour
{
    [SerializeField] private AudioClip explodeSFX;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float explosionRadius;

    private Transform selfTransform;

    public void Explode()
    {
        AudioManager.Instance().PlaySFX(explodeSFX, 0.5f);
        Collider2D[] potentialHits = Physics2D.OverlapCircleAll(selfTransform.position, explosionRadius, playerLayer);

        foreach(Collider2D collider2D in potentialHits)
        {
            if(collider2D.gameObject.tag == "Player")
            {
                collider2D.gameObject.GetComponent<Health>().DeductHealth(1);
            }
        }
    }

    private void Awake()
    {
        selfTransform = transform;
    }
}
