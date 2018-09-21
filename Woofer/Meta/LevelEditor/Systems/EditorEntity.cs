using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;

namespace WooferGame.Meta.LevelEditor.Systems
{
    class EditorEntity : Entity
    {
        public EditorEntity()
        {
            Components.Add(new EditorMarker());
        }
    }
}
