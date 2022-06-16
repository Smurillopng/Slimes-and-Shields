using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Restore Mana", order = 1)]
    public class RestoreMana : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public int manaRestored; // Quantidade de mana a ser adicionada
        
        public override void Apply(GameObject target)
        {
            target.GetComponent<PlayerController>().playerStats.currentMana += manaRestored; 
            
            if (target.GetComponent<PlayerController>().playerStats.currentMana >
                target.GetComponent<PlayerController>().playerStats.maxMana)
            {
                target.GetComponent<PlayerController>().playerStats.currentMana =
                    target.GetComponent<PlayerController>().playerStats.maxMana; 
            }

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