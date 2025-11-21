using UnityEngine;
using TMPro;

public class DistanceCounter : MonoBehaviour
{
    public TMP_Text distanceText;

    private float _distance = 0f;

    private TileGenerator _tileGenerator;

    private bool _isPaused = false;

    public float Distance => _distance;
    void Start()
    {
        _tileGenerator = Object.FindFirstObjectByType<TileGenerator>();
        if (_tileGenerator == null)
        {
            Debug.LogError("TileGenerator не найден в сцене!");
        }
    }

    void Update()
    {
        if (_tileGenerator != null)
        {
            if (!_isPaused)
            {
                float speed = _tileGenerator.speed;
                _distance += speed * Time.deltaTime;
            }

            if (distanceText != null)
                distanceText.text = Mathf.FloorToInt(_distance).ToString();
        }
    }

    public void SetPaused(bool paused)
    {
        _isPaused = paused;
    }
}