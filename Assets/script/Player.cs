using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public UnityEvent DieEvent = new();
    [SerializeField] private float _speed = 5f;
    [SerializeField] private CharacterController _characterController;

    private Animator _animator;
    private bool _isAlive = true;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_characterController != null && _characterController.enabled && _characterController.gameObject.activeInHierarchy)
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector3 move = Vector3.right * _speed * horizontal * Time.deltaTime;
            _characterController.Move(move);
        }
    }

    public void Die()
    {
        if (_isAlive == false)
            return;

        print("Die");
        _animator.SetTrigger("Die");
        _isAlive = false;
        DieEvent?.Invoke();

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Остановить счётчик дистанции
        var distanceCounter = Object.FindFirstObjectByType<DistanceCounter>();
        if (distanceCounter != null) distanceCounter.SetPaused(true);

        // Остановить генератор плит (движение / спавн)
        var tileGenerator = Object.FindFirstObjectByType<TileGenerator>();
        if (tileGenerator != null) tileGenerator.SetEnabling(false);

        // Отключить управление игроком
        if (_characterController != null) _characterController.enabled = false;

        // Ждём случайно от 5 до 10 секунд (реальное время, не затрагивает Time.timeScale)
        float waitTime = Random.Range(5f, 10f);
        yield return new WaitForSecondsRealtime(waitTime);

        SceneManager.LoadScene(0);
    }
}