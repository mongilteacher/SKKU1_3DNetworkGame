using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;


public enum EMonsterState
{
    Idle,
    Patrol,
    Trace,
    Attack,
    Damaged,
    Death
}

public class Bear : MonoBehaviour
{
    private EMonsterState _state = EMonsterState.Idle;
    public EMonsterState State => _state;
    
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private Player _targetPlayer = null;

    private float _idleTimer = 0f;
    private float _traceTimer = 0f;
    private float _attackTimer;

    private const float IDLE_TIME = 7f;
    private const float SEARCH_RANGE = 9f;
    private const float TRACE_TIME = 5f;
    private const float ATTACK_RANGE = 5f;
    private const float ATTACK_COOLTIME = 1.5f;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (_state)
        {
            // 현재 상태를 체크해서 해당 상태별로 정해진 기능을 수행한다.
            case EMonsterState.Idle:
                Idle();
                break;
            case EMonsterState.Patrol:
                Patrol();
                break;
            case EMonsterState.Trace:
                Trace();
                break;
            case EMonsterState.Attack:
                Attack();
                break;
            case EMonsterState.Damaged:
                Damaged();
                break;
            case EMonsterState.Death:
                Death();
                break;
        }
    }

    private void Idle()
    {
        // 대기하는 상태
        
        // Idle 애니메이션 재생
        _animator.SetTrigger("Idle");
        
        // Patrol: - Idle 상태가 N초 이상되면 순찰 돌기
        _idleTimer += Time.deltaTime;
        if (_idleTimer >= IDLE_TIME)
        {
            _idleTimer = 0f;
            UnityEngine.Debug.Log("Idle -> Patrol");
            _state = EMonsterState.Patrol;
            return;
        }
        
        // Trace: 특정 범위 안에 있는 랜덤 플레이어를 찾아서 쫓아가기
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return;
        
        var searched = players.Where(p => Vector3.Distance(p.transform.position, transform.position) <= SEARCH_RANGE).ToList();
        if (searched.Count == 0) return;
        
        _targetPlayer = searched[Random.Range(0, searched.Count())].GetComponent<Player>();

        if (_targetPlayer != null)
        {
            UnityEngine.Debug.Log("Idle -> Trace");
            _state = EMonsterState.Trace;
            return;
        }
    }

    private void Patrol()
    {
        
    }

    private void Trace()
    {
        // 애니메이션 재생
        _animator.SetTrigger("Trace");

        // Idle: trace타임이 오래되면
        if (_traceTimer >= TRACE_TIME)
        {
            _navMeshAgent.ResetPath();

            _traceTimer = 0f;
            UnityEngine.Debug.Log("Trace -> Idle");
            _state = EMonsterState.Idle;
            return;
        }
        
        // Idle: 플레이어가 죽으면
        if (_targetPlayer == null || _targetPlayer.State == EPlayerState.Death)
        {
            _navMeshAgent.ResetPath();

            UnityEngine.Debug.Log("Trace -> Idle");
            _state = EMonsterState.Idle;
            return;
        }
        
        // Attack: 플레이어가 범위 안
        Debug.Log(Vector3.Distance(_targetPlayer.transform.position, transform.position));
        if (Vector3.Distance(_targetPlayer.transform.position, transform.position) <= ATTACK_RANGE)
        {
            _navMeshAgent.ResetPath();

            _state = EMonsterState.Attack;
            return;
        }
        
        
        _navMeshAgent.SetDestination(_targetPlayer.transform.position);
    }

    private void Attack()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= ATTACK_COOLTIME)
        {
            _attackTimer = 0f;
            _animator.SetTrigger("Attack");
        }
    }

    private void Damaged()
    {
        
    }

    private void Death()
    {
        
    }
    
}
