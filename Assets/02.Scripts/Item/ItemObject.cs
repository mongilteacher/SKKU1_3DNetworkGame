using System;
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
    [SerializeField]
    private EItemType _itemType;
    public EItemType ItemType => _itemType;
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 내 플레이어가 아니면 리턴한다.
            Player player = other.GetComponent<Player>();
            if (!player.GetComponent<PhotonView>().IsMine)
            {
                return;
            }
            
            switch (_itemType)
            {
                case EItemType.Score:
                {
                   ScoreManager.Instance.AddScore(100);
                    break;
                }

                case EItemType.Health:
                {
                    // 데이터와 데이터를 다루는 로직이 떨어져있죠.
                    // -> 응집도가 떨어집니다.
                    player.Stat.Health = Mathf.Max(player.Stat.MaxHealth, player.Stat.Health + 50);
                    break;
                }

                case EItemType.Stamina:
                {
                    player.Stat.Stamina += 50;
                    break;
                }
            }
            
            //PhotonNetwork.Destroy(gameObject);
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
