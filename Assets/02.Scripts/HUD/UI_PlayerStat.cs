using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStat : MonoBehaviour
{
    // Q. 나의 플레이어를 어떻게 찾을것인가?
    //    - 할당이 불가능한 이유는 : 나의 플레이어는 없었다가. 동적으로 생긴다.
    
    // 1. UI_PlayerStat에서 Player 찾기
        // - 1-1. Start혹은 코루틴, update에서 찾기    (운빨: 코드가 바뀌면 망가진다)
        // - 1-2. GameEvent 호출되면 그때 찾기         (옵저버, 이벤트)  (마이크로FPS)
        
    // 2. Player에서 UI_PlayerStat 찾기               (하드코딩)
    
    
    private Player _player;

    public Slider HealthSlider;
    public Slider StatminaSlider;

    public void SetPlayer(Player player)
    {
        _player = player;
    }
    
    
    private void Update()
    {
        if (_player == null)
        {
            return;
        }
        
        HealthSlider.value = _player.Stat.Health /  _player.Stat.MaxHealth;
        StatminaSlider.value = _player.Stat.Stamina /  _player.Stat.MaxStamina;
    }
    
}
