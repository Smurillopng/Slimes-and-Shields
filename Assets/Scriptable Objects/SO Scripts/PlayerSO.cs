using UnityEngine;

namespace Scriptable_Objects.SO_Scripts
{
    [CreateAssetMenu(fileName = "New Player", menuName = "SO/New Player", order = 2)]
    public class PlayerSO : ScriptableObject
    {
        [Header("Stats")]
        public int initialHealth; // Vida base do player
        public int maxHealth; // Vida máxima do player
        public int currentHealth; // Vida atual do player
        [Space(8)] public int initialMana; // Mana base do player
        public int maxMana; // Mana máxima do player
        public int currentMana; // Mana atual do player
        [Space(8)] public int baseAttack; // Ataque base do player
        public int attack; // Ataque atual do player
        [Space(8)] public int baseMagicAttack; // Ataque mágico base do player
        public int magicAttack; // Ataque mágico atual do player
        [Space(8)] public int baseSpeed; // Velocidade base do player
        public int speed; // Velocidade atual do player
        [Space(8)] public int baseDefense; // Defesa base do player
        public int defense; // Defesa atual do player
        [Space(8)] public int baseSpecialAttackCost; // Custo inicial do ataque especial
        public int specialAttackCost; // Custo do ataque especial

        public void ResetStats() // Reseta os valores dos atributos do player
        {
            maxHealth = initialHealth;
            maxMana = initialMana;
            currentHealth = initialHealth;
            currentMana = initialMana;
            attack = baseAttack;
            magicAttack = baseMagicAttack;
            speed = baseSpeed;
            defense = baseDefense;
            specialAttackCost = baseSpecialAttackCost;
        }

        public int GetStats(int status) => status switch // Retorna o valor de um atributo do player
        {
            0 => maxHealth,
            1 => maxMana,
            2 => attack,
            3 => magicAttack,
            4 => speed,
            5 => defense,
            _ => 6
        };
        
        public void SetStats(int status, int statusValue) // Seta o valor de um atributo do player
        {
            switch (status)
            {
                case 0:
                    maxHealth = statusValue;
                    break;
                case 1:
                    maxMana = statusValue;
                    break;
                case 2:
                    attack = statusValue;
                    break;
                case 3:
                    magicAttack = statusValue;
                    break;
                case 4:
                    speed = statusValue;
                    break;
                case 5:
                    defense = statusValue;
                    break;
            }
        }
    }
}