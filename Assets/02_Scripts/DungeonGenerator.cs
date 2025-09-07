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

    [Header("Configuracion")]
    public int roomCount = 10;
    public int spacing = 20;
    public int seed = 0;

    [Header("Loot / Decoracion")]
    [Range(0f, 1f)] public float chestSpawnChance = 0.3f;
    [Range(0f, 1f)] public float corridorChestChance = 0.05f;
    public int maxChests = 3; // ahora hasta 3 cofres

    private List<Room> rooms = new List<Room>();
    private System.Random rng;
    private int chestCount = 0;

    void Start()
    {
        Generate();
    }

    public void Generate()
    {
        // limpiar lo anterior
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        rng = (seed == 0) ? new System.Random() : new System.Random(seed);
        rooms.Clear();
        chestCount = 0;

        // primera sala
        Room start = new Room { position = Vector2Int.zero };
        rooms.Add(start);

        // generar mas salas
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

        // instanciar salas
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

            TrySpawnChestInRoom(room.instance);
        }

        // instanciar pasillos
        foreach (Room room in rooms)
        {
            foreach (Room other in room.connections)
            {
                if (other.instance == null) continue;

                Vector3 corridorPos = (room.instance.transform.position + other.instance.transform.position) / 2f;

                Quaternion rot = Quaternion.identity;
                if (room.position.x != other.position.x)
                {
                    rot = Quaternion.Euler(0, 90, 0);
                }

                GameObject corridor = Instantiate(corridorPrefab, corridorPos, rot, transform);

                TrySpawnChestInCorridor(corridor);
            }
        }
    }

    // spawnea cofres en habitaciones
    void TrySpawnChestInRoom(GameObject roomInstance)
    {
        if (chestPrefab == null || chestCount >= maxChests) return;

        if (rng.NextDouble() < chestSpawnChance)
        {
            Vector3 spawnPos = roomInstance.transform.position + Vector3.up * 0.5f;

            // si la habitacion tiene renderer, usamos sus bounds
            Renderer rend = roomInstance.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                Bounds b = rend.bounds;

                if (rng.NextDouble() < 0.5f)
                {
                    // centro
                    spawnPos = new Vector3(b.center.x, b.min.y + 0.5f, b.center.z);
                }
                else
                {
                    // esquinas
                    List<Vector3> corners = new List<Vector3>()
                    {
                        new Vector3(b.min.x + 1, b.min.y + 0.5f, b.min.z + 1),
                        new Vector3(b.min.x + 1, b.min.y + 0.5f, b.max.z - 1),
                        new Vector3(b.max.x - 1, b.min.y + 0.5f, b.min.z + 1),
                        new Vector3(b.max.x - 1, b.min.y + 0.5f, b.max.z - 1),
                    };
                    spawnPos = corners[rng.Next(corners.Count)];
                }
            }
            else
            {
                // fallback con spacing
                spawnPos = roomInstance.transform.position + new Vector3(0, 0.5f, 0);
            }

            Instantiate(chestPrefab, spawnPos, Quaternion.identity, roomInstance.transform);
            chestCount++;
        }
    }

    // spawnea cofres raros en pasillos
    void TrySpawnChestInCorridor(GameObject corridorInstance)
    {
        if (chestPrefab == null || chestCount >= maxChests) return;

        if (rng.NextDouble() < corridorChestChance)
        {
            Vector3 spawnPos = corridorInstance.transform.position + Vector3.up * 0.5f;
            Instantiate(chestPrefab, spawnPos, Quaternion.identity, corridorInstance.transform);
            chestCount++;
        }
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
