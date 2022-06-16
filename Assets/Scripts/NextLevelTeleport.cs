using Menu;
using UnityEngine;

public class NextLevelTeleport : MonoBehaviour
{
    private MenuManager _menuManager;

    private void Awake() { _menuManager = FindObjectOfType<MenuManager>(); }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _menuManager.LoadGame();
    }   
}