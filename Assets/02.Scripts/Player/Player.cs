using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine.UI;

public enum EPlayerState
{
    Live,
    Death
}


public class Player : MonoBehaviour, IDamaged
{
    public PlayerStat Stat;

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
    public void Damaged(float damage)
    {
        if (_state == EPlayerState.Death) return;
        
        Stat.Health = Mathf.Max(0, Stat.Health - damage);

        if (Stat.Health <= 0)
        {
            _state = EPlayerState.Death;
            
            // 사망 애니메이션 실행
            _animator.SetTrigger("Death");
            
            // 5초 동안(못 움직이고 , 못 맞고, 못 때린다.)
            // 5초 후에 체력과 스태미너 회복된 상태에서 랜덤한 위치에 리스폰
            StartCoroutine(Death_Coroutine());
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



