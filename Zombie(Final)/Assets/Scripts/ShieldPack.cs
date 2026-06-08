using UnityEngine;

// 일정 시간 동안 플레이어의 피해를 대신 받아주는 아이템
public class ShieldPack : MonoBehaviour, IItem {
    public float shieldAmount = 60f;
    public float duration = 10f;

    public void Use(GameObject target) {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.ActivateShield(shieldAmount, duration);
        }

        Destroy(gameObject);
    }
}
