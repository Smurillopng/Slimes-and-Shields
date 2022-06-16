using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebuffManager : MonoBehaviour
{
    [ReadOnly] public bool isPoisoned; // Veneno
    [ReadOnly] public bool isSlowed; // Lentidão
    [ReadOnly] public bool isBurning; // Queimadura
    [ReadOnly] public bool isFrozen; // Congelamento
    [ReadOnly] public bool isStunned; // Atordoamento
    [ReadOnly] public bool isRooted; // Enraizamento
    [ReadOnly] private float _seconds; // Segundos de duração do debuff
    [ReadOnly] private float _valueChange; // Verifica se o valor mudou
    
    [ReadOnly] public float duration; // Duração do debuff
    [ReadOnly] public int damagePerSecond; // Dano por segundo
    [ReadOnly] public int chance; // 1 em x chances de aplicar o debuff
    
    private void Start()
    {
        _valueChange = _seconds; // Seta o valor inicial do timer
    }

    public IEnumerator Poison()
    {
        var poisonChance = Random.Range(0, 100); // Gera um numero aleatorio entre 0 e 100
        if (poisonChance < 100 - chance) yield break;
        isPoisoned = true; // Se o numero aleatorio for maior que a chance de envenenar, o inimigo esta envenenado
        var startTime = 0f; // Tempo inicial
        while (_seconds < duration) // Enquanto o tempo for menor que o tempo de duração
        {
            startTime += Time.deltaTime; // Adiciona tempo ao tempo inicial
            _seconds = startTime % 60; // Calcula o tempo restante
            if ((int)_seconds != (int)_valueChange) // Se o tempo for diferente do valor inicial
            {
                _valueChange = _seconds; // Atualiza o valor inicial
                if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().TakeDamage(damagePerSecond); }
                if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().TakeDamage(damagePerSecond); }
            }
            yield return null;
        }
        _seconds = 0; // Zera o tempo
        isPoisoned = false; // O inimigo nao esta mais envenenado
    }

    public IEnumerator Slow()
    {
        var slowChance = Random.Range(0, 100); // Gera um numero aleatorio entre 0 e 100
        var originalSpeed = 0f;
        if (GetComponent<EnemyController>()) { originalSpeed = GetComponent<EnemyController>().enemyStats.speed; }
        if (GetComponent<EnemyMagic>()) { originalSpeed = GetComponent<EnemyMagic>().enemyStats.speed; }
        if (slowChance < 100 - chance) yield break;
        isSlowed = true; // Se o numero aleatorio for maior que a chance de deixar lento, o inimigo esta lento
        var slowSpeed = 0f;
        if (GetComponent<EnemyController>()) { slowSpeed = GetComponent<EnemyController>().enemyStats.speed / 2; }
        if (GetComponent<EnemyMagic>()) { slowSpeed = GetComponent<EnemyMagic>().enemyStats.speed / 2; }
        var startTime = 0f; // Tempo inicial
        while (_seconds < duration) // Enquanto o tempo for menor que o tempo de duração
        {
            startTime += Time.deltaTime; // Adiciona tempo ao tempo inicial
            _seconds = startTime % 60; // Calcula o tempo restante
            if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().enemyStats.speed = slowSpeed; }
            if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().enemyStats.speed = slowSpeed; }
            yield return null;
        }
        if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().enemyStats.speed = originalSpeed; }
        if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().enemyStats.speed = originalSpeed; }
        _seconds = 0; // Zera o tempo
        isSlowed = false; // O inimigo nao esta mais lento
    }
    
    public IEnumerator Burn()
    {
        var burnChance = Random.Range(0, 100); // Gera um numero aleatorio entre 0 e 100
        if (burnChance < 100 - chance) yield break;
        isBurning = true; // Se o numero aleatorio for maior que a chance de queimar, o inimigo esta queimando
        var startTime = 0f; // Tempo inicial
        while (_seconds < duration) // Enquanto o tempo for menor que o tempo de duração
        {
            startTime += Time.deltaTime; // Adiciona tempo ao tempo inicial
            _seconds = startTime % 60; // Calcula o tempo restante
            if ((int)_seconds != (int)_valueChange) // Se o tempo for diferente do valor inicial
            {
                _valueChange = _seconds; // Atualiza o valor inicial
                if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().TakeDamage(damagePerSecond); }
                if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().TakeDamage(damagePerSecond); }
            }
            yield return null;
        }
        _seconds = 0; // Zera o tempo
        isBurning = false; // O inimigo nao esta mais queimando
    }
    
    public IEnumerator Freeze()
    {
        var freezeChance = Random.Range(0, 100); // Gera um numero aleatorio entre 0 e 100
        var speed = 0f;
        if (GetComponent<EnemyController>()) { speed = GetComponent<EnemyController>().enemyStats.speed; }
        if (GetComponent<EnemyMagic>()) { speed = GetComponent<EnemyMagic>().enemyStats.speed; }
        if (freezeChance < 100 - chance) yield break;
        isFrozen = true; // Se o numero aleatorio for maior que a chance de congelar, o inimigo esta congelado
        var startTime = 0f; // Tempo inicial
        while (_seconds < duration)
        {
            if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().enemyStats.speed = 0; }
            if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().enemyStats.speed = 0; }
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // Congela a posição e a rotação
            GetComponent<Animator>().speed = 0; // Congela a animação
            startTime += Time.deltaTime; // Adiciona tempo ao tempo inicial
            _seconds = startTime % 60; // Calcula o tempo restante
            yield return null;
        }
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // Retorna as restrições de posição e rotação
        if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().enemyStats.speed = speed; }
        if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().enemyStats.speed = speed; }
        GetComponent<Animator>().speed = 1; // Retorna a velocidade da animação
        _seconds = 0; // Zera o tempo
        isFrozen = false; // O inimigo nao esta mais congelado
    }
    
    public IEnumerator Stun()
    {
        var stunChance = Random.Range(0, 100); // Gera um numero aleatorio entre 0 e 100
        var speed = 0f;
        if (GetComponent<EnemyController>()) { speed = GetComponent<EnemyController>().enemyStats.speed; }
        if (GetComponent<EnemyMagic>()) { speed = GetComponent<EnemyMagic>().enemyStats.speed; }
        if (stunChance < 100 - chance) yield break;
        isStunned = true; // Se o numero aleatorio for maior que a chance de stunar, o inimigo esta stunado
        var startTime = 0f; // Tempo inicial
        while (_seconds < duration)
        {
            if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().enemyStats.speed = 0; }
            if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().enemyStats.speed = 0; }
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll; // Congela a posição e a rotação
            GetComponent<Animator>().speed = 0; // Congela a animação
            startTime += Time.deltaTime; // Adiciona tempo ao tempo inicial
            _seconds = startTime % 60; // Calcula o tempo restante
            yield return null;
        }
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // Retorna as restrições de posição e rotação
        if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().enemyStats.speed = speed; }
        if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().enemyStats.speed = speed; }
        GetComponent<Animator>().speed = 1; // Retorna a velocidade da animação
        _seconds = 0; // Zera o tempo
        isStunned = false; // O inimigo nao esta mais stunado
    }
    
    public IEnumerator Root()
    {
        var rootChance = Random.Range(0, 100); // Gera um numero aleatorio entre 0 e 100
        var speed = 0f;
        if (GetComponent<EnemyController>()) { speed = GetComponent<EnemyController>().enemyStats.speed; }
        if (GetComponent<EnemyMagic>()) { speed = GetComponent<EnemyMagic>().enemyStats.speed; }
        if (rootChance < 100 - chance) yield break;
        isRooted = true; // Se o numero aleatorio for maior que a chance de enraizar, o inimigo esta enraizado
        var startTime = 0f; // Tempo inicial
        while (_seconds < duration)
        {
            if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().enemyStats.speed = 0; }
            if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().enemyStats.speed = 0; }
            startTime += Time.deltaTime; // Adiciona tempo ao tempo inicial
            _seconds = startTime % 60; // Calcula o tempo restante
            yield return null;
        }
        if (GetComponent<EnemyController>()) { GetComponent<EnemyController>().enemyStats.speed = speed; }
        if (GetComponent<EnemyMagic>()) { GetComponent<EnemyMagic>().enemyStats.speed = speed; }
        _seconds = 0; // Zera o tempo
        isRooted = false; // O inimigo nao esta mais enraizado
    }
}