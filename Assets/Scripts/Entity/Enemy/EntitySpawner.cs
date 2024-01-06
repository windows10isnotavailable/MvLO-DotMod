using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NSMB.Utils;

public class EntitySpawner : MonoBehaviourPun {

    public float playerSearchRadius = 7, playerCloseCutoff = 1;
    public float initialSpawnTimer = 5;
    public int maxSpawnCount = 3;
    public string spawnObject = "Prefabs/Enemy/Goomba";

    public bool left = false;
    public bool autoDirection = false;

    private float spawnTimer;
    private readonly List<GameObject> entities = new();

    private Vector2 searchBox, closeSearchBox = new(1.5f, 1f), searchOffset, spawnOffset = new(0.25f, -0.2f);

    void Start() {
        searchBox = new(playerSearchRadius, playerSearchRadius);
        searchOffset = new(playerSearchRadius/2 + playerCloseCutoff, 0);
    }

    void Update() {
        if (!PhotonNetwork.IsMasterClient || GameManager.Instance.gameover)
            return;

        if ((spawnTimer -= Time.deltaTime) <= 0) {
            spawnTimer = initialSpawnTimer;
            TryToSpawn();
        }
    }

    void TryToSpawn() {
        for (int i = 0; i < entities.Count; i++) {
            if (entities[i] == null)
                entities.RemoveAt(i--);
        }
        if (entities.Count >= maxSpawnCount)
            return;

        //Check for players close by
        if (IntersectsPlayer(transform.position + Vector3.down * 0.25f, closeSearchBox))
            return;

        bool playerLeft = IntersectsPlayer((Vector2)transform.position - searchOffset, searchBox);
        bool direction = autoDirection ? playerLeft : left;

        GameObject newEntity = PhotonNetwork.InstantiateRoomObject(spawnObject, transform.position, Quaternion.identity, 0, new object[] { direction });
        entities.Add(newEntity);

    }

    bool IntersectsPlayer(Vector2 origin, Vector2 searchBox) {
        foreach (var hit in Physics2D.OverlapBoxAll(origin, searchBox, 0)) {
            if (hit.gameObject.CompareTag("Player"))
                return true;
        }
        return false;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position + Vector3.down * 0.5f, closeSearchBox);
        Gizmos.color = new Color(0, 0, 1, 0.5f);
        Gizmos.DrawCube((Vector2) transform.position - searchOffset, searchBox);
        Gizmos.DrawCube((Vector2) transform.position + searchOffset, searchBox);
    }
}