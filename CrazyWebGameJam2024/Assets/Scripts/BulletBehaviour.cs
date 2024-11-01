using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    private Vector2 bulletDirection = Vector2.right;

    private void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    private void Update()
    {
        transform.position += (Vector3)bulletDirection * bulletSpeed * Time.deltaTime;
    }

    public void SetBulletDirection(Vector2 newDirection)
    {
        bulletDirection = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Health>().DeductHealth(1);
            Destroy(gameObject);
        }
    }
}
