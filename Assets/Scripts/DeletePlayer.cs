using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePlayer : MonoBehaviour
{
    private GameObject player;
    
	private void Update()
	{
		if (GameObject.FindGameObjectWithTag("Player")) 
		{
			player = GameObject.FindGameObjectWithTag("Player");
			StartCoroutine(KillPlayer());
		}
    }
    //delete player after starting scene
    private IEnumerator KillPlayer()
    {
	    yield return new WaitForSecondsRealtime(0.1f);
        Destroy(player);
    }
}
