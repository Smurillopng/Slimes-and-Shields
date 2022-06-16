using System.Collections;
using Menu;
using UnityEngine;
using UnityEngine.Video;

public class DeathScene : MonoBehaviour
{
    public VideoPlayer enterDeathVideo, exitDeathVideo, loopVideo;
    public MenuManager menuManager;
    private bool _isLooping;
    public float delay;
    
    private void Update()
    {
        if (_isLooping) return;
        if (!enterDeathVideo.isPlaying && !_isLooping) { StartCoroutine(Looping()); }
    }

    public void PlayVideo() { StartCoroutine(VideoLength()); }
    
    private IEnumerator VideoLength()
    {
        loopVideo.isLooping = false;
        exitDeathVideo.Play();
        yield return new WaitForSeconds((float)exitDeathVideo.clip.length);
        menuManager.LoadGame();
    }

    private IEnumerator Looping()
    {
        _isLooping = true;
        yield return new WaitForSeconds((float)enterDeathVideo.clip.length - delay);
        loopVideo.Play();
    }
}