using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class ShieldBounce : MonoBehaviour
{
    private Rigidbody _rb;
    public BoxCollider bc;
    private bool _flying;
    public float force;
    public GameObject holder;
    public GameObject attack;
    private GameObject _player;
    private bool _moving, _isAttacking, _specialOn;
    private Vector3 _lookAt;
    private PlayerController _pc;
    [SerializeField] private Animator playerAnimator;
    
    [SerializeField][ReadOnly] private bool onCooldown;
    [ReadOnly] public float cooldown;
    public float cooldownTime;

    [SerializeField][ReadOnly] private int hits;
    public int totalHits;
    private static readonly int Shield = Animator.StringToHash("Shield");

    private void Awake()
    {
        _player = transform.parent.gameObject;
        _rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        _pc = _player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeScale is 1 && !_isAttacking && !_flying && !onCooldown && _pc.playerStats.currentHealth > 0)
        {
            StartCoroutine(AttackCoroutine());
        }

        if (_isAttacking)
        {
            var shieldTransform = transform;
            shieldTransform.position = Vector3.Lerp(shieldTransform.position, attack.transform.position, Time.deltaTime * 10);
            transform.rotation = Quaternion.Slerp(shieldTransform.rotation, attack.transform.rotation, Time.deltaTime * 10);
            return;
        }

        switch (_flying)
        {
            case false:
                bc.enabled = false;
                _rb.velocity = Vector3.zero;
                var shieldTransform = transform;
                transform.position = Vector3.Lerp(shieldTransform.position, holder.transform.position, Time.deltaTime * 10);
                transform.rotation = Quaternion.Slerp(shieldTransform.rotation, holder.transform.rotation, Time.deltaTime * 10);
                _rb.isKinematic = true;
                break;
            case true:
            {
                _rb.isKinematic = false;
                transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime * 100);
                bc.enabled = true;
            
                if (!_moving)
                {
                    _lookAt = transform.forward;
                    transform.rotation = Quaternion.LookRotation(_lookAt);
                    _rb.AddForce(_lookAt * force, ForceMode.Impulse);
                    transform.SetParent(null);
                    _moving = true;
                    GetComponentInChildren<ParticleSystem>().Play();
                }
                break;
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) && Time.timeScale is 1 && !_flying && !onCooldown && !_specialOn &&  _pc.playerStats.currentMana >= 10 && _pc.playerStats.currentHealth > 0)
        {
            _specialOn = true;
            playerAnimator.SetTrigger(Shield);
            StartCoroutine(ThrowShield());
        }
        
        if(hits >= totalHits)
        {
            _flying = false;
            _moving = false;
            transform.SetParent(_player.transform);
            GetComponentInChildren<ParticleSystem>().Stop();
            hits = 0;
            onCooldown = true;
            cooldown = cooldownTime;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) > 30)
        {
            _flying = false;
            _moving = false;
            transform.SetParent(_player.transform);
            GetComponentInChildren<ParticleSystem>().Stop();
            hits = 0;
            onCooldown = true;
            cooldown = cooldownTime;
        }
        
        if (!onCooldown) return;
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            onCooldown = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player) return;
        if (other.gameObject.CompareTag("Enemy"))
        {
	        if (other.gameObject.GetComponent<EnemyController>()) { other.gameObject.GetComponent<EnemyController>().TakeDamage(_player.GetComponent<PlayerController>().playerStats.attack); }
        }
	    if (other.gameObject.CompareTag("Enemy"))
	    {
		    if (other.gameObject.GetComponent<EnemyMagic>())
		    { other.gameObject.GetComponent<EnemyMagic>().TakeDamage(_player.GetComponent<PlayerController>().playerStats.attack); }
    	}
        if (_isAttacking) return;
        Reflect();
        hits++;
    }

    private void Reflect()
    {
        var ray = new Ray(transform.position, _lookAt);
        if (!Physics.Raycast(ray, out var hit)) return;
        _lookAt = Vector3.Reflect(_lookAt, hit.normal);
        _rb.velocity = _lookAt * force;
    }

    private IEnumerator ThrowShield()
    {
        yield return new WaitForSeconds(0.2f);
        _flying = true;
        _pc.playerStats.currentMana -= _pc.playerStats.specialAttackCost;
        _specialOn = false;
    }
    
    private IEnumerator AttackCoroutine()
    {
        _isAttacking = true; // Seta o bool para true
        _pc.animator.SetBool(_pc.walk, false); // Para de andar
        _pc.animator.SetTrigger(PlayerController.attack);
        bc.enabled = true;
        yield return new WaitForSeconds(1f); // Espera 0.5 segundos
        bc.enabled = false;
        _isAttacking = false; // Seta o bool para false
        onCooldown = true;
        cooldown = cooldownTime/2;
    }
}
