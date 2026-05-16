using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    private static Noise instance;

    public Noise()
    {
        if (instance != null && instance == this)
        {
            return;
        }

        instance = this;
    }

    //--------------------------------PRIVATE METHODS----------------------
    private Vector2 Fract(Vector2 v)
    {
        return new Vector2(
            v.x - Mathf.Floor(v.x),
            v.y - Mathf.Floor(v.y)
        );
    }

    private Vector2 RandomNoise(Vector2 st)
    {
        st = new Vector2(
            Vector2.Dot(st, new Vector2(127.1f, 311.7f)),
            Vector2.Dot(st, new Vector2(269.5f, 183.3f))
        );

        return new Vector2(
            -1.0f + 2.0f * Frac(Mathf.Sin(st.x) * 43758.5453123f),
            -1.0f + 2.0f * Frac(Mathf.Sin(st.y) * 43758.5453123f)
        );
    }

    private float Frac(float x)
    {
        return x - Mathf.Floor(x);
    }

    private float Mix(float a, float b, float t)
    {
        return a * (1.0f - t) + b * t;
    }

    private float GradientNoise(Vector2 st)
    {
        Vector2 i = new Vector2(
            Mathf.Floor(st.x),
            Mathf.Floor(st.y)
        );

        Vector2 f = Fract(st);

        Vector2 u = new Vector2(
            f.x * f.x * (3.0f - 2.0f * f.x),
            f.y * f.y * (3.0f - 2.0f * f.y)
        );

        float a = Vector2.Dot(
            RandomNoise(i + new Vector2(0.0f, 0.0f)),
            f - new Vector2(0.0f, 0.0f)
        );

        float b = Vector2.Dot(
            RandomNoise(i + new Vector2(1.0f, 0.0f)),
            f - new Vector2(1.0f, 0.0f)
        );

        float c = Vector2.Dot(
            RandomNoise(i + new Vector2(0.0f, 1.0f)),
            f - new Vector2(0.0f, 1.0f)
        );

        float d = Vector2.Dot(
            RandomNoise(i + new Vector2(1.0f, 1.0f)),
            f - new Vector2(1.0f, 1.0f)
        );

        return Mix(
            Mix(a, b, u.x),
            Mix(c, d, u.x),
            u.y
        );
    }

    //--------------------------------PUBLIC METHODS----------------------
    public float FBM_GradientNoise(
        Vector2Int position,
        float amplitude,
        float frequency,
        float persistence,
        float lacunarity,
        int octave)
    {
        float total = 0.0f;

        for (int i = 0; i < octave; i++)
        {
            total += GradientNoise(new Vector2(position.x, position.y) * frequency) * amplitude;

            frequency *= lacunarity;
            amplitude *= persistence;
        }

        return total;
    }

    public static Noise GetInstance()
    {
        return instance;
    }
}