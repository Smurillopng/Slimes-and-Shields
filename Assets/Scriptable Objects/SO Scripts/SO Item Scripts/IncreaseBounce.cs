using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Increase Bounce", order = 0)]
    public class IncreaseBounce : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public int addBounce; // Quantidade de dano a ser adicionada
        
        public override void Apply(GameObject target)
        {
            target.GetComponentInChildren<ShieldBounce>().totalHits += addBounce; // Aumenta o ataque do player

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