using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private string enemyTag;
    [SerializeField] private float bulletSpeed;
    private Vector2 bulletDirection = Vector2.right;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == enemyTag)
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
