﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Interfaces.Visuals;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Saves.Json.Converter.DefaultConverters;
using GameInterfaces.Controller;
using WooferGame.Common;
using WooferGame.Systems.Physics;
using WooferGame.Systems.RoomBuilding;
using WooferGame.Systems.Sounds;
using WooferGame.Systems.Visual;
using WooferGame.Systems.Visual.Animation;

namespace WooferGame.Meta
{
    class WooferLoadOperation : LoadOperation
    {
        public WooferLoadOperation(IGameController controller, string name, string save) : base(controller, Path.Combine(Woofer.DirectoryPath, save, name + ".scn"))
        {
            this.AddConverter(new CollisionBoxConverter());
            this.AddConverter(new ColorConverter());
            this.AddConverter(new ListConverter<CollisionBox>());
            this.AddConverter(new ListConverter<Sound>());
            this.AddConverter(new ListConverter<Sprite>());
            this.AddConverter(new ListConverter<AnimatedSprite>());
            this.AddConverter(new EnumConverter<DrawMode>());
            this.AddConverter(new BoolMapConverter());
        }
    }
}