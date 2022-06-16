using UnityEngine;

public class HealthBar : MonoBehaviour
{
     private MaterialPropertyBlock _matBlock;
     private MeshRenderer _meshRenderer;
     private Camera _mainCamera;
     private EnemyController _enemyController;
     private EnemyMagic _enemyMagic;
     private static readonly int Fill = Shader.PropertyToID("_Fill");

     private void Awake()
     {
          _meshRenderer = GetComponent<MeshRenderer>();
          _matBlock = new MaterialPropertyBlock();
          
          if (GetComponentInParent<EnemyController>())
          {
               _enemyController = GetComponentInParent<EnemyController>();
          }

          if (GetComponentInParent<EnemyMagic>())
          {
               _enemyMagic = GetComponentInParent<EnemyMagic>();
          }
     }

     private void Start()
     {
          _mainCamera = Camera.main;
     }
     
     private void Update()
     {
          if (_enemyController)
          {
               if (_enemyController.enemyStats.health < _enemyController.enemyStats.maxHealth)
               {
                    _meshRenderer.enabled = true;
                    AlignCamera();
                    UpdateParams();
               }
               else
               {
                    _meshRenderer.enabled = false;
               }
          }
          if (_enemyMagic)
          {
               if (_enemyMagic.enemyStats.health < _enemyMagic.enemyStats.maxHealth)
               {
                    _meshRenderer.enabled = true;
                    AlignCamera();
                    UpdateParams();
               }
               else
               {
                    _meshRenderer.enabled = false;
               }
          }
     }
     
     private void UpdateParams()
     {
          _meshRenderer.GetPropertyBlock(_matBlock);
          if (_enemyController)
          {
               _matBlock.SetFloat(Fill, _enemyController.enemyStats.health / (float)_enemyController.enemyStats.maxHealth);
          }
          if (_enemyMagic)
          {
               _matBlock.SetFloat(Fill, _enemyMagic.enemyStats.health / (float)_enemyMagic.enemyStats.maxHealth);
          }
          _meshRenderer.SetPropertyBlock(_matBlock);
     }
     
     private void AlignCamera()
     {
          if (_mainCamera != null)
          {
               var camXform = _mainCamera.transform;
               var forward = transform.position - camXform.position;
               forward.Normalize();
               var up = Vector3.Cross(forward, camXform.right);
               transform.rotation = Quaternion.LookRotation(forward, up);
          }
     }
}