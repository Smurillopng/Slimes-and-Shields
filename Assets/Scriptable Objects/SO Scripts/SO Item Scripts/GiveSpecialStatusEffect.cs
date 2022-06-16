using System;
using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Special Status", order = 1)]
    public class GiveSpecialStatusEffect : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public SpecialStatus specialStatus; // O status especial que o item dará
        DebuffManager[] _debuffTargets; // O array de targets que receberão o debuff
        
        public float overrideDuration; // Se o item for usado, a duração do debuff será substituida por este valor
        public int overrideDamagePerSecond; // Se o item for usado, o dano do debuff será substituido por este valor
        public int overrideChance; // Se o item for usado, a chance de acerto do debuff será substituida por este valor

        public enum SpecialStatus { Poison, Slow, Stun, Burn, Freeze, Root } // O enum que define os status especiais
        
        public override void Apply(GameObject target)
        {
            ChangeVars(target); // Altera as variaveis do ataque especial do player
            ApplyDebuff(target); // Aplica o debuff
            

            if (enableCustomMaterial) 
            {
                CustomMaterial(target); // Aplica o material customizado
            }
            if (enableCustomColor)
            {
                CustomColor(target); // Aplica a cor customizada
            }
        }

        private void ApplyDebuff(GameObject target)
        {
            switch (specialStatus)
            {
                case SpecialStatus.Poison:
                    target.GetComponent<DebuffCheck>().canPoison = true; // O jogador recebe o status especial de poison
                    break;
                case SpecialStatus.Slow: 
                    target.GetComponent<DebuffCheck>().canSlow = true; // O jogador recebe o status especial de slow
                    break;
                case SpecialStatus.Stun:
                    target.GetComponent<DebuffCheck>().canStun = true; // O jogador recebe o status especial de stun
                    break;
                case SpecialStatus.Burn:
                    target.GetComponent<DebuffCheck>().canBurn = true; // O jogador recebe o status especial de burn
                    break;
                case SpecialStatus.Freeze:
                    target.GetComponent<DebuffCheck>().canFreeze = true; // O jogador recebe o status especial de freeze
                    break;
                case SpecialStatus.Root:
                    target.GetComponent<DebuffCheck>().canRoot = true; // O jogador recebe o status especial de root
                    break;
                default:
                    throw new ArgumentOutOfRangeException(); // Se o status não for encontrado, lança um erro
            }
        }

        private void ChangeVars(GameObject target)
        {
            target.GetComponent<PlayerController>().specialDuration = overrideDuration; // Substitui a duração do debuff
            target.GetComponent<PlayerController>().specialDamagePerSecond = overrideDamagePerSecond; // Substitui o dano do debuff   
            target.GetComponent<PlayerController>().specialChance = overrideChance; // Substitui a chance de acerto do debuff
        }
    }
}