using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Menu
{
	public class MenuManager : MonoBehaviour
	{
		public GameObject cam, painelMenu, painelOptions, painelCreditos, painelMapa;
		public AudioControl audioControl;// referencia a script
		public GameObject loadingScreen;// referencia ao canvas da tela de carregamento
		public VideoPlayer mainVideo, creditsVideo, optionsVideo;
		public bool mapBool, paused;
		public int indexScene;

		private void Awake()
		{
			if (SceneManager.GetActiveScene().name != "menu")
			{
				StartCoroutine(NewSceneDelay());
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				switch (paused)
				{
					case true:
						ResumeGame();
						Time.timeScale = 1;
						break;
					case false:
						LoadOptions();
						Time.timeScale = 0;
						break;
				}
			}
			if (Input.GetKeyDown(KeyCode.Tab)) BigMap();
		}
	
		//Volta para o menu inicial
		public void LoadMainMenu() 
		{
			var currentScene = SceneManager.GetActiveScene ();// Pega a cena atual
			var sceneName = currentScene.name;// Devolve o nome dessa cena
 
			switch (sceneName)
			{
				// Se for a cena onde o jogo ocorre, volta para a cena de menu
				case "lvl1" or "lvl2" or "lvl3" or "bossRoom" or "bossRoom2" or "bossRoom3":
					SceneManager.LoadScene("menu");
					Time.timeScale = 1;
					break;
					
				// Se for a cena de morte, volta para a cena de menu	
				case "deathScene" or "victoryScreen":
					SceneManager.LoadScene("menu");
					Time.timeScale = 1;
					break;
				// Se for a cena do menu principal, move a camera para o background desejado e 
				//desativa o os outros paineis exceto o menu.Options fica não interativo e com alpha = 0
				case "menu":
					mainVideo.Play();
					if (mainVideo.isPrepared)
					{
						creditsVideo.Stop();
						optionsVideo.Stop();
					}
					Time.timeScale = 1;
					cam.transform.position = new Vector3(294,-41,40);
					painelMenu.SetActive(true);
					//painelOptions.SetActive(false);
					painelOptions.GetComponent<CanvasGroup>().alpha = 0;
					painelOptions.GetComponent<CanvasGroup>().interactable = false;
					painelCreditos.SetActive(false);
					break;
			}
		}    
    
		public void LoadGame() 
		{
			if(indexScene is 7)
			{
				DialogueManager.PlayAlways("VictoryScene");
			}
			StartCoroutine(LoadMainSceneAsync(indexScene))/*SceneManager.LoadScene("lvl1")*/;
		}

		public void LoadOptions()
		{
			var currentScene = SceneManager.GetActiveScene ();// Pega a cena atual
			var sceneName = currentScene.name;// Devolve o nome dessa cena

			switch (sceneName)
			{
				case "lvl1" or "lvl2" or "lvl3" or "bossRoom" or "bossRoom2" or "bossRoom3":
					paused = true;
					painelOptions.GetComponent<CanvasGroup>().alpha = 1;
					painelOptions.GetComponent<CanvasGroup>().interactable = true;
					break;
				case "menu":
					optionsVideo.Play();
					if (optionsVideo.isPrepared) { mainVideo.Stop(); }
					cam.transform.position = new Vector3(624,-41,40);
					painelMenu.SetActive(false);
					//painelOptions.SetActive(true);
					painelOptions.GetComponent<CanvasGroup>().alpha = 1;
					painelOptions.GetComponent<CanvasGroup>().interactable = true;
					painelCreditos.SetActive(false);
					break;
			}
		}
	
		public void LoadCredits()
		{
			creditsVideo.Play();
			if (creditsVideo.isPrepared) { mainVideo.Stop(); }
			// move a camera para o background desejado e 
			//desativa o os outros paineis exceto o de Créditos. Options fica não interativo e com alpha = 0
			cam.transform.position = new Vector3(-24,-41,40);
			painelMenu.SetActive(false);
			//painelOptions.SetActive(false);
			painelCreditos.SetActive(true);
			painelOptions.GetComponent<CanvasGroup>().alpha = 0;
			painelOptions.GetComponent<CanvasGroup>().interactable = false;
			audioControl.Play("undertale");
		}
		public void FullScreen() { Screen.fullScreen = !Screen.fullScreen; }
	
		public void ResumeGame()// Despausa o jogo e desativa o painel de Options, deixando não interativo e com alpha = 0 
		{
			paused = false;
			painelOptions.GetComponent<CanvasGroup>().alpha = 0;
			painelOptions.GetComponent<CanvasGroup>().interactable = false;
			Time.timeScale = 1;
		}

		private void  BigMap()
		{
			switch (mapBool)
			{
				case false:
					painelMapa.SetActive(true);
					mapBool = true;
					Time.timeScale = 0;
					return;
				case true:
					painelMapa.SetActive(false);
					mapBool = false;
					Time.timeScale = 1;
					return;
			}
		}
		private IEnumerator LoadMainSceneAsync(int indexOfScene)
		{
			var loading = SceneManager.LoadSceneAsync(indexOfScene);

			while (!loading.isDone)
			{
				//anima��o escudo rodandinho
				loadingScreen.SetActive(true);
				yield break;
			}
		}

		private IEnumerator NewSceneDelay()
		{
			loadingScreen.SetActive(true);
			yield return new WaitForSecondsRealtime(2);
			loadingScreen.SetActive(false);
		}

		public void QuitGame() 
		{ 
			Application.Quit();
		}
	}
}
