using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Elysium.Tilemaps
{
    public abstract class SmartTilemap<T> : MonoBehaviour
    {
        public Tilemap Tilemap { get; protected set; }
        public TilemapProperties<T> Properties { get; protected set; }
        public IVisualTilemap<T> Visuals { get; protected set; }

        protected virtual void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
            Properties = new TilemapProperties<T>(Tilemap);
            Visuals = GetComponent<IVisualTilemap<T>>();

            if (Visuals == null) 
            {
                Debug.LogError("No visuals set in smart tilemap."); 
                return; 
            }

            Visuals.smartTilemap = this;
            Properties.OnValueCreated += Visuals.CreateVisuals;
            Properties.OnValueChanged += Visuals.UpdateVisuals;
            Properties.OnValueRemoved += Visuals.RemoveVisuals;
        }
    }
}