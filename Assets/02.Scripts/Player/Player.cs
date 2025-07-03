using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EPlayerState
{
    Live,
    Death
}

[RequireComponent(typeof(PlayerMoveAbility))]
public class Player : MonoBehaviour, IDamaged
{
    public PlayerStat Stat;

    public int Score = 0;
    

    private EPlayerState _state = EPlayerState.Live;
    public EPlayerState State => _state;
    
    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();

    private PhotonView _photonView;
    private Animator _animator;
    private CharacterController _characterController;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }

    [PunRPC]
    public void Damaged(float damage, int actorNumber)
    {
        if (_state == EPlayerState.Death) return;
        
        Stat.Health = Mathf.Max(0, Stat.Health - damage);

        // 타격 이펙트 생성
        
        
        if (Stat.Health <= 0)
        {
            _state = EPlayerState.Death;
            
            // 사망 애니메이션 실행
            _animator.SetTrigger("Death");
            
            // 5초 동안(못 움직이고 , 못 맞고, 못 때린다.)
            // 5초 후에 체력과 스태미너 회복된 상태에서 랜덤한 위치에 리스폰
            StartCoroutine(Death_Coroutine());

            RoomManager.Instance.OnPlayerDeath(_photonView.Owner.ActorNumber, actorNumber);

            if (_photonView.IsMine)
            {
                MakeItems(Random.Range(1, 4));
            }
        }
        else
        {
            // RPC로 호출 X
            GetAbility<PlayerShakingAbility>().Shake();
        }
    }

    private void MakeItems(int count)
    {
        // 30% 확률로 포션 드랍
        if (Random.Range(0f, 1f) < 0.3f)
        {
            if (Random.Range(0, 2) == 0)
            {
                ItemObjectFactory.Instance.RequestCreate(EItemType.Health, transform.position + new Vector3(0, 2, 0));
            }
            else
            {
                ItemObjectFactory.Instance.RequestCreate(EItemType.Stamina, transform.position + new Vector3(0, 2, 0));
            }
        }
        
        for (int i = 0; i < count; ++i)
        {
            // 포톤의 네트워크 객체의 생명 주기
            // Player : 플레이어가 생성하고, 플레이어가 나가면 자동삭제 (PhotonNetwork.Instantiate/Destroy)
            // Room   : 방장이 생성하고, 룸이 없어지면 삭제.. (PhotonNetwork.InstantiatateRoomObject/Destory)
            // PhotonNetwork.InstantiateRoomObject("ScoreItem", transform.position + new Vector3(0, 2, 0), Quaternion.identity, 0);
            
            ItemObjectFactory.Instance.RequestCreate(EItemType.Score, transform.position + new Vector3(0, 2, 0));
        }
    }
    

    private IEnumerator Death_Coroutine()
    {
        var wait = new WaitForSeconds(5f);
        
        _characterController.enabled = false;

        yield return wait;
        
        Stat.Health = Stat.MaxHealth;
        Stat.Stamina = Stat.MaxStamina;

        _state = EPlayerState.Live;
        _animator.SetTrigger("Live");        
        
        // 리스폰 코드 : 주인만 움직여야 한다.
        if (_photonView.IsMine)
        {
            var randomSpawnPoint = SpawnPoints.Instance.GetRandomSpawnPoint();
            transform.position = randomSpawnPoint;
        }
        
        _characterController.enabled = true;
    }
    

    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
        {
            return ability as T;
        }

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라
        //                    필요할때만 하는.. 뒤로 미루는 기법
        ability = GetComponentInChildren<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability;

            return ability as T;
        }
        
        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }
    

    private void Update()
    {
        if (!GetAbility<PlayerMoveAbility>().IsJumping && !GetAbility<PlayerMoveAbility>().IsRunning)
        {
            Stat.Stamina = Mathf.Min(Stat.MaxStamina, Stat.Stamina + Stat.StaminaRecovery * Time.deltaTime);
        }
    }
}



