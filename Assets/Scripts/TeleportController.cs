using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public GameObject cam, camToFollow; // Camera
	public GameObject player; // Player
	public GameObject miniMapCam; // Camera do Minimapa
	public GameObject miniMapPlayer; // Icone do player no miniMapa
    public GameObject tpTarget; // Ponto de teleporte do player
	public GameObject camTarget;  // Ponto de teleporte da camera
	public GameObject tpIconTarget; // Ponto de teleporte do player
	public GameObject miniMapCamTarget;  // Ponto de teleporte da camera
	[ReadOnly] public RoomCheck roomCheck; // Verifica se o player está dentro de uma sala
    [ReadOnly] public StageManager stageManager; // Gerencia o estado do jogo
    private bool _isMoving;

    private DadosRoda _dadosRoda;
    public SpawnerController spawnerController;

    private void Awake()
    {
	    cam = GameObject.Find("Main Camera"); // Busca a camera
	    camToFollow = GameObject.FindGameObjectWithTag("RefCamera");
	    player = GameObject.Find("obj_player"); // Busca o player
	    miniMapCam = GameObject.Find("Minimap Camera");// Busca a camera do miniMapa
	    miniMapPlayer = GameObject.Find("player_icon"); // Busca o icone do player
	    _dadosRoda = FindObjectOfType<DadosRoda>();
        spawnerController = FindObjectOfType<SpawnerController>();
        roomCheck = transform.parent.GetComponentInParent<RoomCheck>();
        stageManager = FindObjectOfType<StageManager>();
    }

    private void Update()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = spawnerController.spawnedEnemies.Count <= 0;

        if (cam is null) return; 
        if (cam.transform.position == camToFollow.transform.position) return;
        cam.transform.position = Vector3.MoveTowards(cam.transform.position, camToFollow.transform.position, Time.unscaledDeltaTime * 2);
    }
    
    private void OnTriggerEnter(Collider other)
    {
	    if (!other.CompareTag("Player")) return;
	    player.transform.position = tpTarget.transform.position; // Teleporta o player para o ponto de teleporte
	    camToFollow.transform.position = camTarget.transform.position; // Teleporta a camera para o ponto de teleporte
		miniMapPlayer.transform.position = tpIconTarget.transform.position; // Teleporta o icone do player para o ponto de teleporte
		miniMapCam.transform.position = miniMapCamTarget.transform.position; // Teleporta a camera para o ponto de teleporte

		if (roomCheck.wasVisited is false)
        {
            _dadosRoda.roll = true;
            StartCoroutine(SpawnEnemy());
	        roomCheck.wasVisited = true;
	        roomCheck.minimapIcon.SetActive(true);
        }

        if (roomCheck.itemRoom)
        {
            _dadosRoda.roll = true;
            StartCoroutine(SpawnItem());
            roomCheck.wasVisited = true;
	        roomCheck.itemRoom = false;
	        roomCheck.minimapIcon.SetActive(true);
        }

        if (!roomCheck.bossTeleportRoom) return;
        StartCoroutine(SpawnBossTeleport());
	    roomCheck.wasVisited = true;
	    roomCheck.bossTeleportRoom = false;
	    roomCheck.minimapIcon.SetActive(true);
    }

    private IEnumerator SpawnEnemy() // Chama o spawner
    {
        yield return new WaitForSeconds(0.001f); // Espera 1 segundo
        player.transform.LookAt(transform.parent.transform); // Faz o player olhar para o ponto de teleporte
        spawnerController.SpawnEnemy(); // Chama o spawner
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(0.1f); // Espera 1 segundo
        stageManager.SpawnItem(transform.parent.gameObject); // Chama o spawner
    }
	private IEnumerator SpawnBossTeleport()
	{
        yield return new WaitForSeconds(0.1f); // Espera 1 segundo
        stageManager.SpawnBossTeleport(transform.parent.gameObject); // Chama o spawner do teleporte do boss
	}
}