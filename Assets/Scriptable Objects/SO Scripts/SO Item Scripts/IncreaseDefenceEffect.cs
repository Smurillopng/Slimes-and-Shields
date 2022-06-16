using UnityEngine;

namespace Scriptable_Objects.SO_Scripts.SO_Item_Scripts
{
    [CreateAssetMenu(fileName = "New Item", menuName = "SO/New Item/Increase Defence", order = 1)]
    public class IncreaseDefenceEffect : ItemSO
    {
        [Space(5)][Header("Item Settings")]
        public int addDefence; // Quantidade de defesa a ser adicionada
        
        public override void Apply(GameObject target)
        {
            target.GetComponent<PlayerController>().playerStats.defense += addDefence; // Aumenta a defesa do player
            
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