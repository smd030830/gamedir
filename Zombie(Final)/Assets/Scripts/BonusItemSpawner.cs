using UnityEngine;
using UnityEngine.AI;

// 신규 보너스 아이템을 별도 프리팹 없이 런타임에서 생성한다
public class BonusItemSpawner : MonoBehaviour {
    public float maxDistance = 7f;
    public float timeBetSpawnMin = 10f;
    public float timeBetSpawnMax = 16f;

    private Transform playerTransform;
    private float nextSpawnTime;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install() {
        if (Object.FindObjectOfType<BonusItemSpawner>() == null &&
            Object.FindObjectOfType<PlayerHealth>() != null)
        {
            GameObject spawner = new GameObject("Bonus Item Spawner");
            spawner.AddComponent<BonusItemSpawner>();
        }
    }

    private void Start() {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerTransform = playerHealth.transform;
        }

        ScheduleNextSpawn();
    }

    private void Update() {
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            return;
        }

        if (playerTransform == null)
        {
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth == null)
            {
                return;
            }

            playerTransform = playerHealth.transform;
        }

        if (Time.time >= nextSpawnTime)
        {
            Spawn();
            ScheduleNextSpawn();
        }
    }

    private void ScheduleNextSpawn() {
        nextSpawnTime = Time.time + Random.Range(timeBetSpawnMin, timeBetSpawnMax);
    }

    private void Spawn() {
        Vector3 spawnPosition = GetRandomPointOnNavMesh(playerTransform.position, maxDistance);
        spawnPosition += Vector3.up * 0.5f;

        bool spawnShotgun = Random.value < 0.5f;
        GameObject item = GameObject.CreatePrimitive(spawnShotgun ? PrimitiveType.Cube : PrimitiveType.Sphere);
        item.name = spawnShotgun ? "ShotgunPack" : "ShieldPack";
        item.transform.position = spawnPosition;
        item.transform.localScale = spawnShotgun ? new Vector3(0.7f, 0.35f, 0.35f) : Vector3.one * 0.55f;

        Collider itemCollider = item.GetComponent<Collider>();
        itemCollider.isTrigger = true;

        Rigidbody itemRigidbody = item.AddComponent<Rigidbody>();
        itemRigidbody.isKinematic = true;
        itemRigidbody.useGravity = false;

        Rotator rotator = item.AddComponent<Rotator>();
        rotator.rotationSpeed = spawnShotgun ? 95f : 70f;

        Light itemLight = item.AddComponent<Light>();
        itemLight.type = LightType.Point;
        itemLight.range = 2.5f;
        itemLight.intensity = 1.4f;
        itemLight.color = spawnShotgun ? Color.yellow : Color.cyan;

        Renderer renderer = item.GetComponent<Renderer>();
        renderer.material.color = spawnShotgun ? new Color(1f, 0.72f, 0.12f) : new Color(0.1f, 0.85f, 1f);

        if (spawnShotgun)
        {
            item.AddComponent<ShotgunPack>();
        }
        else
        {
            item.AddComponent<ShieldPack>();
        }

        Destroy(item, 8f);
    }

    private Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance) {
        Vector3 randomPos = Random.insideUnitSphere * distance + center;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return center;
    }
}
