using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;
using WooferGame.Systems.Physics;
using Color = System.Drawing.Color;

namespace WooferGame.Meta.LevelEditor.Systems.EntityOutlines
{
    interface IOutline
    {
        Rectangle Bounds { get; }
        Color Color { get; }
        int Thickness { get; }
    }

    class RectangleOutline : IOutline
    {
        public Rectangle Bounds { get; set; }
        public Color Color { get; set; }
        public int Thickness { get; set; } = 2;

        public RectangleOutline(Rectangle bounds, Color color) : this(bounds, color, 4)
        {

        }

        public RectangleOutline(Rectangle bounds, Color color, int thickness)
        {
            Bounds = bounds;
            Color = color;
            Thickness = thickness;
        }
    }

    class CollisionBoxOutline : IOutline
    {
        public CollisionBox Box { get; set; }
        public Color Color { get; set; }
        public int Thickness { get; set; } = 2;

        private Vector2D Pivot;
        public Rectangle Bounds => Box.ToRectangle() + Pivot;

        public CollisionBoxOutline(Vector2D pivot, CollisionBox bounds, Color color) : this(pivot, bounds, color, 2)
        {

        }

        public CollisionBoxOutline(Vector2D pivot, CollisionBox bounds, Color color, int thickness)
        {
            Pivot = pivot;
            Box = bounds;
            Color = color;
            Thickness = thickness;
        }
    }

    class EntityOutline : IOutline
    {
        public long Id;
        public Scene Scene;

        public Rectangle Bounds => Id == 0 ? null : EditorUtil.GetSelectionBounds(Scene.Entities[Id]);
        public Color Color => Color.White;
        public int Thickness => 2;

        public EntityOutline(Scene scene, long id)
        {
            Scene = scene;
            Id = id;
        }
    }
}
