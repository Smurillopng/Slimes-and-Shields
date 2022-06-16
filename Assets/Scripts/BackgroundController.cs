using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundController : MonoBehaviour
{
    public Material backgroundMaterial;

    private void Start()
    {
        GetComponent<Renderer>().material = backgroundMaterial;
    }
}
