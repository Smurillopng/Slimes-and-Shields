using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Menu;
using NaughtyAttributes;
using Scriptable_Objects.SO_Scripts;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Utils;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueTmp, itemTmp; // TextMeshPro Text
    [SerializeField] [Expandable] private List<TextBoxSO> dialogueText; // List of dialogue text
    [SerializeField] private float timeBtwChars = 0.1f; // Tempo entre caracteres
    [SerializeField] private string leadingChar = ""; // Caractere que sera adicionado depois do texto
	[SerializeField] private bool leadingCharBeforeDelay;
	[SerializeField] public DialogueList dialogueList;// referencia a script
	[SerializeField] public AudioControl audioControl;// referencia a script
    public GameObject txtBoxImage, itemBoxImage;
	private const string introHasPlayedPrefKey = "introHasPlayed";//chave do playerPrefs para dialogo do tutorial
    private int _currentIndex, _counter; // Indice do texto atual
    public int currentDialogueIndex; // Indice do dialogo atual
    public Image portraitImage; // Sprite do personagem

    public static UnityAction<string> PlayAlways; // Evento de play do dialogo
    public static UnityAction<string> PlayOneShot; // Evento de play do dialogo
    public static UnityAction<string, int> PlayChance; // Evento de play do dialogo
	public static UnityAction<string> ResetOneShot; // Evento de play do dialogo
	public static UnityAction<string> PlayAlwaysRandom; // Evento de play do dialogo

    private void Awake()
    {
        PlayAlways = RunDialogue;
        PlayOneShot = RunDialogueOnce;
        PlayChance = RunDialogue;
	    ResetOneShot = ResetDialogue;
	    PlayAlwaysRandom = RunRandomDialogue;
	    portraitImage.enabled = false;
	    dialogueText[dialogueText.Count-1].hasPlayed = false;
    }

    private void Update()
    {
        if (itemTmp.text != "")
        {
            Time.timeScale = 1;
            if (_counter is 1) { StartCoroutine(ItemDisplayed()); }
        }

        if (dialogueTmp.text == "") return;
        if (dialogueTmp.text != "")
        {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && _currentIndex != dialogueText[currentDialogueIndex].texts.Length - 1)
        {
            StopAllCoroutines();
            audioControl.Stop("undertale"); // Parando o som de texto
            _currentIndex++; // Incrementando o indice do texto
            StartCoroutine(TypeWriterTMP(dialogueTmp, dialogueText)); // Iniciando a corotina de escrita
        }
        else if (Input.GetKeyDown(KeyCode.Space) &&
                 _currentIndex == dialogueText[currentDialogueIndex].texts.Length - 1)
        {
            StopAllCoroutines();
            //StopCoroutine(TypeWriterTMP(dialogueTmp, dialogueText)); // Parando a corotina de escrita
            audioControl.Stop("undertale"); // Parando o som de texto
            dialogueTmp.text = ""; // Limpando o texto
            Time.timeScale = 1;
            portraitImage.enabled = false;
            dialogueTmp.gameObject.SetActive(false);
            txtBoxImage.SetActive(false);
            _counter = 0;
        }
    }

    private IEnumerator TypeWriterTMP(TMP_Text box, IReadOnlyList<TextBoxSO> list)
    {
        portraitImage.enabled = true;
        portraitImage.sprite = list[currentDialogueIndex].portrait;
        box.gameObject.SetActive(true);
        Time.timeScale = 0; // Parando o tempo
        box.text = ""; // Limpando o texto
        box.text = leadingCharBeforeDelay ? leadingChar : ""; // Adicionando o caractere antes do texto

        yield return new WaitForSecondsRealtime(list[currentDialogueIndex].delay); // Esperando o tempo de delay

        foreach (var c in list[currentDialogueIndex].texts[_currentIndex])
        {
            audioControl.Play("undertale"); // Tocando o som de texto
            if (box.text.Length > 0)
            {
                box.text = box.text[..^leadingChar.Length]; // Removendo o caractere antes do texto
            }

            box.text += c; // Adicionando o caractere
            box.text += leadingChar; // Adicionando o caractere depois do texto
            yield return new WaitForSecondsRealtime(timeBtwChars); // Esperando o tempo entre caracteres
        }

        if (leadingChar != "")
        {
            box.text = box.text[..^leadingChar.Length]; // Removendo o caractere depois do texto
        }

        _counter++;
    }

    private void RunDialogue(string dialogueIndex)
    {
        SearchForDialogue(dialogueIndex);
        _currentIndex = 0; // Definindo o indice do texto atual
        txtBoxImage.SetActive(true);
        StartCoroutine(TypeWriterTMP(dialogueTmp, dialogueText)); // Iniciando a corotina de escrita
    }

    private void RunDialogueOnce(string dialogueIndex)
    {
        SearchForDialogue(dialogueIndex);
        if (dialogueText[currentDialogueIndex].hasPlayed) return;
        _currentIndex = 0; // Definindo o indice do texto atual
        txtBoxImage.SetActive(true);
        StartCoroutine(TypeWriterTMP(dialogueTmp, dialogueText)); // Iniciando a corotina de escrita
        dialogueText[currentDialogueIndex].hasPlayed = true; // Definindo que o dialogo ja aconteceu
    }

    private void RunDialogue(string dialogueIndex, int chance)
    {
        SearchForDialogue(dialogueIndex);
        var dialogueChance = Random.Range(0, 100); // Gera um numero aleatorio entre 0 e 100
        if (dialogueChance < 100 - chance) return;
        _currentIndex = 0; // Definindo o indice do texto atual
        txtBoxImage.SetActive(true);
        StartCoroutine(TypeWriterTMP(dialogueTmp, dialogueText)); // Iniciando a corotina de escrita
    }

	private void RunRandomDialogue(string category)
	{
		GetIntroHasPlayed();
		if(dialogueText[dialogueText.Count-1].hasPlayed == false && dialogueText[dialogueText.Count-1].id == "Intro")
		{
			RunDialogueOnce("Intro"); 
			PlayerPrefs.SetInt(introHasPlayedPrefKey, 1);
		}
		else if(category == "Entrance1")
		{
			int rand = Random.Range(0,dialogueList.dialoguesEntrance1.Count-1);
			RunDialogue(dialogueList.dialoguesEntrance1[rand].id);    
		}
		else if(category == "Entrance2")
		{
			int rand = Random.Range(0,dialogueList.dialoguesEntrance2.Count-1);
			RunDialogue(dialogueList.dialoguesEntrance2[rand].id);    
		}
		else if(category == "Entrance3")
		{
			int rand = Random.Range(0,dialogueList.dialoguesEntrance3.Count-1);
			RunDialogue(dialogueList.dialoguesEntrance3[rand].id);    
		}
		else if(category == "Death")
		{
			int rand = Random.Range(0,dialogueList.dialoguesDeath.Count-1);
			RunDialogue(dialogueList.dialoguesDeath[rand].id);    
		}
		else if(category == "Boss1")
		{
			int rand = Random.Range(0,dialogueList.dialoguesBoss1.Count-1);
			RunDialogue(dialogueList.dialoguesBoss1[rand].id);    
		}
		else if(category == "Boss2")
		{
			int rand = Random.Range(0,dialogueList.dialoguesBoss2.Count-1);
			RunDialogue(dialogueList.dialoguesBoss2[rand].id);    
		}
		else if(category == "Boss3")
		{
			int rand = Random.Range(0,dialogueList.dialoguesBoss3.Count-1);
			RunDialogue(dialogueList.dialoguesBoss3[rand].id);    
		}
		else
		{
			this.LogError("Dialogue list not found: " + category);
		}
        
	}
    

    private void ResetDialogue(string dialogueIndex)
    {
        SearchForDialogue(dialogueIndex);
        dialogueText[currentDialogueIndex].ResetOneShot(); // Definindo que o dialogo nao aconteceu
    }

    private void SearchForDialogue(string dialogueName)
    {
        var found = false;
        foreach (var dialogue in dialogueText.Where(dialogue => dialogue.id == dialogueName))
        {
            currentDialogueIndex = dialogueText.IndexOf(dialogue);
            found = true;
        }
        if (found is false) { this.LogError("Dialogue not found: " + dialogueName); }
    }
    
	private void SetIntroHasPlayed(bool hasPlayed)
	{
		if(hasPlayed == false)
		{
			PlayerPrefs.SetInt(introHasPlayedPrefKey, 1);
		}
		
	}
	
	private void GetIntroHasPlayed()
	{
		if(PlayerPrefs.GetInt(introHasPlayedPrefKey,0)==1)
		{
			dialogueText[dialogueText.Count-1].hasPlayed = true;
		}
		
		
		
	}

    public void DisplayItem(List<TextBoxSO> list)
    {
        currentDialogueIndex = 0;
        _currentIndex = 0;
        itemBoxImage.SetActive(true);
        StartCoroutine(TypeWriterTMP(itemTmp, list));
    }

    private IEnumerator ItemDisplayed()
    {
        yield return new WaitForSecondsRealtime(1f);
        audioControl.Stop("undertale"); // Parando o som de texto
        itemTmp.text = "";
        portraitImage.enabled = false;
        itemBoxImage.SetActive(false);
        _counter = 0;
    }
    
}