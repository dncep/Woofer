﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;

namespace WooferGame.Systems.Checkpoints
{
    [Event("checkpoint_request")]
    class CheckpointRequestEvent : Event
    {
        public Entity EntityToMove { get; private set; }

        public CheckpointRequestEvent(Component sender, Entity toMove) : base(sender)
        {
            this.EntityToMove = toMove;
        }
    }
}
