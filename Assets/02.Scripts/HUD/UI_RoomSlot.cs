using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class UI_RoomSlot : MonoBehaviour
{
    public TextMeshProUGUI RoomNameTextUI;
    public TextMeshProUGUI MasterNicknameTextUI;
    public TextMeshProUGUI PlayerCountTextUI;

    private RoomInfo _roomInfo;
    
    public void Refresh(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        
        RoomNameTextUI.text = roomInfo.Name;
        MasterNicknameTextUI.text = "알수없음";
        PlayerCountTextUI.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
    }

    public void OnClick()
    {
        LobbyScene.Instance.TryJoinRoom(_roomInfo.Name);
    }
}
