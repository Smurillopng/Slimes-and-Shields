using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Increase Multiple Effects", order = 1)]
    public class IncreaseMutiStatsEffect : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public ItemSO[] effects; // Efeitos que serão aplicados
        
        public override void Apply(GameObject target)
        {
            foreach (var effect in effects) // Aplica cada efeito
            {
                effect.Apply(target);
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