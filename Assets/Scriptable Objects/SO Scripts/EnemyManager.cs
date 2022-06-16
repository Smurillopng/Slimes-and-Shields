using UnityEngine;

namespace Scriptable_Objects.SO_Scripts
{
    [CreateAssetMenu(fileName = "EnemyManager", menuName = "SO/Managers/EnemyManager", order = 0)]
    public class EnemyManager : ScriptableObject
    {
        [Space(10)] public GameObject[] commonEnemies; // Inimigos fáceis
        [Space(10)] public GameObject[] rareEnemies; // Inimigos normais
        [Space(10)] public GameObject[] epicEnemies; // Inimigos difíceis
    }
}