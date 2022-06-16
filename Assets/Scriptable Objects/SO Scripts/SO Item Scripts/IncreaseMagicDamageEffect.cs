using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Increase Magic Damage", order = 1)]
    public class IncreaseMagicDamageEffect : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public int addMagicAttack; // Quantidade de dano mágico a ser adicionada
        
        public override void Apply(GameObject target)
        {
            target.GetComponent<PlayerController>().playerStats.magicAttack += addMagicAttack; // Aumenta o ataque mágico do player

            if (enableCustomMaterial) 
            {
                CustomMaterial(target); // Aplica o material customizado
            }
            if (enableCustomColor)
            {
                CustomColor(target); // Aplica a cor customizada
            }
        }
    }
}