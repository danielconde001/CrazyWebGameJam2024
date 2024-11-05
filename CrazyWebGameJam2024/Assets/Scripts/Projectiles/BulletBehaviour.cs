using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private string enemyTag;
    [SerializeField] private float bulletSpeed;
    private Vector2 bulletDirection = Vector2.right;
    private bool canPenetrate = false;
    private Rigidbody2D selfRigidbody2D;

    private void Awake()
    {
        selfRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    private void Update()
    {
        //transform.position += (Vector3)bulletDirection * bulletSpeed * Time.deltaTime;
        selfRigidbody2D.linearVelocity = bulletDirection * bulletSpeed * Time.fixedDeltaTime;
    }

    public void SetBulletDirection(Vector2 newDirection)
    {
        bulletDirection = newDirection;
    }

    public void SetCanPenetrate(bool _canPenetrate)
    {
        canPenetrate = _canPenetrate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy Shield" && enemyTag == "Enemy")
        {
            collision.GetComponent<EnemyShield>().ShieldHit(canPenetrate);
            Destroy(gameObject);
        }

        else if (collision.tag == enemyTag)
        {
            collision.GetComponent<Health>().DeductHealth(1);
            Destroy(gameObject);
        }

        else if(collision.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
