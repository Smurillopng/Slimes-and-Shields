using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Lower Special Cooldown", order = 0)]
    public class LowerCooldown : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public float cooldownReduction; // Quantidade de dano a ser adicionada
        
        public override void Apply(GameObject target)
        {
            target.GetComponent<ShieldBounce>().cooldownTime -= cooldownReduction; // Aumenta o ataque do player

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