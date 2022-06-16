using NaughtyAttributes;
using Scriptable_Objects.SO_Scripts;
using TMPro;
using UnityEngine;

public class Sliders : MonoBehaviour
{
        [SerializeField][ReadOnly] private PlayerSO playerStats; // Referência ao scriptable object do player
        [SerializeField][ReadOnly] private ShieldBounce shieldBounce; // Referência ao script ShieldBounce
        
        public UnityEngine.UI.Slider healthSlider;  // Referência ao slider de vida
        public TextMeshProUGUI maxHeathText; // Referência ao texto de vida máxima
        public TextMeshProUGUI currentHealthText; // Referência ao texto de vida atual
        
        public UnityEngine.UI.Slider manaSlider;  // Referência ao slider de vida
        public TextMeshProUGUI maxManaText; // Referência ao texto de mana máxima
        public TextMeshProUGUI currentManaText; // Referência ao texto de mana atual
        
        public UnityEngine.UI.Slider shieldCooldownSlider;  // Referência ao slider de escudo
        
        private void Awake()
        {
                playerStats = FindObjectOfType<PlayerController>().playerStats; // Busca o scriptable object do player
                shieldBounce = FindObjectOfType<ShieldBounce>(); // Busca o script ShieldBounce
        }
        
        public void Update()
        {
                healthSlider.maxValue = playerStats.maxHealth; // Atualiza o valor máximo do slider de vida
                healthSlider.value = playerStats.currentHealth; // Atualiza o valor atual da vida
                maxHeathText.text = playerStats.maxHealth.ToString();
                currentHealthText.text = playerStats.currentHealth.ToString();
                
                manaSlider.maxValue = playerStats.maxMana; // Atualiza o valor máximo do slider de vida
                manaSlider.value = playerStats.currentMana; // Atualiza o valor atual da vida
                maxManaText.text = playerStats.maxMana.ToString(); // Atualiza o valor máximo do slider de mana
                currentManaText.text = playerStats.currentMana.ToString(); // Atualiza o valor atual do slider de mana

                shieldCooldownSlider.maxValue = shieldBounce.cooldownTime; // Atualiza o valor máximo do slider de cooldown
                shieldCooldownSlider.value = shieldBounce.cooldown; // Atualiza o valor atual do slider de cooldown
        }
}