using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Globals;

public class ChangementScene3 : MonoBehaviour
{
    public VideoPlayer VideoPlayer;

    void Start()
    {
        // Quand la vidéo se termine,
        VideoPlayer.loopPointReached += LoadScene;
    }
    
    void LoadScene(VideoPlayer vp)
    {
        // On passe à la map de prélude suite
        SceneManager.LoadScene("PreludeSuite");
    }
}