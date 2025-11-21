using UnityEngine;
using TMPro;

public class DistanceCounter : MonoBehaviour
{
    public TMP_Text distanceText;

    public float distance;

    private TileGenerator _tileGenerator;

    private bool _isPaused = false;

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
                distance += speed * Time.deltaTime;
            }

            if (distanceText != null)
                distanceText.text = Mathf.FloorToInt(distance).ToString();
        }
    }

    public void SetPaused(bool paused)
    {
        _isPaused = paused;
    }
}