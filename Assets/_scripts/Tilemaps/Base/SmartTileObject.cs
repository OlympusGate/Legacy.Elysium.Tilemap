using Elysium.Timers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Tilemaps
{
    public class SmartTileObject
    {
        public Vector3Int position { get; private set; }

        public event Action<Vector3Int> OnValueChanged;

        public SmartTileObject(Vector3Int _position, Action<Vector3Int> _callback)
        {
            this.position = _position;
            OnValueChanged += _callback;
        }

        public void TriggerOnValueChanged(Vector3Int _position)
        {
            OnValueChanged?.Invoke(_position);
        }
    }
}