using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{    
    private Animator _animator;

    private float _attackTimer = 0f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        // 문제
        // Ability에서 다른 Ability에 접근하는 효율적(편하고 좋은거)인 방법
        // ex) PlayerMoveAbility의 IsMove 속성에 따라 공격 여부를 정하고 싶다....

        _attackTimer += Time.deltaTime;

        if (Input.GetMouseButton(0) && _attackTimer >= (1f / _owner.Stat.AttackSpeed))
        {
            _attackTimer = 0f;
            
            _animator.SetTrigger($"Attack{Random.Range(1, 4)}");
        }
    }
}

