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
using EntityComponentSystem.Interfaces.Visuals;
using EntityComponentSystem.Saves;
using EntityComponentSystem.Saves.Json.Converter.DefaultConverters;
using GameInterfaces.Input;
using WooferGame.Controller.Commands;
using WooferGame.Input;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Sounds;
using WooferGame.Systems.Visual;
using WooferGame.Systems.Visual.Animation;

namespace WooferGame.Systems.Debug
{
    [ComponentSystem("quicksave", ProcessingCycles.Input)]
    class Quicksave : ComponentSystem
    {
        private static InputTimeframe quicksave = new InputTimeframe(5);
        private static InputTimeframe quickload = new InputTimeframe(5);

        private static readonly string TargetDirectory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), @"Woofer/scenes");
        private static readonly string TargetFile = Path.Combine(TargetDirectory, "scene");

        static Quicksave()
        {
            Directory.CreateDirectory(TargetDirectory);
        }

        public override void Input()
        {
            ButtonState saveButton = Woofer.Controller.InputManager.ActiveInputMap.Quicksave;
            ButtonState loadButton = Woofer.Controller.InputManager.ActiveInputMap.Quickload;

            quicksave.RegisterState(saveButton);

            if (saveButton.IsPressed() && quicksave.Execute())
            {
                SaveOperation save = new SaveOperation(Owner);
                save.AddConverter(new CollisionBoxConverter());
                save.AddConverter(new ListConverter<CollisionBox>());
                save.AddConverter(new ListConverter<Sound>());
                save.AddConverter(new ListConverter<Sprite>());
                save.AddConverter(new ListConverter<AnimatedSprite>());
                save.AddConverter(new EnumConverter<DrawMode>());
                save.Save(TargetFile);
                Console.WriteLine("Scene saved");
            }

            quickload.RegisterState(loadButton);

            if (loadButton.IsPressed() && quickload.Execute())
            {
                LoadOperation load = new LoadOperation(TargetFile);
                load.AddConverter(new CollisionBoxConverter());
                load.AddConverter(new ListConverter<CollisionBox>());
                load.AddConverter(new ListConverter<Sound>());
                load.AddConverter(new ListConverter<Sprite>());
                load.AddConverter(new ListConverter<AnimatedSprite>());
                load.AddConverter(new EnumConverter<DrawMode>());

                Woofer.Controller.CommandFired(new SceneChangeCommand(load.Load()));
                Console.WriteLine("Scene loaded");
            }
        }
    }
}
