using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public float speed => _speed; // лише для читання
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private float _speed;
    [SerializeField] private int _maxCount;
    [SerializeField] private List<Tile> _tiles = new List<Tile>(); 
    [SerializeField] private Transform _tileHolder;
    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private float _startSpawnBomb = 3;

    private float _timer;
    private bool _isEnabling = true;
    void Start()
    {
        _tiles.First().speed = _speed;
        for (int i = 0; i < _maxCount; i++)
        {
            GenerateTile();
        }
    }

    void Update()
    {
        if (_isEnabling == false)
            return;

        _timer += Time.deltaTime;

        if (_tiles.Count < _maxCount)
        {
            GenerateTile();
        }
    }

    public void SetEnabling(bool state)
    {
        _isEnabling = state;
        foreach(Tile tile in _tiles)
        {
            tile.SetMoving(state);
        }
    }
    private void GenerateTile()
    {
        GameObject newTileObject = Instantiate(_tilePrefab, _tiles.Last().transform.position + Vector3.forward * _tilePrefab.transform.localScale.z, Quaternion.identity);
        Tile newTile = newTileObject.GetComponent<Tile>();
        newTile.initialize(_coin, _bomb, _startSpawnBomb, _timer);
        newTile.speed = _speed;
        _tiles.Add(newTile);
        newTileObject.transform.SetParent(_tileHolder);
    }

    private void OnTriggerEnter(Collider other)
    {
        _tiles.Remove(other.GetComponent<Tile>());  
        Destroy(other.gameObject);
    }
}

