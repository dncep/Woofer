using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;
using WooferGame.Systems;
using WooferGame.Systems.Physics;
using WooferGame.Systems.Visual;

namespace WooferGame.Meta.LevelEditor.Systems
{
    static class EditorUtil
    {
        internal static Rectangle GetSelectionBounds(Entity entity)
        {
            Spatial sp = entity.Components.Get<Spatial>();
            Rectangle realBounds = null;
            if (entity.Components.Has<SoftBody>())
            {
                SoftBody sb = entity.Components.Get<SoftBody>();
                CollisionBox cb = sb.Bounds;
                realBounds = new Rectangle(cb.X, cb.Y, cb.Width, cb.Height);
                if (sp != null) realBounds += sp.Position;
            }
            else if (entity.Components.Has<RigidBody>())
            {
                CollisionBox cb = entity.Components.Get<RigidBody>().UnionBounds;
                realBounds = new Rectangle(cb.X, cb.Y, cb.Width, cb.Height);
                if (sp != null) realBounds += sp.Position;
            }
            else if (entity.Components.Has<Renderable>())
            {
                Renderable renderable = entity.Components.Get<Renderable>();
                Rectangle union = null;
                foreach (Sprite sprite in renderable.Sprites)
                {
                    if (union == null) union = sprite.Destination;
                    else union = union.Union(sprite.Destination);
                }
                realBounds = union;
                if (sp != null) realBounds += sp.Position;
            }

            return realBounds;
        }
    }
}
