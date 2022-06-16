using System.Collections;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
}
