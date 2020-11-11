using Elysium.Timers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Tilemaps
{
    public class BuildingTileObject : SmartTileObject
    {
        public TestBuildingObject building { get; private set; }
        public GameObject gameObject { get; set; }
        public bool functional { get; set; }
        public TimerInstance activeStatusTimer { get; set; }

        public BuildingTileObject(Vector3Int _position, Action<Vector3Int> _callback, TestBuildingObject _building) : base(_position, _callback)
        {
            this.building = _building;
            this.functional = true;

            this.activeStatusTimer = Timer.CreateTimer(10f, false);
            this.activeStatusTimer.OnTimerEnd += Deactivate;
        }

        ~BuildingTileObject()
        {
            activeStatusTimer.isDestroyed = true;
            activeStatusTimer.SetTime(0.1f);
        }

        public void Deactivate()
        {
            if (!functional) { return; }

            functional = false;
            activeStatusTimer.SetTime(0f);
            TriggerOnValueChanged(position);
        }

        public void Repair()
        {
            if (functional) { return; }

            functional = true;
            activeStatusTimer.SetTime(10f);
            TriggerOnValueChanged(position);
        }
    }

    public class BuildingTilemap : SmartTilemap<BuildingTileObject>
    {
        public BuildingTileObject GetBuilding(Vector2 _position)
        {
            Vector3Int gridPosition = Tilemap.WorldToCell(_position);
            Properties.GetValue(gridPosition, out BuildingTileObject building);
            return building;
        }

        public BuildingTileObject GetBuilding(Vector3Int _position)
        {
            Properties.GetValue(_position, out BuildingTileObject building);
            return building;
        }

        public bool ConstructBuilding(Vector2 _position, TestBuildingObject _building)
        {
            var tilePosition = Tilemap.WorldToCell(_position);
            BuildingTileObject buildingObject = new BuildingTileObject(tilePosition, Properties.TriggerOnValueChanged, _building);

            Vector3Int gridPosition = Tilemap.WorldToCell(_position);
            return Properties.CreateValue(gridPosition, buildingObject);
        }

        public bool ConstructBuilding(Vector3Int _position, TestBuildingObject _building)
        {
            BuildingTileObject buildingObject = new BuildingTileObject(_position, Properties.TriggerOnValueChanged, _building);

            Vector3Int gridPosition = Tilemap.WorldToCell(_position);
            return Properties.CreateValue(gridPosition, buildingObject);
        }

        public bool UpdateBuilding(Vector2 _position, BuildingTileObject _building)
        {
            Vector3Int gridPosition = Tilemap.WorldToCell(_position);
            return Properties.ChangeValue(gridPosition, _building);
        }

        public bool DemolishBuilding(Vector2 _position)
        {
            Vector3Int gridPosition = Tilemap.WorldToCell(_position);
            return Properties.RemoveValue(gridPosition);
        }
    }
}