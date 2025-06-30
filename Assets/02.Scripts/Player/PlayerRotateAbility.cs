using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    // 목표: 마우스를 조작하면 캐릭터/카메라를 그 방향으로 회전시키고 싶다.
    public Transform CameraRoot;
    
    // 마우스 입력값을 누적할 변수
    private float _mx;
    private float _my;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;


        // 내 캐릭터라면...
        if (_photonView.IsMine)
        {
            CinemachineCamera camera = GameObject.FindWithTag("FollowCamera").GetComponent<CinemachineCamera>();
            camera.Follow = CameraRoot;
            
            UI_PlayerStat ui_playerStat = FindAnyObjectByType<UI_PlayerStat>();
            ui_playerStat.SetPlayer(_owner);
            
            MinimapCamera minimapCamera = FindAnyObjectByType<MinimapCamera>();
            minimapCamera.Target = CameraRoot;
        }
    }
    
    private void Update()
    {
        if (_photonView.IsMine == false)
        {
            return;
        }
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
       
        _mx += mouseX * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += mouseY * _owner.Stat.RotationSpeed * Time.deltaTime;
        
        _my = Mathf.Clamp(_my, -90f, 90f);
        
        // - 캐릭터 
        //    ㄴ 카메라 루트
        
        // y축 회전은 캐릭터만한다.
        transform.eulerAngles = new Vector3(0f, _mx, 0f);

        // x축 회전은 캐릭터는 하지 않는다. (즉, 카메라 루트만 x축 회전하면 된다.)
        CameraRoot.localEulerAngles = new Vector3(-_my, 0f, 0f);
    }
}
