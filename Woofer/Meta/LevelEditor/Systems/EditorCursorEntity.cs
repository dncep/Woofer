using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;

namespace WooferGame.Meta.LevelEditor.Systems
{
    class EditorCursorEntity : EditorEntity
    {
        public EditorCursorEntity()
        {
            Components.Add(new Spatial());
            Components.Add(new EditorCursor());
        }
    }
}
