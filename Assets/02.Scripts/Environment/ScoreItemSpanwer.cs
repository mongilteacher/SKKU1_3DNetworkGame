using Photon.Pun;
using UnityEngine;

public class ScoreItemSpanwer : MonoBehaviour
{
    public float Interval;            // 몇초마다 생성할 것이냐
    private float _intervalTimer = 0;
    public float Range;               // 랜덤한 범위

    private void Start()
    {
        Interval = Random.Range(5f, 20f);
        Range = Random.Range(1f, 10f);
    }
    
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        _intervalTimer += Time.deltaTime;

        if (_intervalTimer >= Interval)
        {
            _intervalTimer = 0;
            
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * Range;
            randomPosition.y = 3f;
            
            ItemObjectFactory.Instance.RequestCreate(EItemType.Score, randomPosition);
        }
    }
}
