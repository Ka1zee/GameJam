using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public float speed;
    [SerializeField] private List<Transform> _points = new List<Transform>();


    private GameObject _coin;
    private GameObject _bomb;
    private float _startSpawnBomb;
    private float _timer;
    void Start()
    {
        if(_coin == null || _bomb == null)
        {
            return;
        }
        int randomPointIndex = Random.Range(0, _points.Count);
        if (_timer < _startSpawnBomb)
        {
            CreateObject(randomPointIndex, _coin);
        }
        else
        {
            float chanceSpawnBomb = 20 + (_timer / 2);
            chanceSpawnBomb = Mathf.Clamp(chanceSpawnBomb, 0, 50);
            if (Random.Range(0, 100) < chanceSpawnBomb)
            {
                CreateObject(randomPointIndex, _bomb);
            }
            else
            {
                CreateObject(randomPointIndex, _coin);
            }
        }
    }
    private void CreateObject(int randomPointIndex, GameObject createdObject)
    {
        GameObject newCoin = Instantiate(createdObject, _points[randomPointIndex].position, Quaternion.identity);
        newCoin.transform.SetParent(transform);
    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.back * speed * Time.fixedDeltaTime);
    }
    public void initialize(GameObject coin, GameObject bomb, float startSpawnBomb, float timer)
    {
        _coin = coin;
        _bomb = bomb;
        _timer = timer;
        _startSpawnBomb = startSpawnBomb;
    }
}
