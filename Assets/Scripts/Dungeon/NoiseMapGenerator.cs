using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseMapGenerator : MonoBehaviour
{
    [Header("Map Dimensions")]
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 100;

    [Header("FBM Parameters")]
    [SerializeField] private float amplitude = 1.0f;
    [SerializeField] private float frequency = 0.05f;
    [SerializeField] private float persistence = 0.5f;
    [SerializeField] private float lacunarity = 2.0f;
    [SerializeField] private int octaves = 4;

    [Header("Tile Thresholds  (must sum to 1.0 and be in ascending order)")]
    [SerializeField] private float deepWaterThreshold = 0.2f;  // noise ≤ this → deep water
    [SerializeField] private float shallowWaterThreshold = 0.35f;
    [SerializeField] private float sandThreshold = 0.42f;
    [SerializeField] private float grassThreshold = 0.65f;
    [SerializeField] private float forestThreshold = 0.80f;
    // anything above forestThreshold → mountain

    [Header("Tiles")]
    [SerializeField] private TileBase deepWaterTile;
    [SerializeField] private TileBase shallowWaterTile;
    [SerializeField] private TileBase sandTile;
    [SerializeField] private TileBase grassTile;
    [SerializeField] private TileBase forestTile;
    [SerializeField] private TileBase mountainTile;

    [Header("Tilemaps  (one per layer, or reuse the same one)")]
    [SerializeField] private Tilemap tilemap;

    // ------------------------------------------------------------------ //
    //  Lifecycle
    // ------------------------------------------------------------------ //
    private void Start()
    {
        // Make sure the singleton is alive before we call it
        if (Noise.GetInstance() == null)
            new Noise();
        GenerateMap();
    }

    // ------------------------------------------------------------------ //
    //  Public API
    // ------------------------------------------------------------------ //
    public void GenerateMap()
    {
        float[,] noiseMap = BuildNoiseMap();
        float min, max;
        GetMinMax(noiseMap, out min, out max);

        PlaceTiles(noiseMap, min, max);
    }

    // ------------------------------------------------------------------ //
    //  Private helpers
    // ------------------------------------------------------------------ //

    /// <summary>Samples the FBM noise for every cell and stores raw values.</summary>
    private float[,] BuildNoiseMap()
    {
        // 2D array, contiguous memory, best for memory
        float[,] map = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = Noise.GetInstance().FBM_GradientNoise(
                    new Vector2Int(x, y),
                    amplitude,
                    frequency,
                    persistence,
                    lacunarity,
                    octaves
                );

                //Debug.Log($"value of noise map at {x}, {y} = {map[x, y]}");
            }
        }

        return map;
    }

    /// <summary>Finds the raw min/max so we can normalise to [0, 1].</summary>
    private void GetMinMax(float[,] map, out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;

        foreach (float v in map)
        {
            if (v < min) min = v;
            if (v > max) max = v;
        }
    }

    /// <summary>Normalises each sample and sets the matching tile.</summary>
    private void PlaceTiles(float[,] noiseMap, float min, float max)
    {
        float range = max - min;

        // iterate through every values of the noiseMap
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float normalized = 0f;

                // eliminate edge case where max noise value = min noise value
                if (range > 0f)
                {
                    normalized = (noiseMap[x, y] - min) / range;
                    
                    TileBase tile = SelectTile(normalized);

                    if (tile != null)
                    {
                        // draw tile at position x y 
                        Vector2Int position = new Vector2Int(x, y);
                        TilemapVisualizer.GetInstance().DrawSingleTile(position, tilemap, tile);
                    }
                }
            }
        }
    }

    /// <summary>Maps a normalised noise value [0, 1] to a tile.</summary>
    private TileBase SelectTile(float value)
    {
        if (value <= deepWaterThreshold) return deepWaterTile;
        if (value <= shallowWaterThreshold) return shallowWaterTile;
        if (value <= sandThreshold) return sandTile;
        if (value <= grassThreshold) return grassTile;
        if (value <= forestThreshold) return forestTile;
        return mountainTile;                // mountain / snow peak
    }
}