using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	    LookAtCameraInit();
    }

	private void LookAtCameraInit()
	{
		if (Camera.main == null) return;
		var cameraPos = Camera.main.transform.position; // Pega a posição da câmera
		var itemPos = transform.position; // Pega a posição do item
		var direction = cameraPos - itemPos; // Calcula a posição do item para a câmera
		transform.rotation = Quaternion.LookRotation(direction); // Faz o item olhar para a câmera
	}
}
