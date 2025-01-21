using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerInput
{
    public FingerInput(int id, Vector2 position, Vector2 velocity, float acceleration, Mode mode)
    {
        this.id = id;
        this.position = position;
        this.velocity = velocity;
        this.acceleration = acceleration;
        this.mode = mode;
    }

    public int id { get; }

    public Vector2 position { get; private set; }

    public Vector2 velocity { get; private set; }
    public float acceleration { get; private set; }

    public Mode mode { get; private set; }

    public void UpdateProps(Vector2 position, Vector2 velocity, float acceleration, Mode mode)
    {
        this.position = position;
        this.velocity = velocity;
        this.acceleration = acceleration;
        this.mode = mode;
    }

    public void UpdateProps(Mode mode)
    {
        this.mode = mode;
    }
}