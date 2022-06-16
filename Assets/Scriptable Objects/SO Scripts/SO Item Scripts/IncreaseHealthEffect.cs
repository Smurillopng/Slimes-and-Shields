using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Increase Health", order = 1)]
    public class IncreaseHealthEffect : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public int addMaxHealth; // Quantidade de vida a ser adicionada

        public override void Apply(GameObject target)
        {
            target.GetComponent<PlayerController>().playerStats.maxHealth += addMaxHealth; // Aumenta a vida máxima do player
            target.GetComponent<PlayerController>().playerStats.currentHealth += addMaxHealth; // Aumenta a vida atual do player
            
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