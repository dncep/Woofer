using System;
using System.Collections.Generic;
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
                save.Save(@"C:\Users\Usuario\Desktop\scene.json");
                Console.WriteLine("Scene saved");
            }

            quickload.RegisterState(loadButton);

            if (loadButton.IsPressed() && quickload.Execute())
            {
                LoadOperation load = new LoadOperation(@"C:\Users\Usuario\Desktop\scene.json");
                load.AddConverter(new CollisionBoxConverter());
                load.AddConverter(new ListConverter<CollisionBox>());
                load.AddConverter(new ListConverter<Sound>());
                load.AddConverter(new ListConverter<Sprite>());
                load.AddConverter(new ListConverter<AnimatedSprite>());
                load.AddConverter(new EnumConverter<DrawMode>());

                Woofer.Controller.ActiveScene = load.Load();
                Console.WriteLine("Scene loaded");
            }
        }
    }
}
