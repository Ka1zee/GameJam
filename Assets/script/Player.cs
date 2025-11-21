using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private CharacterController _characterController;

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
        print("Die");
    }
}
