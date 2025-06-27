using Photon.Pun;
using UnityEngine;

public class PlayerMoveAbility : PlayerAbility, IPunObservable
{
    private CharacterController _characterController;
    private Animator _animator;
    
    private float _gravity = -9f;
    private float _yVelocity = 0f;

    private Vector3 _receivedPosition    = Vector3.zero;
    private Quaternion _receivedRotation = Quaternion.identity;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    // 데이터 동기화를 위한 데이터 전송 및 수신 기능
    // stream : 서버에서 주고받을 데이터가 담겨있는 변수
    // info   : 송수신 성공/실패 여부에 대한 로그
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("전송중");
            // 내꺼의 데이터만 보내준다...
            // 데이터를 전송하는 상황 -> 데이터를 보내주면 되고,
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else if (stream.IsReading)
        {
            Debug.Log("수신중");
            // 데이터를 수신하는 상황 -> 받은 데이터를 세팅하면 됩니다.
            // 보내준 순서대로 받는다.
            _receivedPosition = (Vector3)stream.ReceiveNext();
            _receivedRotation = (Quaternion)stream.ReceiveNext();
        }
    }
    
    
    private void Update()
    {
        // 포톤뷰가 내꺼가 아니라면
        // -> 내 캐릭터가 아니라면...
        if (_photonView.IsMine == false)
        {
            
            transform.position = Vector3.Lerp(transform.position, _receivedPosition, Time.deltaTime * 20f);
            transform.rotation = Quaternion.Slerp(transform.rotation, _receivedRotation, Time.deltaTime * 20f);
            
            return;
        }
        
        
        // 목표: 키보드 [W], [A], [S], [D] 키를 누르면 캐릭터를 그 방향으로 이동시키고 싶다.
        // 순서:
        // 1. 사용자의 키보드 입력 받기
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향 설정하기
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; // = dir.Normalize();
        
        // 카메라가 바라보는 방향 기준으로 수정하기
        dir = Camera.main.transform.TransformDirection(dir);
        _animator.SetFloat("Move", dir.magnitude);
        
        // 2-1. 수직 속도에 중력 값을 적용한다.
        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;
        
        // 2-2. 점프 적용
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = _owner.Stat.JumpPower;
        }
        
        // 3. 이동 속도에 따라 그 방향으로 이동하기
        // 캐릭터의 위치 = 현재 위치 + 속도  * 시간
        _characterController.Move(dir * _owner.Stat.MoveSpeed * Time.deltaTime);
    }
}