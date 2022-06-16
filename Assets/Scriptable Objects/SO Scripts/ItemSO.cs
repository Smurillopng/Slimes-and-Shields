using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Scriptable_Objects.SO_Scripts
{
    public abstract class ItemSO : ScriptableObject
    {
        [Header("Basic Settings")]
        public List<TextBoxSO> itemDescription; // Nome do item
        [ShowAssetPreview] public Sprite itemSprite; // Sprite do item

        [Space(8)][Header("Customization")]
        public bool enableCustomMaterial; // Ativa a customização do material do player
        [ShowIf("enableCustomMaterial")] public Material customMaterial; // Material a ser atribuído ao player
        [ShowIf("enableCustomMaterial")] public BodyParts[] materialParts; // Parte do player que o item aplicará o material
        [Space(5)]
        public bool enableCustomColor; // Ativa a customização da cor do player
        [ShowIf("enableCustomColor")] public Color itemColor; // Cor a ser atribuída ao player
        [ShowIf("enableCustomColor")] public BodyParts[] colorParts; // Parte do player que o item aplicará a cor
        private static readonly int ShaderColor = Shader.PropertyToID("_Color");

        public enum BodyParts { Face, Body, Shine, Outline, Shield}; // Cada parte do player

        public abstract void Apply(GameObject target); // Aplica o efeito do item no player

        protected void CustomMaterial(GameObject target) // Aplica materiais customizados no player
        {
            foreach (var part in materialParts)
            {
                switch(part)
                {
                    case BodyParts.Face: // Material da boca
                        target.GetComponent<PlayerController>().playerFace.GetComponent<SkinnedMeshRenderer>().material = customMaterial;
                        break;
                    case BodyParts.Body: // Material do corpo
                        target.GetComponent<PlayerController>().playerBody.GetComponent<SkinnedMeshRenderer>().material = customMaterial;
                        break;
                    case BodyParts.Shine: // Material do brilho
                        target.GetComponent<PlayerController>().playerShine.GetComponent<SkinnedMeshRenderer>().material = customMaterial;
                        break;
                    case BodyParts.Outline: // Material da borda
                        target.GetComponent<PlayerController>().playerOutline.GetComponent<SkinnedMeshRenderer>().material = customMaterial;
                        break;
                    case BodyParts.Shield: // Material do escudo
                        target.GetComponent<PlayerController>().playerShield.GetComponent<MeshRenderer>().materials[1] = customMaterial;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        protected void CustomColor(GameObject target) // Aplica cores customizadas no player
        {
            foreach (var part in colorParts)
            {
                switch (part)
                {
                    case BodyParts.Face: // Cor da boca
                        target.GetComponent<PlayerController>().playerFace.GetComponent<SkinnedMeshRenderer>().material.SetColor(ShaderColor, itemColor);
                        target.GetComponent<PlayerController>().playerFace.GetComponent<SkinnedMeshRenderer>().material.color = itemColor;
                        break;
                    case BodyParts.Body: // Cor do corpo
                        target.GetComponent<PlayerController>().playerBody.GetComponent<SkinnedMeshRenderer>().material.SetColor(ShaderColor, itemColor);
                        target.GetComponent<PlayerController>().playerBody.GetComponent<SkinnedMeshRenderer>().material.color = itemColor;
                        break;
                    case BodyParts.Shine: // Cor do brilho
                        target.GetComponent<PlayerController>().playerShine.GetComponent<SkinnedMeshRenderer>().material.SetColor(ShaderColor, itemColor);
                        target.GetComponent<PlayerController>().playerShine.GetComponent<SkinnedMeshRenderer>().material.color = itemColor;
                        break;
                    case BodyParts.Outline: // Cor da borda
                        target.GetComponent<PlayerController>().playerOutline.GetComponent<SkinnedMeshRenderer>().material.SetColor(ShaderColor, itemColor);
                        target.GetComponent<PlayerController>().playerOutline.GetComponent<SkinnedMeshRenderer>().material.color = itemColor;
                        break;
                    case BodyParts.Shield: // Cor do escudo
                        target.GetComponent<PlayerController>().playerShield.GetComponent<MeshRenderer>().materials[1].SetColor(ShaderColor, itemColor);
                        target.GetComponent<PlayerController>().playerShield.GetComponent<MeshRenderer>().materials[1].color = itemColor;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
