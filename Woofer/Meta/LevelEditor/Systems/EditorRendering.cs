using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using GameInterfaces.Controller;

namespace WooferGame.Meta.LevelEditor.Systems
{
    [ComponentSystem("EditorRendering", ProcessingCycles.Render))]
    class EditorRendering : ComponentSystem
    {
        public override void Render<TSurface, TSource>(ScreenRenderer<TSurface, TSource> r)
        {
        }
    }
}
