using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    private Slider myself;
    private MusicPlayer myplayer;
    private void Awake()
    {
        myself = GetComponent<Slider>();
        if (GameObject.FindGameObjectsWithTag("MusicPlayer").Length != 0)
        {
            myplayer = GameObject.FindWithTag("MusicPlayer").GetComponent<MusicPlayer>();
            myself.onValueChanged.AddListener(delegate { myplayer.SetMusicVol(myself.value); });
        }
    }
}
