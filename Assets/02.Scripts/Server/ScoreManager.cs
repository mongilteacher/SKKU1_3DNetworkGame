using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    private int _score = 0;
    public int Score => _score;

    public override void OnJoinedRoom()
    {
        // 방에 들어가면 '내 점수가 0이다' 라는 내용으로 
        // 커스텀 프로퍼티를 초기화해준다.
        Refresh();
    }
    
    private void Refresh()
    {
        Hashtable hashTable = new Hashtable();
        hashTable.Add("Score", _score);
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashTable);
    }

    public void AddScore(int addedScore)
    {
        _score += addedScore;
        
        // 프로퍼티 밸류 수정
        Refresh();
    }

    // 플레이어의 커스텀 프로퍼티가 변경되면 호출되는 콜백 함수
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable hashtable)
    {
        Debug.Log($"Player {targetPlayer.NickName}_{targetPlayer.ActorNumber}의 점수: {hashtable["Score"]}");
    }

}
