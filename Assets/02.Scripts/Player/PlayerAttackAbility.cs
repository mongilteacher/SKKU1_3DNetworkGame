using Photon.Pun;
using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{    
    private Animator _animator;

    private float _attackTimer = 0f;

    private bool _isAttacking = false;
    public bool IsAttacking => _isAttacking;

    public Collider WeaponCollider;
    

    private void Start()
    {
        _animator = GetComponent<Animator>();

        DeActiveCollider();
    }
    
    // - '위치/회전' 처럼 상시로 확인이 필요한 데이터 동기화: IPunObservable(OnPhotonSerializeView)
    // - '트리거/공격/피격' 처럼 간헐적으로 특정한 이벤트가 발생했을때의 변화된 데이터 동기화: RPC
    //    RPC: Remote Procedure Call
    //         ㄴ 물리적으로 떨어져 있는 다른 디바이스의 함수를 호출하는 기능
    //         ㄴ RPC 함수를 호출하면 네트워크를 통해 다른 사용자의 스크립트에서 해당 함수가 호출된다.
    
    
    
    private void Update()
    {
        if (_photonView.IsMine == false || _owner.State == EPlayerState.Death)
        {
            return;
        }

        _attackTimer += Time.deltaTime;

        
        if (Input.GetMouseButton(0) && _owner.Stat.Stamina >= _owner.Stat.StaminaAttackCost && _attackTimer >= (1f / _owner.Stat.AttackSpeed))
        {
            _owner.Stat.Stamina -= _owner.Stat.StaminaAttackCost;
            
            _attackTimer = 0f;
            
            // 1. 일반 메서드 호출 방식
            // PlayAttackAnimation( Random.Range(1, 4));
            
            // 2. RPC 메서드 호출 방식
            // 네임오브를 쓰자! 오타주의!
            _photonView.RPC(nameof(PlayAttackAnimation), RpcTarget.All, Random.Range(1, 4));
        }
    }

    
    public void ActiveCollider()
    {
        WeaponCollider.enabled = true;
    }

    public void DeActiveCollider()
    {
        WeaponCollider.enabled = false;
    }
    
    
    
    // RPC로 호출할 함수는 반드시 [PunRPC] 어트리뷰트를 함수 앞에 명시해줘야 한다.
    [PunRPC]
    private void PlayAttackAnimation(int randomNumber)
    {
        _animator.SetTrigger($"Attack{randomNumber}");
    }


    public void Hit(Collider other)
    {
        // 내 캐릭터가 아니면...
        if (_photonView.IsMine == false || _owner.State == EPlayerState.Death)
        {
            return;
        }
        
        if (other.GetComponent<IDamaged>() == null) return;

        DeActiveCollider();
        
        // RPC로 호출해야지 다른 사람의 게임오브젝트들도 이 함수가 실행된다.
        // damagedObject.Damaged(_owner.Stat.Damage);
        
        PhotonView otherPhotonView = other.GetComponent<PhotonView>();
        otherPhotonView.RPC(nameof(Player.Damaged), RpcTarget.All, _owner.Stat.Damage, _photonView.Owner.ActorNumber);
    }
}

