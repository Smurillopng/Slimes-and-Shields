using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Increase Speed", order = 1)]
    public class IncreaseSpeedEffect : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public int addSpeed; // Quantidade de velocidade a ser adicionada
        
        public override void Apply(GameObject target)
        {
            target.GetComponent<PlayerController>().playerStats.speed += addSpeed; // Aumenta a velocidade do player

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