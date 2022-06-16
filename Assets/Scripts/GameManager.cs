using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static UnityAction _initializeGame;
    public GameObject cameraMain;
    private GameObject _target; // Variavel pública que recebe o objeto do jogador
    private RoomController _roomController; // Variavel pública que recebe o objeto do RoomController
    public static string Scene;
    private void Start()
    {
        _initializeGame = StartLevel;
        _initializeGame();
        Scene = SceneManager.GetActiveScene().name;
    }
    
    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        cameraMain = GameObject.Find("Main Camera");
        _roomController = GameObject.FindObjectOfType<RoomController>();
        if (SceneManager.GetActiveScene().name is not ("bossRoom" or "bossRoom2" or "bossRoom3"))
            StartCoroutine(_roomController.GeraMapa(0));
        yield return new WaitForSeconds(0.5f);
        _target = GameObject.FindWithTag("MainRoom"); // Busca o objeto com a tag MainRoom
        _target = _target.transform.Find("camera position").gameObject; // Busca o objeto com o nome "camera position"
	    cameraMain.transform.position = _target.transform.position; // Coloca a câmera na posição do jogador mais a diferença calculada
	    if (SceneManager.GetActiveScene().name is "lvl1")
	    	//DialogueManager.PlayAlways("Intro1");
	    	//DialogueManager.PlayAlways("Intro2");
	    	//DialogueManager.PlayAlways("Intro3");
	    	//DialogueManager.PlayAlways("Intro4");
	    	DialogueManager.PlayAlwaysRandom("Entrance1");
	    if (SceneManager.GetActiveScene().name is "lvl2")
		    DialogueManager.PlayAlwaysRandom("Entrance2");
	    if (SceneManager.GetActiveScene().name is "lvl3")
		    DialogueManager.PlayAlwaysRandom("Entrance3");
	    if (SceneManager.GetActiveScene().name is "bossRoom")
		    DialogueManager.PlayAlwaysRandom("Boss1");
	    if (SceneManager.GetActiveScene().name is "bossRoom2")
		    DialogueManager.PlayAlwaysRandom("Boss2");
	    if (SceneManager.GetActiveScene().name is "bossRoom3")
		    DialogueManager.PlayAlwaysRandom("Boss3");
		
    }

	private void StartLevel() { StartCoroutine(StartGameCoroutine()); }
	
}