using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ChangementScene3 : MonoBehaviour
{
    VideoPlayer player;

    void Awake()
    {
        player = GetComponent<VideoPlayer>();
    }
    private void OnEnable()
    {
        player.loopPointReached += LoadScene;
    }
    private void OnDisable()
    {
        player.loopPointReached -= LoadScene;
    }

    void LoadScene(VideoPlayer _)
    {
        // On passe à la map de prélude suite
        SceneManager.LoadScene("PreludeSuite");
    }
}