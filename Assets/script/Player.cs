using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent DieEvent = new ();
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
    }
}
