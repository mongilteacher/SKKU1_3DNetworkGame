using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.Score += 10;
            
            Destroy(gameObject);
        }
    }
}
