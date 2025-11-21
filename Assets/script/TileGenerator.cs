using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public float speed => _speed; // лише для читання
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedIncreaseRate = 0.5f; // скорость прироста (ед./сек)
    [SerializeField] private float _maxSpeed = 10f; // максимальная скорость
    [SerializeField] private int _maxCount;
    [SerializeField] private List<Tile> _tiles = new List<Tile>();
    [SerializeField] private Transform _tileHolder;
    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private float _startSpawnBomb = 3;

    [Header("Theme groups (assign prefabs)")]
    [SerializeField] private List<GameObject> _winterTilePrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> _summerTilePrefabs = new List<GameObject>();

    [Header("Timer group switching")]
    [SerializeField] private float _initialSwitchInterval = 60f; // первая смена (сек)
    [SerializeField] private float _switchIntervalIncrement = 30f; // +30 сек каждый раз
    [SerializeField] private bool _alternateGroups = true; // true — чередовать, false — выбирать случайную следующую

    private float _timer; // общий таймер (используется для логики появления бомб)
    private bool _isEnabling = true;

    private float _elapsedSinceLastSwitch;
    private float _currentSwitchInterval;
    private int _currentGroupIndex = 0; // 0 = winter, 1 = summer

    void Start()
    {
        if (_tiles != null && _tiles.Count > 0)
        {
            foreach (var t in _tiles)
                if (t != null) t.speed = _speed;
        }

        _currentSwitchInterval = Mathf.Max(0.01f, _initialSwitchInterval);
        _elapsedSinceLastSwitch = 0f;

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

        // Постоянное увеличение скорости со временем, ограниченное _maxSpeed
        if (_speedIncreaseRate != 0f)
        {
            _speed = Mathf.Min(_maxSpeed, _speed + _speedIncreaseRate * Time.deltaTime);

            for (int i = 0; i < _tiles.Count; i++)
            {
                if (_tiles[i] != null)
                    _tiles[i].speed = _speed;
            }
        }

        // Таймерное переключение групп
        _elapsedSinceLastSwitch += Time.deltaTime;
        if (_elapsedSinceLastSwitch >= _currentSwitchInterval)
        {
            SwitchGroup();
            _elapsedSinceLastSwitch -= _currentSwitchInterval; // учитываем возможный overflow
            _currentSwitchInterval += _switchIntervalIncrement;
        }

        if (_tiles.Count < _maxCount)
        {
            GenerateTile();
        }
    }

    public void SetEnabling(bool state)
    {
        _isEnabling = state;
        foreach (Tile tile in _tiles)
        {
            if (tile != null) tile.SetMoving(state);
        }
    }

    private void SwitchGroup()
    {
        int previous = _currentGroupIndex;
        if (_alternateGroups)
        {
            _currentGroupIndex = (_currentGroupIndex + 1) % 2;
        }
        else
        {
            int next = Random.Range(0, 2);
            int tries = 0;
            while (next == _currentGroupIndex && tries < 5)
            {
                next = Random.Range(0, 2);
                tries++;
            }
            _currentGroupIndex = next;
        }
        Debug.Log($"Tile group switched: {previous} -> {_currentGroupIndex}. Next interval: {_currentSwitchInterval}s");
    }

    private List<GameObject> CurrentGroupPrefabs()
    {
        return _currentGroupIndex == 0 ? _winterTilePrefabs : _summerTilePrefabs;
    }

    private GameObject PickPrefabForSpawn()
    {
        var group = CurrentGroupPrefabs();
        if (group != null && group.Count > 0)
        {
            int idx = Random.Range(0, group.Count);
            return group[idx];
        }
        // fallback на единый префаб, если группы не заданы
        return _tilePrefab;
    }

    private void GenerateTile()
    {
        GameObject prefabToSpawn = PickPrefabForSpawn();

        Vector3 spawnPos;
        if (_tiles != null && _tiles.Count > 0 && _tiles.Last() != null)
        {
            spawnPos = _tiles.Last().transform.position + Vector3.forward * prefabToSpawn.transform.localScale.z;
        }
        else
        {
            Vector3 basePos = _tileHolder != null ? _tileHolder.position : transform.position;
            spawnPos = basePos + Vector3.forward * prefabToSpawn.transform.localScale.z;
        }

        GameObject newTileObject = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        Tile newTile = newTileObject.GetComponent<Tile>();
        newTile.initialize(_coin, _bomb, _startSpawnBomb, _timer);
        newTile.speed = _speed;
        _tiles.Add(newTile);
        newTileObject.transform.SetParent(_tileHolder);
    }

    private void OnTriggerEnter(Collider other)
    {
        var tile = other.GetComponent<Tile>();
        if (tile != null) _tiles.Remove(tile);
        Destroy(other.gameObject);
    }
}