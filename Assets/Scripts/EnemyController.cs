﻿using System.Collections;
using Menu;
using NaughtyAttributes;
using Scriptable_Objects.SO_Scripts;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField][Expandable] private EnemySO enemyPreset; // O tipo de inimigo
    [Expandable] public EnemySO enemyStats; // O tipo de inimigo
    private Animator _enemyAnimator; // Animator do inimigo
    public AudioControl audioControl; // O controle de audio
    private SpawnerController _spawnerController; // referencia ao spawnerController
    public Collider attackCollider; // Collider de ataque
    private Collider _enemyCollider; // Collider do inimigo
    private bool _isDead, _isAttacking, _isBlinking; // Se o inimigo esta morto
    public bool morreu { get; private set; } // Se o inimigo esta morto
    private GameObject _player; // O player
    private Material _currentMaterial, _blinkMaterial; // O material do inimigo
    [SerializeField] private SkinnedMeshRenderer damageTarget; // O mesh do inimigo
    public GameObject manaDrop, healthDrop; // O item que o inimigo dropa
    public int manaDropChance, healthDropChance; // A chance de dropar o item

    private static readonly int Attack = Animator.StringToHash("Attack"); // Hash do trigger de ataque
    private static readonly int Hit = Animator.StringToHash("Hit"); // Hash do trigger de hit
    private static readonly int Dead = Animator.StringToHash("Dead"); // Hash do trigger de morte
    private static readonly int Speed = Animator.StringToHash("Speed"); // Hash do trigger de correr
    
    private Rigidbody _rigidbody;

    private void Awake()
    {
        audioControl = FindObjectOfType<AudioControl>();
        _rigidbody = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player");
        enemyStats = Instantiate(enemyPreset);
        _enemyCollider = GetComponent<BoxCollider>();
        _enemyAnimator = GetComponent<Animator>(); // Busca o Animator do inimigo
        _spawnerController = FindObjectOfType<SpawnerController>(); // Busca o SpawnerController
    }

    private void Start()
    {
        _blinkMaterial = new Material(damageTarget.material) { color = Color.red };
        enemyStats.Instantiate(); // Pega os valores dos status do inimigo
        attackCollider.enabled = false; // Desabilita o collider de ataque
    }

    private void Update()
    {
        _rigidbody.velocity = new Vector3(0,0,0);
        transform.position = new Vector3(transform.position.x, 0 , transform.position.z);
        var direction = _player.transform.position - transform.position;
        direction.y = 0;

        if (enemyStats.health > 0)
        {
            if (_player.GetComponent<PlayerController>().isDead) return; // Se o player estiver morto, não faz nada
            
            if (direction.magnitude < enemyStats.attackRange)
            {
                if (!_isAttacking)
                {
                    _isAttacking = true;
                    StartCoroutine(EnableAttack());
                }
            }

            switch (_isAttacking)
            {
                case true:
                    break;
                case false:
                    _enemyAnimator.SetFloat(Speed, enemyStats.speed); // Seta a velocidade do inimigo
                    transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, enemyStats.speed * Time.deltaTime);
                    break;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }
        if (enemyStats.health > 0) return; // Se a vida for maior que 0, não faz nada
        _enemyAnimator.SetFloat(Speed, 0);
        enemyStats.health = 0; // Se a vida for menor ou igual a zero, zera a vida
        _isDead = true; // Seta que o inimigo está morto
        if (gameObject != _isDead) return;
        StartCoroutine(KillEnemy(2)); // Toca a animação de morte
    }

    public void TakeDamage(int damage)
    {
        _enemyAnimator.SetTrigger(Hit); // Ativa a animação de hit
        switch (enemyStats.enemyId)
        {
            case 1:
                audioControl.Play("boss_hit_sound");
                break;
            case 2:
                audioControl.Play("goblin_hit_sound");
                break;
            case 3:
                audioControl.Play("goblin_hit_sound");
                break;
            case 5:
                audioControl.Play("rat_hit_sound");
                break;
            case 6:
                audioControl.Play("skeleton_hit_sound");
                break;
            case 8:
                audioControl.Play("skeleton_hit_sound");
                break;
        }
        attackCollider.enabled = false; // Desativa o collider de ataque
        if (!_isBlinking) { StartCoroutine(Blink()); } // Se o inimigo não estiver piscando, pisca
        enemyStats.health -= damage; // Diminui a vida do inimigo
    }

    private IEnumerator Blink()
    {
        _isBlinking = true;
        _currentMaterial = damageTarget.material; // Pega o material do inimigo
        yield return new WaitForSeconds(0.1f);
        damageTarget.material = _blinkMaterial; // Seta o material do inimigo para o blink>()
        yield return new WaitForSeconds(0.1f);
        damageTarget.material = _currentMaterial; // Seta o material do inimigo para o blink>()
        _isBlinking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerCheck = FindObjectOfType<DebuffCheck>();
        var enemyCheck = GetComponent<DebuffManager>();
        
        attackCollider.enabled = false;
        if (!other.gameObject.CompareTag("PlayerShield")) return;
        if (playerCheck.canPoison && enemyCheck.isPoisoned == false) { StartCoroutine(enemyCheck.Poison()); }
        if (playerCheck.canSlow   && enemyCheck.isSlowed   == false) { StartCoroutine(enemyCheck.Slow());   }
        if (playerCheck.canBurn   && enemyCheck.isBurning  == false) { StartCoroutine(enemyCheck.Burn());   }
        if (playerCheck.canFreeze && enemyCheck.isFrozen   == false) { StartCoroutine(enemyCheck.Freeze()); }
        if (playerCheck.canStun   && enemyCheck.isStunned  == false) { StartCoroutine(enemyCheck.Stun());   }
        if (playerCheck.canRoot   && enemyCheck.isRooted   == false) { StartCoroutine(enemyCheck.Root());   }
    }
    
    private IEnumerator KillEnemy(float time)
    {
        if (morreu) yield break;
        morreu = true;
        _enemyAnimator.ResetTrigger(Attack);
        _enemyAnimator.ResetTrigger(Hit);
        _enemyAnimator.SetFloat(Speed, 0);
        _enemyAnimator.SetTrigger(Dead); // Ativa a animação de morte
        switch (enemyStats.enemyId)
        {
            case 1:
                audioControl.Play("boss_death_sound");
                break;
            case 2:
                audioControl.Play("goblin_death_sound");
                break;
            case 3:
                audioControl.Play("goblin_death_sound");
                break;
            case 5:
                audioControl.Play("rat_death_sound");
                break;
            case 6:
                audioControl.Play("skeleton_death_sound");
                break;
            case 8:
                audioControl.Play("skeleton_death_sound");
                break;
        }
        _enemyCollider.enabled = false; // Desativa o collider
        if (Random.Range(0, 100) < manaDropChance)
        {
            Instantiate(manaDrop, transform.position, Quaternion.identity);
        }
        if (Random.Range(0, 100) < healthDropChance)
        {
            Instantiate(healthDrop, transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(time); // Espera o tempo
        Destroy(gameObject); // Destroi o inimigo
        if (gameObject.name == "obj_enemy_boss") yield break;
        _spawnerController.spawnedEnemies.Remove(gameObject); // Remove o inimigo da lista de inimigos spawnados
    }

    private IEnumerator EnableAttack()
    {
        _isAttacking = true;
        _enemyAnimator.SetTrigger(Attack);
        switch (enemyStats.enemyId)
        {
            case 1:
                audioControl.Play("boss_attack_sound");
                break;
            case 2:
                audioControl.Play("goblin_attack_sound");
                break;
            case 3:
                audioControl.Play("goblin_attack_sound");
                break;
            case 5:
                audioControl.Play("rat_attack_sound");
                break;
            case 6:
                audioControl.Play("skeleton_attack_sound");
                break;
            case 8:
                audioControl.Play("skeleton_attack_sound");
                break;
        }
        yield return new WaitForSeconds(0.2f); // Espera 0.2 segundos
        attackCollider.enabled = true; // Ativa o collider de ataque
        yield return new WaitForSeconds(_enemyAnimator.GetCurrentAnimatorStateInfo(0).length);
        attackCollider.enabled = false; // Desativa o collider de ataque
        _enemyAnimator.ResetTrigger(Attack);
        _isAttacking = false;
    }
}