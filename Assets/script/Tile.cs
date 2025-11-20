using UnityEngine;

public class Tile : MonoBehaviour
{

    public float speed;

    void Start()
    {

    }
    void FixedUpdate()
    {
        transform.Translate(Vector3.back * speed * Time.fixedDeltaTime);
    }
}
