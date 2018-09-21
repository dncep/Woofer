using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using GameInterfaces.Controller;
using WooferGame.Meta.LevelEditor.Systems;
using WooferGame.Scenes;

namespace WooferGame.Meta.LevelEditor
{
    class Editor : IntroScene
    {
        public Editor() : base()
        {
            Controller.Paused = true;

            Systems.Add(new EntityListSystem());
            Systems.Add(new EntityOutliner());
            Systems.Add(new EditorRendering());
            Systems.Add(new EditorCursorSystem());

            List<Entity> delegates = new List<Entity>();
            
            foreach(Entity entity in Entities)
            {
                Entity delegateEntity = new EditorEntity();
                delegateEntity.Components.Add(new EntityDelegate(entity.Id));
                delegates.Add(delegateEntity);
            }

            Entities.Add(new EditorCursorEntity());

            Entities.AddRange(delegates);
        }
    }
}
