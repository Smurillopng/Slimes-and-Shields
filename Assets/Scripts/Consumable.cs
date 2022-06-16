using System.Collections;
using Menu;
using NaughtyAttributes;
using Scriptable_Objects.SO_Scripts;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    //[Expandable] public DifficultyRarityManager difficultyRarityManager; // Referência para o script de gerenciamento de dificuldade e raridade
    [Expandable] public ItemSO itemEffect; // Referência para o scriptable object que contém os dados do item
    private SpriteRenderer spriteRenderer; // Referência para o sprite do item
    private AudioControl audioControl; // Referência para o script de gerenciamento de áudio

    private void Awake()
    {
        audioControl = FindObjectOfType<AudioControl>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    private void Start()
    {
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

    private IEnumerator LoadItem()
    {
        yield return new WaitForSeconds(0.0001f);
        spriteRenderer.sprite = itemEffect.itemSprite;
    }
}