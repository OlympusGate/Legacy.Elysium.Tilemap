using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Elysium.Tilemaps
{
    public class TilemapProperties<T>
    {
        public Tilemap Tilemap => tilemap;
        protected Tilemap tilemap;

        protected Dictionary<Vector3Int, T> TilemapDictionary;

        public event Action<Vector3Int> OnValueCreated;
        public event Action<Vector3Int> OnValueChanged;
        public event Action<Vector3Int> OnValueRemoved;

        public TilemapProperties(Tilemap _tilemap)
        {
            this.tilemap = _tilemap;
            TilemapDictionary = new Dictionary<Vector3Int, T>();
        }

        public void TriggerOnValueChanged(Vector3Int _position) => OnValueChanged?.Invoke(_position);

        public virtual bool GetValue(Vector3Int _position, out T _value)
        {
            if (!TilemapDictionary.ContainsKey(_position))
            {
                Debug.Log($"Value doesn't exist at {_position}.");
                _value = default(T);
                return false;
            }

            _value = TilemapDictionary[_position];
            return true;
        }

        public virtual bool CreateValue(Vector3Int _position, T _value)
        {
            if (TilemapDictionary.ContainsKey(_position)) 
            { 
                Debug.Log($"Value already exists at {_position}."); 
                return false; 
            }

            TilemapDictionary.Add(_position, _value);
            OnValueCreated?.Invoke(_position);
            OnValueChanged?.Invoke(_position);
            return true;
        }

        public virtual bool ChangeValue(Vector3Int _position, T _value)
        {
            if (!TilemapDictionary.ContainsKey(_position))
            {
                Debug.Log($"Value doesn't exist at {_position}.");
                return false;
            }

            TilemapDictionary[_position] = _value;
            OnValueChanged?.Invoke(_position);
            return true;
        }

        public virtual bool RemoveValue(Vector3Int _position)
        {
            if (!TilemapDictionary.ContainsKey(_position)) 
            {
                Debug.Log($"Value doesn't exist at {_position}.");
                return false;
            }

            TilemapDictionary.Remove(_position);
            OnValueRemoved?.Invoke(_position);
            OnValueChanged?.Invoke(_position);
            return true;
        }
    }
}