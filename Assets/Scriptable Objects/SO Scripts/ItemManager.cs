using NaughtyAttributes;
using UnityEngine;

namespace Scriptable_Objects.SO_Scripts
{
    [CreateAssetMenu(fileName = "ItemManager", menuName = "SO/Managers/ItemManager", order = 0)]
    public class ItemManager : ScriptableObject
    {
        [Space(10)][Expandable] public ItemSO[] commonItems; // Itens comuns
        [Space(10)][Expandable] public ItemSO[] rareItems; // Itens raros
        [Space(10)][Expandable] public ItemSO[] legendaryItems; // Itens lendários
    }
}
