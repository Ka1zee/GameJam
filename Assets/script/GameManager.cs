using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TileGenerator _tileGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player.DieEvent.AddListener(LoseHandler);
    }

   private void LoseHandler()
    {
       _tileGenerator.SetEnabling(false);
    }
    

}
