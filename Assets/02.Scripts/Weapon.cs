using UnityEngine;

public class Weapon : MonoBehaviour
{
    private PlayerAttackAbility _attackAbility;

    private void Start()
    {
        _attackAbility = GetComponentInParent<PlayerAttackAbility>();

        ScoreManager.Instance.OnDataChanged += Refresh;
        
        Refresh();
    }

    private void Refresh()
    {
        int score = ScoreManager.Instance.Score;

        int factor = 1 + score / 10000;
        
        transform.localScale = new Vector3(factor, factor, factor);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신과 부딛혔다면 아무고또 안한다.
        if (other.transform == _attackAbility.transform)
        {
            return;
        }
        
        // IDamaged 인터페이스를 구현하고 있는지 확인
        if (other.GetComponent<IDamaged>() != null)
        {
            _attackAbility.Hit(other);
        }
    }
}
