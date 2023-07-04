using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BackgroundColorChanger : MonoBehaviour
{
    [SerializeField] Color[] _colors;
    [SerializeField] Vector2 _minMaxColorRange;

    [SerializeField] SpriteRenderer _bgRenderer;

    public void ChangeBGColor()
    {
        // var color = _colors[level];
        //var color = new Color(GetRandomInt(), GetRandomInt(), GetRandomInt(), 255);
        var color = _bgRenderer.color;
        color.r = GetRandomFloat();
        color.g = GetRandomFloat();
        color.b = GetRandomFloat();
        _bgRenderer.color = color;
    }

    private float GetRandomFloat()
    {
        return Random.Range(_minMaxColorRange.x, _minMaxColorRange.y + 1);
    }
}