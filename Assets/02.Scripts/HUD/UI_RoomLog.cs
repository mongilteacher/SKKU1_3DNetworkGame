using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    public TextMeshProUGUI LogTextUI;

    private string _logMessage = "방에 입장했습니다.";
    
    private void Start()
    {
        RoomManager.Instance.OnPlayerEntered += PlayerEnterLog;
        RoomManager.Instance.OnPlayerExited  += PlayerExitLog;
        
        Refresh();
    }
    
    private void Refresh()
    {
        LogTextUI.text = _logMessage;
    }


    public void PlayerEnterLog(string playerName)
    {
        // 구글 검색: 유니티 rich text
        _logMessage += $"\n<color=#00ff00ff>{playerName}</color>님이 <color=blue>입장</color>하였습니다.";
        Refresh();
    }

    public void PlayerExitLog(string playerName)
    {
        _logMessage += $"\n<color=#00ff00ff>{playerName}</color>님이 <color=red>퇴장</color>하였습니다.";
        Refresh();
    }
    
}
