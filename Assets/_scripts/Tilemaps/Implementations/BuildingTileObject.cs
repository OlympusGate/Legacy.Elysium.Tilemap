using Elysium.Timers;
using System;
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
}