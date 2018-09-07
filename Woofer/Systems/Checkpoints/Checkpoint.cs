using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;

namespace WooferGame.Systems.Checkpoints
{
    class Checkpoint : Entity
    {
        public Checkpoint(double x, double y) : this(x, y, false)
        {

        }

        public Checkpoint(double x, double y, bool selected)
        {
            this.Components.Add(new Spatial(x, y));
            this.Components.Add(new CheckpointComponent() { Selected = selected });
        }
    }
}
