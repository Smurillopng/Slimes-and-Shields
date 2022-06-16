using UnityEngine;

namespace Scriptable_Objects.SO_Scripts
{
    [CreateAssetMenu(fileName = "New Enemies", menuName = "SO/New Enemy", order = 3)]
    public class EnemySO : ScriptableObject
    {
        [Header("Enemy Stats")]
        public int maxHealth; //  Vida máxima do inimigo
        public int health; // Vida atual do inimigo
        [Space(8)] public int baseDamage; // Dano base do inimigo
        public int damage; // Dano atual do inimigo
        [Space(8)] public float baseSpeed; // Velocidade base do inimigo
        public float speed; // Velocidade atual do inimigo
        [Space(8)] public float attackRange; // Distância de ataque do inimigo
        [Space(8)] public int attackRate; // Tempo de ataque do inimigo
        
        [Space(10)] [Header("Enemy Identifier")]
        public int enemyId; // ID do inimigo
        
        public void Instantiate() // Seta os valores iniciais do inimigo
        {
            health = maxHealth; 
            damage = baseDamage;
            speed = baseSpeed;
        }
    }
}