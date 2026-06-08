using UnityEngine;

// 일정 시간 동안 플레이어 총을 샷건 모드로 바꾸는 아이템
public class ShotgunPack : MonoBehaviour, IItem {
    public float duration = 15f;
    public int bonusAmmo = 12;

    public void Use(GameObject target) {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        if (playerShooter != null && playerShooter.gun != null)
        {
            playerShooter.gun.ammoRemain += bonusAmmo;
            playerShooter.gun.EquipShotgun(duration);
        }

        Destroy(gameObject);
    }
}
