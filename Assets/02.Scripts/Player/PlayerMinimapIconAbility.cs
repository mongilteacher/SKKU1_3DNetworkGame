using UnityEngine;

public class PlayerMinimapIconAbility : PlayerAbility
{
    public GameObject MineIcon;
    public GameObject OtherIcon;

    private void Start()
    {
        bool mine = _photonView.IsMine;
        MineIcon.SetActive(mine);
        OtherIcon.SetActive(!mine);
    }
}
