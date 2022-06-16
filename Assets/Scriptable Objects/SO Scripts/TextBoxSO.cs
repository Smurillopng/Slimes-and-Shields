using NaughtyAttributes;
using UnityEngine;

namespace Scriptable_Objects.SO_Scripts
{
    [CreateAssetMenu(fileName = "New TextBox Group", menuName = "SO/New TextBox Group", order = 4)]
    public class TextBoxSO : ScriptableObject
    {
        [TextArea(3, 10)]public string[] texts; // Textos que serão exibidos na tela
        [ReadOnly] public float delay = 0.1f; // Tempo de delay entre cada caractere
        [ReadOnly] public bool hasPlayed = false; // Se está escrevendo ou não
        public string id; // Nome do grupo
        [Required()] public Sprite portrait; // Foto do personagem
        
        public void ResetOneShot()
        {
            hasPlayed = false;
        }
    }
}