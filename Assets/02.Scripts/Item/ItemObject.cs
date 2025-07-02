using UnityEngine;
using Photon.Pun;


public enum EItemType
{
    Score,
    Health,
    Stamina
}

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ItemObject : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.Score += 10;
            
            PhotonNetwork.Destroy(gameObject);
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
