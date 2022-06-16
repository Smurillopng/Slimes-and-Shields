using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectile;
    public float speed;
    public float lifeTime;
    public int damage;
    public GameObject player;
    private Vector3 _targetPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _targetPosition = player.transform.position;
    }

    private void Update()
    {
        if (lifeTime <= 0) { Destroy(gameObject); }
        else { lifeTime -= Time.deltaTime; }
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) return;
        if (other.gameObject.CompareTag("ProjectileAttack")) return;
        Destroy(gameObject);
    }
}