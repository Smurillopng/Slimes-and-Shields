using System;
using System.Collections;
using NaughtyAttributes;
using Scriptable_Objects.SO_Scripts;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [Expandable] public PlayerSO playerStats; // Referência ao scriptable object que contém os status do player

    [Space(8)][Header("Customization (Optional)")]
    public GameObject playerFace; // Boca do player
    public GameObject playerBody; // Corpo do player
    public GameObject playerShine; // Brilho do player
    public GameObject playerOutline; // Contorno do player
    public GameObject playerShield; // Escudo do player

    [Space(8)][Header("Special Attack Effects")]
    [ReadOnly] public float specialDuration; // Duração do efeito da habilidade especial
    [ReadOnly] public int specialDamagePerSecond; // Dano causado por segundo
    [ReadOnly] public int specialChance; // Chance de acertar o debuff
    
    private const float RotationSpeed = 2f; // Velocidade de rotação do player
    private Rigidbody _rigidbody; // Referência ao rigidbody do player
    public Animator animator; // Referência ao animator do player
    private bool _isBlinking; // bool que indica se o player está atacando

    [Space(8)][ReadOnly] public bool isDead;

    private Vector3 _forward, _right; // As direções do forward e right

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Died = Animator.StringToHash("Died");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Walk = Animator.StringToHash("Walk");
    
    public static int attack { get; } = Attack;
    public int walk { get; } = Walk;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>(); // Referência ao rigidbody do player
        playerStats.ResetStats(); // Reseta os status do player
        InitializeCamera(); // Inicializa a câmera
        DontDestroyOnLoad(this.gameObject);
    }

    private void InitializeCamera() // Define a posição inicial da câmera
    {
        if (Camera.main != null) _forward = Camera.main.transform.forward; // Seta a direção do forward
        _forward.y = 0; // Ignora o eixo y do forward
        _forward = Vector3.Normalize(_forward); // Normaliza o forward
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward; // Seta a direção do right
    }

    private void TakeDamage(int damage)
    {
        playerStats.currentHealth -= damage - playerStats.defense; // Diminui a vida do player
        animator.SetTrigger(Hit);
        if (_isBlinking) return;
        StartCoroutine(DamageBlink()); // Inicia a coroutine que faz o player piscar
        if (playerStats.currentHealth > 0) return;
        playerStats.currentHealth = 0;  // Se a vida for menor ou igual a zero, seta ela como zero
        if (isDead) return;
        StartCoroutine(Die()); // O player morre
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DebuffManager>())
        {
            other.GetComponent<DebuffManager>().duration = specialDuration; // Define o tempo de duração do debuff
            other.GetComponent<DebuffManager>().damagePerSecond = specialDamagePerSecond; // Define o dano por segundo do debuff
            other.GetComponent<DebuffManager>().chance = specialChance; // Define a chance de acerto do debuff     
        }

        if (other.gameObject.CompareTag("Projectile")) { TakeDamage(other.GetComponent<Projectile>().damage); }
        if (!other.gameObject.CompareTag("EnemyAttack") || isDead) return;
        if (other.GetComponentInParent<EnemyController>())
        {
            TakeDamage(other.GetComponentInParent<EnemyController>().enemyStats.damage);
            other.GetComponentInParent<EnemyController>().attackCollider.enabled = false; // Desativa o collider de ataque do inimigo
        }
        if (other.GetComponentInParent<EnemyMagic>())
        {
            TakeDamage(other.GetComponentInParent<EnemyMagic>().enemyStats.damage);
            other.GetComponentInParent<EnemyMagic>().attackCollider.enabled = false; // Desativa o collider de ataque do inimigo
        }
    }

    private IEnumerator Die()
    {
        isDead = true; // Seta o bool para true
        animator.SetBool(Walk, false);
        animator.SetBool(attack, false);
        animator.SetBool(Hit, false);
	    animator.SetTrigger(Died);
	    DialogueManager.PlayAlwaysRandom("Death");
        yield return new WaitForSeconds(2f); // Espera 2 segundos
	    gameObject.SetActive(false); // Desativa o player
	    SceneManager.LoadScene("deathScene");
    }
    
	private IEnumerator Bug()
	{
		isDead = true; // Seta o bool para true
		DialogueManager.PlayAlwaysRandom("Death");
		yield return new WaitForSeconds(2f); // Espera 2 segundos
		gameObject.SetActive(false); // Desativa o player
		SceneManager.LoadScene("deathScene");
	}

    private Color _origin;
    private IEnumerator DamageBlink()
    {
        _isBlinking = true; // Seta o bool para true
        _origin = playerBody.GetComponent<SkinnedMeshRenderer>().material.color;
        yield return new WaitForSeconds(0.1f);
        playerBody.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        playerBody.GetComponent<SkinnedMeshRenderer>().material.color = _origin;
        _isBlinking = false; // Seta o bool para true
    }

    private void FixedUpdate() 
    {
        var direction = _forward * Input.GetAxis("VK") + _right * Input.GetAxis("HK"); // Direção do movimento do player
        direction.Normalize();
        _rigidbody.velocity = new Vector3(0,0,0);
        
        if (isDead) return;
        if (Camera.main != null && Math.Abs(Time.timeScale - 1) < 0.1f)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Cria um raio a partir da posição do mouse
            if (Physics.Raycast(ray, out var hit, 100)) // Se o raio colidiu com algo
            {
                var lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z); // Define a posição do lookAt
                transform.LookAt(lookAt); // Faz o player olhar para a posição do lookAt
            }
        }

        Transform playerTransform; // Transform do player
        (playerTransform = transform).rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), Mathf.Round(Time.deltaTime * RotationSpeed)); // Faz o player rodar de acordo com o input
        
        playerTransform.position += direction * (playerStats.speed * Time.deltaTime); // Move o player para a direção do movimento
        
	    if (Input.GetAxis("HK") != 0 || Input.GetAxis("VK") != 0) { animator.SetBool(Walk, true); }
	    if (transform.position.y < -1) 
	    { 
	    	playerStats.currentHealth = 0;
	    	StartCoroutine(Bug());
	    }
    }
}
