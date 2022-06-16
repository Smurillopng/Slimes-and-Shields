using System.Collections;
using Menu;
using NaughtyAttributes;
using Scriptable_Objects.SO_Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemController : MonoBehaviour
{
    //[Expandable] public DifficultyRarityManager difficultyRarityManager; // Referência para o script de gerenciamento de dificuldade e raridade
    [Expandable] public ItemSO itemEffect; // Referência para o scriptable object que contém os dados do item
    [Expandable] public ItemManager itemManager; // Referência para o script de gerenciamento de itens
    public int randomID; // pega um ID aleatório de item
    public SpriteRenderer spriteRenderer; // Referência para o sprite do item
    public int rarity; // Raridade do item
    public DadosRoda dadosRoda; // Referência para o script de gerenciamento de dados
    public DialogueManager dialogueManager; // Referência para o script de gerenciamento de diálogos
    public AudioControl audioControl; // Referência para o script de gerenciamento de áudio

    private void Awake()
    {
        audioControl = FindObjectOfType<AudioControl>();
        dialogueManager = FindObjectOfType<DialogueManager>();
	    dadosRoda = FindObjectOfType<DadosRoda>();
    }

    private void Start()
    {
	    StartCoroutine(Rarity()); // Chama o método que pega a raridade do item
	    spriteRenderer = GetComponent<SpriteRenderer>(); // Pega o componente SpriteRenderer do spawner
        StartCoroutine(LoadItem()); // Chama o método que spawna o item
        LookAtCamera(); // Chama o método que faz o item olhar para a câmera
    }

    private void Update()
    {
        var y = Mathf.PingPong(Time.time, 1) + 1;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        dialogueManager.DisplayItem(itemEffect.itemDescription);
        audioControl.Play("item_soundEffect");
        Destroy(gameObject); // Destrói o item
        itemEffect.Apply(other.gameObject); // Aplica o efeito do item no player
    }

    private void LookAtCamera()
    {
        if (Camera.main == null) return;
        var cameraPos = Camera.main.transform.position; // Pega a posição da câmera
        var itemPos = transform.position; // Pega a posição do item
        var direction = cameraPos - itemPos; // Calcula a posição do item para a câmera
        transform.rotation = Quaternion.LookRotation(direction); // Faz o item olhar para a câmera
    }

    private IEnumerator Rarity()
    {
        yield return new WaitForSeconds(0.0001f);
        rarity = dadosRoda.result switch
        {
            >= 1 and <= 10 => 1,
            >= 11 and <= 15 => 2,
            >= 16 and <= 20 => 3,
            _ => rarity
        };
    }

    private IEnumerator LoadItem()
    {
        yield return new WaitForSeconds(0.0002f);
        switch (rarity)
        {
            // Spawna um item comum
            case 1:
                randomID = Random.Range(1, itemManager.commonItems.Length + 1); // Pega um ID aleatório de item entre os itens comuns
                itemEffect = itemManager.commonItems[randomID - 1]; // Pega o scriptable object do item do id aleatório
                break;
            // Spawna um item raro
            case 2:
                randomID = Random.Range(1, itemManager.rareItems.Length + 1); // Pega um ID aleatório de item entre os itens raros
                itemEffect = itemManager.rareItems[randomID - 1]; // Pega o scriptable object do item do id aleatório
                break;
            // Spawna um item lendário
            case 3:
                randomID = Random.Range(1, itemManager.legendaryItems.Length + 1); // Pega um ID aleatório de item entre os itens lendários
                itemEffect = itemManager.legendaryItems[randomID - 1]; // Pega o scriptable object do item do id aleatório
                break;
        }

        spriteRenderer.sprite = itemEffect.itemSprite;
    }
}