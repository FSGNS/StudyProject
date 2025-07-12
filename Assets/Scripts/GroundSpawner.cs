using UnityEngine;
using System.Collections.Generic;

public class GroundSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject groundTilePrefab;
    public GameObject thornPrefab;
    public bool invertGravity;
    public Vector3 thornOffset;
    public float thornChance;
    public int minTilesBeforeThorn;
    public int maxConsecutiveThorns;
    public int minSafeTilesAfterThorns;
    public int tilesAhead = 10;
    public int tilesBehind = 10;
    // public int initialHeight = 0;
    public int initialLevel = 0;
    public int tilesWithoutHeightChange = 10;
    public int minHeightChange = -5;
    public int maxHeightChange = 5;    

    private float lastTileX = 0f;
    private Queue<GameObject> tiles = new Queue<GameObject>();
    private int tileCount = 0;

    private float tileWidth;
    private float tileHeight;
    private float baseY = 0f;
    private int currentLevel = 0;
    private int tilesUntilNextHeightChange = 0;

    private ThornState thornState = ThornState.CanPlace;
    private int thornStreak = 0;
    private int safeStreak = 0;
    private int groundDirectionMultiplier = 1;
    private int thornDirectionMultiplier = 1;


    private enum ThornState
    {
        CanPlace,
        PlacingThorns,
        Cooldown
    }

    void Start()
    {
        SpriteRenderer sr = groundTilePrefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            tileWidth = sr.bounds.size.x;
            tileHeight = sr.bounds.size.y;
        }

        thornDirectionMultiplier = invertGravity ? -1 : 1;
        groundDirectionMultiplier = invertGravity ? 1 : -1;

        currentLevel = initialLevel;
        // baseY = player.position.y + groundDirectionMultiplier * tileHeight / 2f;
        float playerHeightOffset = 0f;

        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerHeightOffset = playerCollider.bounds.size.y / 2f;
        }

        baseY = player.position.y + groundDirectionMultiplier * (playerHeightOffset + tileHeight / 2f);        

        for (int i = 0; i < tilesAhead; ++i)
        {
            bool allowElevation = i >= tilesWithoutHeightChange;
            SpawnNextTile(allowElevation);
        }
    }

    void Update()
    {
        if (player.position.x + tilesAhead * tileWidth > lastTileX)
        {
            SpawnNextTile(allowElevation: true);
        }

        if (tiles.Count > tilesAhead + tilesBehind + 1)
        {
            GameObject oldTile = tiles.Dequeue();
            Destroy(oldTile);
        }
    }

    void SpawnNextTile(bool allowElevation)
    {
        if (allowElevation)
        {
            if (tilesUntilNextHeightChange <= 0)
            {
                int minStep = Mathf.Max(minHeightChange, currentLevel - 1);
                int maxStep = Mathf.Min(maxHeightChange, currentLevel + 1);

                currentLevel = Random.Range(minStep, maxStep + 1);
                tilesUntilNextHeightChange = tilesWithoutHeightChange;
            }
            else
            {
                --tilesUntilNextHeightChange;
            }
        }

        float y = baseY + currentLevel * tileHeight;
        Vector3 spawnPos = new Vector3(lastTileX, y, 0f);

        GameObject newTile = Instantiate(groundTilePrefab, spawnPos, Quaternion.identity);
        tiles.Enqueue(newTile);

        TryPlaceThorn(newTile);

        lastTileX += tileWidth;
        ++tileCount;
    }

    private void TryPlaceThorn(GameObject groundTile)
    {
        if (tileCount < minTilesBeforeThorn)
            return;

        switch (thornState)
        {
            case ThornState.CanPlace:
                if (Random.value < thornChance)
                {
                    PlaceThorn(groundTile);
                    thornStreak = 1;
                    thornState = ThornState.PlacingThorns;
                }
                break;

            case ThornState.PlacingThorns:
                if (thornStreak < maxConsecutiveThorns && Random.value < thornChance)
                {
                    PlaceThorn(groundTile);
                    thornStreak++;
                }
                else
                {
                    thornState = ThornState.Cooldown;
                    safeStreak = 0;
                }
                break;

            case ThornState.Cooldown:
                ++safeStreak;
                if (safeStreak >= minSafeTilesAfterThorns)
                {
                    thornState = ThornState.CanPlace;
                    thornStreak = 0;
                    safeStreak = 0;
                }
                break;
        }
    }

    private void PlaceThorn(GameObject groundTile)
    {
        SpriteRenderer groundRenderer = groundTile.GetComponent<SpriteRenderer>();
        float groundHeight = groundRenderer.bounds.size.y;

        SpriteRenderer thornRenderer = thornPrefab.GetComponent<SpriteRenderer>();
        float thornHeight = thornRenderer.bounds.size.y;

        Vector3 groundPos = groundTile.transform.position;

        float spikeY = groundPos.y + thornDirectionMultiplier * (groundHeight / 2 + thornHeight / 2);
        Vector3 spikePos = new Vector3(groundPos.x, spikeY, groundPos.z);

        GameObject thorn = Instantiate(thornPrefab, spikePos, Quaternion.identity);

        if (thornDirectionMultiplier < 0)
        {
            thorn.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }    
}
