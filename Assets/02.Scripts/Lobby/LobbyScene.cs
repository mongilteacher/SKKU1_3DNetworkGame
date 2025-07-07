using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public enum ECharacterType
{
    Male,
    Female
}

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public TMP_InputField NickinameInputField;
    public TMP_InputField RoomNameInputField;

    public static ECharacterType CharacterType = ECharacterType.Male;
    public GameObject MaleCharacter;
    public GameObject FemaleCharacter;
    
    // 로버트C마틴 -> 클린코드(1장. 깨끗한 코드 -> 변수명)
    // 큰거에 대해서 신뢰감을 쌓기 위해서는 작은거부터 신뢰감을 쌓아가야한다.
    // 일관성있는 용어를 써라. (List vs 복수형 -> 둘 중 하나만 써라)
    private List<RoomInfo> _roomList;
    public List<RoomInfo> RoomList => _roomList;
    public event Action OnDataChanged; 
    
    
    
    
    public void OnClickMaleCharacter() => OnClickCharacterTypeButton(ECharacterType.Male);
    public void OnClickFemaleCharacter() => OnClickCharacterTypeButton(ECharacterType.Female);
    public void OnClickCharacterTypeButton(ECharacterType characterType)
    {
        // 파라미터(매개변수) vs 인자
        // parameter vs argument
        
        CharacterType = characterType;
        
        MaleCharacter.SetActive(characterType == ECharacterType.Male);
        FemaleCharacter.SetActive(characterType == ECharacterType.Female);
    }
    
    // 싱글톤 상속받아서 쓰는데;
    // 현업
    // - 1. 종류별로 따로 기본 스크립트를 만든다.
    //   - C# 전용 (모노비헤비어 X) - Singleton
    //   - DontDestroyOnLoad     - PermanentSingletonBehaviour
    //   - DontDestroyOnLoad X   - SingletonBehaviour
    
    // - 2. 위 내용을 옵션으로 선태할 수 있게..
    //   - 위 내용 뿐만 아니라
    //   - 초기화 시점도 선택가능하게.. (Awake, Start, Lazy)

    public static LobbyScene Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        OnClickMaleCharacter();
    }
    
    
    
    // 방 만들기 함수를 호출
    public void OnClickMakeRoomButton()
    {
        MakeRoom();
    }

    private void MakeRoom()
    {
        string nickname = NickinameInputField.text;
        string roomName = RoomNameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            return;
        }
        
        // 포톤에 닉네임 등록
        PhotonNetwork.NickName = nickname;
        
        // Room 속성 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;   
        roomOptions.IsOpen = true;     // 룸 입장 가능 여부
        roomOptions.IsVisible = true;  // 로비(채널) 룸 목록에 노출시킬지 여부
        
        // Room 생성
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    
    // 룸 목록을 수신하는 콜백 함수
    // 내가 입장한 로비(채널)에서 룸이 수정/삭제/추가되면 호출되는 콜백 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomList = roomList;
        OnDataChanged?.Invoke();
        
        foreach (RoomInfo room in roomList)
        {
            // UI에 필요한 내용: 방 이름, 방장명, 인원수
            Debug.Log($"{room.Name}(방장명): ({room.PlayerCount}/{room.MaxPlayers})");
        }
    }

    public void TryJoinRoom(string roomName)
    {
        string nickname = NickinameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            return;
        }

        PhotonNetwork.NickName = nickname;
        
        PhotonNetwork.JoinRoom(roomName);

        return;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
