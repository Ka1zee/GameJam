using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    private int _coinsCount;
    [SerializeField] private Player _player;
    [SerializeField] private TileGenerator _tileGenerator;
    [SerializeField] private DistanceCounter _distanceCounter;
    [SerializeField] private AudioClip _audioCclip;
    [SerializeField] private AudioClip _audioBombClip;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _bgMusicSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player.DieEvent.AddListener(LoseHandler);
        
        if (_bgMusicSource != null)
        {
            _bgMusicSource.loop = true;
            _bgMusicSource.Play();
        }
    }

    private void LoseHandler()
    {
        if (_bgMusicSource != null)
            _bgMusicSource.Stop();
        _audioSource.PlayOneShot(_audioBombClip);
        _tileGenerator.SetEnabling(false);
    }
    private void SaveScore()
    {
        PlayerPrefs.SetInt("score", Mathf.FloorToInt(_distanceCounter.Distance));
    }
    public void AddCoin()
    {
        _audioSource.PlayOneShot(_audioCclip);
        _coinsCount++;
        _coinsText.text = _coinsCount.ToString();
    }
}
