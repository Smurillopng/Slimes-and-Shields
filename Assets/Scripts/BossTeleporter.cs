using System.Collections;
using Menu;
using UnityEngine;

public class BossTeleporter : MonoBehaviour
{
	private Transform _teleportTarget;
	private GameObject _player, _loadingScreen,_target;
	private MenuManager _menuManager;

	private void Awake()
	{
		_menuManager = FindObjectOfType<MenuManager>();
		_player = GameObject.FindGameObjectWithTag("Player");
		DontDestroyOnLoad(gameObject);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Player")) return;
		_menuManager.LoadGame();
		StartCoroutine(LoadBossRoom());
	}

	private IEnumerator LoadBossRoom()
	{
		yield return new WaitForSeconds(1f);
		_target = GameObject.FindWithTag("MainRoom"); // Busca o objeto com a tag MainRoom
		_target = _target.transform.Find("camera position").gameObject; // Busca o objeto com o nome "camera position"
		transform.position = _target.transform.position; // Coloca a câmera na posição do jogador mais a diferença calculada
		_teleportTarget = GameObject.FindGameObjectWithTag("BossTeleport").transform;
		_player.transform.position = _teleportTarget.position;
		yield return new WaitForSeconds(0.1f);
		Destroy(gameObject); // Destrói o item
	}
}
