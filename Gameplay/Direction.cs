﻿using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum Direction {
    NULL = 0,
    Up = 1,
    UpRight = 2,
    Right = 4,
    DownRight = 8,
    Down = 16,
    DownLeft = 32,
    Left = 64,
    UpLeft = 128
}

static class DirectionMethods {

    public static Vector3 GetVector3(this Direction direction) {
        return GetVector2(direction);
    }
    public static Vector2 GetVector2(this Direction direction) {
        Vector2 result = Vector2.zero;

        switch (direction) {
            case Direction.Up: result = new Vector2(0.0f, 1.0f); break;
            case Direction.UpRight: result = new Vector2(0.5f, 0.5f); break;
            case Direction.Right: result = new Vector2(1.0f, 0.0f); break;
            case Direction.DownRight: result = new Vector2(0.5f, -0.5f); break;
            case Direction.Down: result = new Vector2(0.0f, -1.0f); break;
            case Direction.DownLeft: result = new Vector2(-0.5f, -0.5f); break;
            case Direction.Left: result = new Vector2(-1.0f, 0.0f); break;
            case Direction.UpLeft: result = new Vector2(-0.5f, 0.5f); break;
        }

        return result;
    }

    public static Quaternion GetAngle(this Direction direction) {
        float result = 0;

        switch (direction) {
            case Direction.Up: result = 0.0f; break;
            case Direction.UpRight: result = -45.0f; break;
            case Direction.Right: result = -90.0f; break;
            case Direction.DownRight: result = -135.0f; break;
            case Direction.Down: result = -180.0f; break;
            case Direction.DownLeft: result = -225.0f; break;
            case Direction.Left: result = -270.0f; break;
            case Direction.UpLeft: result = -315.0f; break;
        }

        return Quaternion.Euler(0, 0, result);
    }

    public static Direction NewDirection(this Direction direction, List<Direction> possibleDirections) {
        Direction result = Direction.NULL;
        if (possibleDirections.Count > 0) {
            possibleDirections.ShuffleList();
            result = possibleDirections[0];
        }
        return result;
    }

}