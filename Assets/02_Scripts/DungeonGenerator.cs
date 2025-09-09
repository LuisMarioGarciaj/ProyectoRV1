using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Room
    {
        public Vector2Int position;

        [System.NonSerialized]
        public List<Room> connections = new List<Room>();

        [System.NonSerialized]
        public GameObject instance;
    }

    [Header("Prefabs")]
    public GameObject roomPrefab;
    public GameObject corridorPrefab;
    public GameObject paredPartidaPuertaPrefab;
    public GameObject chestPrefab;
    public GameObject coinPrefab; // Prefab de moneda

    [Header("Configuracion")]
    public int roomCount = 10;
    public int spacing = 20;
    public int seed = 0;

    [Header("Loot / Cofres")]
    [Range(0f, 1f)] public float chestSpawnChance = 0.3f;
    [Range(0f, 1f)] public float corridorChestChance = 0.05f;
    public int maxChests = 3;
    private int chestCount = 0;

    [Header("Monedas")]
    [Range(0f, 1f)] public float coinSpawnChance = 0.7f; // mas frecuente
    [Range(0f, 1f)] public float corridorCoinChance = 0.3f;
    public int maxCoins = 20;
    private int coinCount = 0;
    public float coinSpacing = 1f; // distancia entre monedas

    private List<Room> rooms = new List<Room>();
    private System.Random rng;


    [Header("Pociones")]
    public GameObject fireRatePotionPrefab;
    public GameObject healthPotionPrefab;
    public GameObject speedPotionPrefab;
    [Header("Spawn Pociones")]
    [Range(0f, 1f)] public float potionSpawnChance = 0.4f; // probabilidad de poción en room
    [Range(0f, 1f)] public float corridorPotionChance = 0.1f; // probabilidad en pasillo
    public int maxPotions = 5; // máximo de pociones en todo el mapa
    private int potionCount = 0;

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        rng = (seed == 0) ? new System.Random() : new System.Random(seed);
        rooms.Clear();
        chestCount = 0;
        coinCount = 0;

        Room start = new Room { position = Vector2Int.zero };
        rooms.Add(start);

        for (int i = 1; i < roomCount; i++)
        {
            Room baseRoom = rooms[rng.Next(rooms.Count)];
            Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            Vector2Int dir = dirs[rng.Next(dirs.Length)];
            Vector2Int newPos = baseRoom.position + dir;

            if (rooms.Exists(r => r.position == newPos))
            {
                i--;
                continue;
            }

            Room newRoom = new Room { position = newPos };
            rooms.Add(newRoom);

            baseRoom.connections.Add(newRoom);
            newRoom.connections.Add(baseRoom);
        }

        foreach (Room room in rooms)
        {
            Vector3 worldPos = new Vector3(room.position.x * spacing, 0, room.position.y * spacing);
            room.instance = Instantiate(roomPrefab, worldPos, Quaternion.identity, transform);

            foreach (Room other in room.connections)
            {
                Vector2Int diff = other.position - room.position;

                if (diff == Vector2Int.up) ReemplazarPorPuerta(room.instance, "Wall_Norte");
                else if (diff == Vector2Int.down) ReemplazarPorPuerta(room.instance, "Wall_Sur");
                else if (diff == Vector2Int.left) ReemplazarPorPuerta(room.instance, "Wall_Oeste");
                else if (diff == Vector2Int.right) ReemplazarPorPuerta(room.instance, "Wall_Este");
            }

            // Spawn cofres y monedas al nivel del piso
            TrySpawnChestInRoom(room.instance);
            TrySpawnCoinsInRoom(room.instance);
            TrySpawnPotionInRoom(room.instance);
        }

        foreach (Room room in rooms)
        {
            foreach (Room other in room.connections)
            {
                if (other.instance == null) continue;

                Vector3 corridorPos = (room.instance.transform.position + other.instance.transform.position) / 2f;

                Quaternion rot = Quaternion.identity;
                if (room.position.x != other.position.x)
                    rot = Quaternion.Euler(0, 90, 0);

                GameObject corridor = Instantiate(corridorPrefab, corridorPos, rot, transform);

                TrySpawnChestInCorridor(corridor);
                TrySpawnCoinsInCorridor(corridor);
                TrySpawnPotionInCorridor(corridor);
            }
        }
    }

    // Cofres al nivel del suelo
    void TrySpawnChestInRoom(GameObject roomInstance)
    {
        if (chestPrefab == null || chestCount >= maxChests) return;

        if (rng.NextDouble() < chestSpawnChance)
        {
            Vector3 spawnPos = roomInstance.transform.position;
            spawnPos.y += 0.5f; // altura sobre el piso

            Instantiate(chestPrefab, spawnPos, chestPrefab.transform.rotation, roomInstance.transform);
            chestCount++;
        }
    }

    void TrySpawnChestInCorridor(GameObject corridorInstance)
    {
        if (chestPrefab == null || chestCount >= maxChests) return;

        if (rng.NextDouble() < corridorChestChance)
        {
            Vector3 spawnPos = corridorInstance.transform.position;
            spawnPos.y += 0.5f;

            Instantiate(chestPrefab, spawnPos, chestPrefab.transform.rotation, corridorInstance.transform);
            chestCount++;
        }
    }

    // Monedas al nivel del suelo
    void TrySpawnCoinsInRoom(GameObject roomInstance)
    {
        if (coinPrefab == null || coinCount >= maxCoins) return;

        if (rng.NextDouble() < coinSpawnChance)
        {
            int coinsToSpawn = rng.Next(1, 4); // 1-3 monedas por habitación

            for (int i = 0; i < coinsToSpawn && coinCount < maxCoins; i++)
            {
                Vector3 spawnPos = roomInstance.transform.position;
                spawnPos.x += (float)(rng.NextDouble() * 2 - 1) * coinSpacing; // un poco al azar
                spawnPos.z += (float)(rng.NextDouble() * 2 - 1) * coinSpacing;
                spawnPos.y += 0.5f; // altura piso

                Instantiate(coinPrefab, spawnPos, coinPrefab.transform.rotation, roomInstance.transform);
                coinCount++;
            }
        }
    }

    void TrySpawnCoinsInCorridor(GameObject corridorInstance)
    {
        if (coinPrefab == null || coinCount >= maxCoins) return;

        if (rng.NextDouble() < corridorCoinChance)
        {
            int coinsToSpawn = rng.Next(1, 3); // 1-2 monedas

            for (int i = 0; i < coinsToSpawn && coinCount < maxCoins; i++)
            {
                Vector3 spawnPos = corridorInstance.transform.position;
                spawnPos.y += 0.5f;

                Instantiate(coinPrefab, spawnPos, coinPrefab.transform.rotation, corridorInstance.transform);
                coinCount++;
            }
        }
    }
    void TrySpawnPotionInRoom(GameObject roomInstance)
    {
        if (potionCount >= maxPotions) return;
        if (rng.NextDouble() > potionSpawnChance) return;

        // elegir una poción aleatoria
        GameObject[] potions = { fireRatePotionPrefab, healthPotionPrefab, speedPotionPrefab };
        GameObject potionPrefab = potions[rng.Next(potions.Length)];
        if (potionPrefab == null) return;

        Vector3 spawnPos = roomInstance.transform.position;
        spawnPos.x += (float)(rng.NextDouble() * 2 - 1); // un poco al azar
        spawnPos.z += (float)(rng.NextDouble() * 2 - 1);
        spawnPos.y += 0.5f; // al nivel del piso

        Instantiate(potionPrefab, spawnPos, potionPrefab.transform.rotation, roomInstance.transform);
        potionCount++;
    }

    void TrySpawnPotionInCorridor(GameObject corridorInstance)
    {
        if (potionCount >= maxPotions) return;
        if (rng.NextDouble() > corridorPotionChance) return;

        GameObject[] potions = { fireRatePotionPrefab, healthPotionPrefab, speedPotionPrefab };
        GameObject potionPrefab = potions[rng.Next(potions.Length)];
        if (potionPrefab == null) return;

        Vector3 spawnPos = corridorInstance.transform.position;
        spawnPos.y += 0.5f;

        Instantiate(potionPrefab, spawnPos, potionPrefab.transform.rotation, corridorInstance.transform);
        potionCount++;
    }

    void ReemplazarPorPuerta(GameObject roomInstance, string wallName)
    {
        Transform wall = roomInstance.transform.Find(wallName);
        if (wall != null && paredPartidaPuertaPrefab != null)
        {
            Vector3 pos = wall.position;
            Transform parent = wall.parent;

            Destroy(wall.gameObject);

            Transform newWall = Instantiate(paredPartidaPuertaPrefab, pos, Quaternion.identity, parent).transform;

            switch (wallName)
            {
                case "Wall_Norte": newWall.localRotation = Quaternion.Euler(0, 0, 0); break;
                case "Wall_Sur": newWall.localRotation = Quaternion.Euler(0, 180, 0); break;
                case "Wall_Este": newWall.localRotation = Quaternion.Euler(0, 90, 0); break;
                case "Wall_Oeste": newWall.localRotation = Quaternion.Euler(0, -90, 0); break;
            }
        }
    }
}
