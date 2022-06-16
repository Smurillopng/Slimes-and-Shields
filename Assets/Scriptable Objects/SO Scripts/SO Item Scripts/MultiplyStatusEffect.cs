using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Multiply Status", order = 1)]
    public class MultiplyStatusEffect : ItemSO
    {
        [Space(5)][Header("Item Settings")]
	    public int multiplier; // Multiplicador do efeito
        public Status status; // Lista de possíveis status que podem ser multiplicados

	    private int _statValue; // Valor do status
        public enum Status { MaxHealth, MaxMana, Attack, MagicAttack, Speed, Defense }; 
        public override void Apply(GameObject target)
        {
            GetStat(target); // Pega o valor do status
            _statValue *= multiplier; // Multiplica o valor do status pelo multiplicador
            SetStat(target); // Seta o valor do status

            if (enableCustomMaterial) 
            {
                CustomMaterial(target); // Aplica o material customizado
            }
            if (enableCustomColor)
            {
                CustomColor(target); // Aplica a cor customizada
            }
        }

        private void GetStat(GameObject target)
        {
            _statValue = target.GetComponent<PlayerController>().playerStats.GetStats((int)status); // Pega o valor do status
        }

        private void SetStat(GameObject target)
        {
	        target.GetComponent<PlayerController>().playerStats.SetStats((int)status, _statValue); // Seta o valor do status
        }
    }
}