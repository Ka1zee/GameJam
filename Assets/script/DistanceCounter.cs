using UnityEngine;
using TMPro;


public class DistanceCounter : MonoBehaviour
{
    public TMP_Text distanceText;

    public float distance;       // пройдена дистанція

    private TileGenerator _tileGenerator; // посилання на твій TileGenerator

    void Start()
    {
        // знаходимо TileGenerator на сцені
        _tileGenerator = Object.FindFirstObjectByType<TileGenerator>();
        if (_tileGenerator == null)
        {
            Debug.LogError("TileGenerator не знайдено на сцені!");
        }
    }

    void Update()
    {
        if (_tileGenerator != null)
        {
            // беремо швидкість з TileGenerator
            float speed = _tileGenerator.speed;

            // рахуємо дистанцію
            distance += speed * Time.deltaTime;

            // виводимо у текстовий рядок (округлюємо до цілих)
            if (distanceText != null)
                distanceText.text = Mathf.FloorToInt(distance).ToString();
        }
    }
}
