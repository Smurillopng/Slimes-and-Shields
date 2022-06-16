using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Restore Health", order = 1)]
    public class RestoreHealth : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public int healthRestored; // Quantidade de mana a ser adicionada
        
        public override void Apply(GameObject target)
        {
            target.GetComponent<PlayerController>().playerStats.currentHealth += healthRestored; 
            
            if (target.GetComponent<PlayerController>().playerStats.currentHealth >
                target.GetComponent<PlayerController>().playerStats.maxHealth)
            {
                target.GetComponent<PlayerController>().playerStats.currentHealth =
                    target.GetComponent<PlayerController>().playerStats.maxHealth; 
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