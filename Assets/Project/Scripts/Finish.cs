using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if finish line trigger touched player car collider and if game actived, end level.
        if (other.CompareTag("Player") && LevelController.Instance.isGameActive)
        {
            LevelController.Instance.EndLevel();
        }   
    }
}
