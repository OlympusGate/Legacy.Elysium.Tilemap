using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Tilemaps
{
    public interface IVisualTilemap<T>
    {
        SmartTilemap<T> smartTilemap { get; set; }
        void CreateVisuals(Vector3Int _position);
        void UpdateVisuals(Vector3Int _position);
        void RemoveVisuals(Vector3Int _position);
    }
}