using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Interfaces.Input;
using EntityComponentSystem.Interfaces.Visuals;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Saves.Json.Converter.DefaultConverters;
using GameInterfaces.Input;
using WooferGame.Common;
using WooferGame.Controller.Commands;
using WooferGame.Input;
using WooferGame.Systems.HUD;
using WooferGame.Systems.Physics;
using WooferGame.Systems.RoomBuilding;
using WooferGame.Systems.Sounds;
using WooferGame.Systems.Visual;
using WooferGame.Systems.Visual.Animation;

namespace WooferGame.Systems.Debug
{
    [ComponentSystem("quicksave", ProcessingCycles.Input)]
    class Quicksave : ComponentSystem
    {
        private static readonly string TargetDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"Woofer/scenes");
        private static readonly string TargetFile = Path.Combine(TargetDirectory, "scene");

        static Quicksave()
        {
            Directory.CreateDirectory(TargetDirectory);
        }

        public override void Input()
        {
            ButtonInput saveButton = Woofer.Controller.InputManager.ActiveInputMap.Quicksave;
            ButtonInput loadButton = Woofer.Controller.InputManager.ActiveInputMap.Quickload;
            
            if (saveButton.Consume())
            {
                SaveOperation save = new SaveOperation(Owner, TargetFile);
                save.AddConverter(new CollisionBoxConverter());
                save.AddConverter(new ColorConverter());
                save.AddConverter(new ListConverter<CollisionBox>());
                save.AddConverter(new ListConverter<Sound>());
                save.AddConverter(new ListConverter<Sprite>());
                save.AddConverter(new ListConverter<AnimatedSprite>());
                save.AddConverter(new EnumConverter<DrawMode>());
                save.AddConverter(new BoolMapConverter());

                save.Save();
                Owner.Events.InvokeEvent(new ShowTextEvent("Saved", null));
            }

            if (loadButton.Consume())
            {
                LoadOperation load = new LoadOperation(Woofer.Controller, TargetFile);
                load.AddConverter(new CollisionBoxConverter());
                load.AddConverter(new ColorConverter());
                load.AddConverter(new ListConverter<CollisionBox>());
                load.AddConverter(new ListConverter<Sound>());
                load.AddConverter(new ListConverter<Sprite>());
                load.AddConverter(new ListConverter<AnimatedSprite>());
                load.AddConverter(new EnumConverter<DrawMode>());
                load.AddConverter(new BoolMapConverter());

                Woofer.Controller.CommandFired(new SceneChangeCommand(load.Load()));
                Woofer.Controller.ActiveScene.Events.InvokeEvent(new ShowTextEvent("Loaded", null));
                Woofer.Controller.Paused = false;
            }
        }
    }
}
