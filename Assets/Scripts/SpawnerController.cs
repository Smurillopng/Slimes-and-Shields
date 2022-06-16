using System.Collections.Generic;
using NaughtyAttributes;
using Scriptable_Objects.SO_Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerController : MonoBehaviour
{
        [Expandable] public EnemyManager enemyManager;
        [SerializeField][ReadOnly] private DadosRoda dadosRoda;
        [SerializeField][ReadOnly] private GameObject player;
        public float spawnDistance = 4f;
        [ReadOnly] public List<GameObject> spawnedEnemies = new List<GameObject>();

        private void Awake()
        {
                dadosRoda = FindObjectOfType<DadosRoda>();
                player = FindObjectOfType<PlayerController>().gameObject;
                player = GameObject.Find("player_target");
        }
        
        public void SpawnEnemy()
        {
                var playerPos = player.transform.position;
                var playerDir = player.transform.forward;
                var playerRot = player.transform.rotation;
                
                var spawnPos = playerPos + playerDir * spawnDistance;

                switch (dadosRoda.result)
                {
                        case >= 1 and <= 10:
                        {
                                var d3Enemy1 = Instantiate(enemyManager.epicEnemies[Random.Range(0, enemyManager.epicEnemies.Length)], spawnPos, playerRot);
                                spawnedEnemies.Add(d3Enemy1);
                                var distance1 = d3Enemy1.transform.position + new Vector3(2, 2, 0);
                                var d3Enemy2 = Instantiate(enemyManager.rareEnemies[Random.Range(0, enemyManager.rareEnemies.Length)], distance1, playerRot);
                                spawnedEnemies.Add(d3Enemy2);
                                var distance2 = d3Enemy1.transform.position + new Vector3(-2, -2, 0);
                                var d3Enemy3 = Instantiate(enemyManager.rareEnemies[Random.Range(0, enemyManager.rareEnemies.Length)], distance2, playerRot);
                                spawnedEnemies.Add(d3Enemy3);
                                break;
                        }
                        case >= 11 and <= 15:
                        {
                                var d2Enemy1 = Instantiate(enemyManager.rareEnemies[Random.Range(0, enemyManager.rareEnemies.Length)], spawnPos, playerRot);
                                spawnedEnemies.Add(d2Enemy1);
                                var distance = d2Enemy1.transform.position + new Vector3(2, 2, 0);
                                var d2Enemy2 = Instantiate(enemyManager.rareEnemies[Random.Range(0, enemyManager.rareEnemies.Length)], distance, playerRot);
                                spawnedEnemies.Add(d2Enemy2);
                                break;
                        }
                        case >= 16 and <= 20:
                        {
                                var d1Enemy = Instantiate(enemyManager.commonEnemies[Random.Range(0, enemyManager.commonEnemies.Length)], spawnPos, playerRot);
                                spawnedEnemies.Add(d1Enemy);
                                break;
                        }
                }
        }
}