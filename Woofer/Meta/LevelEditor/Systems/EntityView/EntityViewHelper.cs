using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Util;
using WooferGame.Common;
using WooferGame.Meta.LevelEditor.Systems.CursorModes;
using WooferGame.Meta.LevelEditor.Systems.InputModes;
using WooferGame.Systems.Visual;

namespace WooferGame.Meta.LevelEditor.Systems.EntityView
{
    class EntityViewHelper
    {
        internal readonly EntityViewSystem Owner;
        private long Id;

        internal Dictionary<string, ComponentSummary> Components = new Dictionary<string, ComponentSummary>();
        
        public EntityViewHelper(EntityViewSystem owner) => Owner = owner;

        public void Update(long id)
        {
            Id = id;
            Components.Clear();
            Entity entity = Owner.Owner.Entities[Id];
            if (entity == null) return;
            foreach(Component component in entity.Components)
            {
                Components[component.ComponentName] = new ComponentSummary(this, component);
            }
        }
    }

    internal class ComponentSummary
    {
        public readonly EntityViewHelper Owner;
        public string ComponentName;
        public Dictionary<string, IMemberSummary> Members = new Dictionary<string, IMemberSummary>();

        public ComponentSummary(EntityViewHelper owner, Component component)
        {
            this.Owner = owner;
            ComponentName = component.ComponentName;
            
            foreach(PropertyInfo property in component.GetType().GetProperties())
            {
                if (property.DeclaringType == typeof(Component)) continue;
                if (property.GetCustomAttribute(typeof(HideInInspector)) != null) continue;
                Members[property.Name] = new PropertySummary(this, property, component);
            }

            foreach (FieldInfo field in component.GetType().GetFields())
            {
                if (field.DeclaringType == typeof(Component)) continue;
                if (field.GetCustomAttribute(typeof(HideInInspector)) != null) continue;
                Members[field.Name] = new FieldSummary(this, field, component);
            }
        }
    }

    internal interface IMemberSummary
    {
        ComponentSummary Owner { get; }
        string Name { get; }
        TextUnit Label { get; }
        bool CanSet { get; }

        InspectorEditType EditType { get; }

        Type DataType { get; }

        object GetValue();
        bool SetValue(object obj);
    }

    internal class PropertySummary : IMemberSummary
    {
        public ComponentSummary Owner { get; private set; }
        private readonly PropertyInfo property;
        private readonly Component component;

        public string Name { get; private set; }
        public bool CanSet { get; private set; }

        public InspectorEditType EditType { get; private set; }

        public Type DataType => property.PropertyType;

        public TextUnit Label => (property.PropertyType == typeof(bool)) ? new TextUnit(new Sprite("editor", new Rectangle(0, 0, 8, 8), new Rectangle((bool)GetValue() ? 8 : 0, 48, 8, 8)), Name) :
            new TextUnit(Name + ": " + GetValue());

        public PropertySummary(ComponentSummary owner, PropertyInfo property, Component component)
        {
            this.Owner = owner;
            this.property = property;
            this.component = component;

            Name = property.Name;
            CanSet = property.CanWrite;

            if (property.GetCustomAttribute(typeof(InspectorAttribute)) is InspectorAttribute attribute)
            {
                EditType = attribute.Flags;
            }
            else EditType = InspectorEditType.Default;
        }

        public object GetValue() => property.GetValue(component);

        public bool SetValue(object obj)
        {
            if(CanSet && property.PropertyType.IsAssignableFrom(obj.GetType()))
            {
                property.SetValue(component, obj);
                return true;
            }
            return false;
        }
    }

    internal class FieldSummary : IMemberSummary
    {
        public ComponentSummary Owner { get; private set; }
        private readonly FieldInfo field;
        private readonly Component component;

        public string Name { get; private set; }
        public bool CanSet { get; private set; }

        public InspectorEditType EditType { get; private set; }

        public Type DataType => field.FieldType;

        public TextUnit Label => (field.FieldType == typeof(bool)) ? new TextUnit(new Sprite("editor", new Rectangle(0, 0, 8, 8), new Rectangle((bool)GetValue() ? 8 : 0, 48, 8, 8)), Name) :
            new TextUnit(Name + ": " + GetValue());

        public FieldSummary(ComponentSummary owner, FieldInfo field, Component component)
        {
            this.Owner = owner;
            this.field = field;
            this.component = component;

            Name = field.Name;
            CanSet = !field.IsInitOnly;
            
            if (field.GetCustomAttribute(typeof(InspectorAttribute)) is InspectorAttribute attribute)
            {
                EditType = attribute.Flags;
            }
            else EditType = InspectorEditType.Default;
        }

        public object GetValue() => field.GetValue(component);

        public bool SetValue(object obj)
        {
            if (CanSet && field.FieldType.IsAssignableFrom(obj.GetType()))
            {
                field.SetValue(component, obj);
                return true;
            }
            return false;
        }
    }

    internal static class MemberEdits
    {
        public static bool TriggerEdit(this IMemberSummary member)
        {
            Type type = member.DataType;
            if (type == typeof(bool))
            {
                member.SetValue(!(bool)member.GetValue());
                return false;
            }
            else if (type == typeof(Vector2D))
            {
                if (member.EditType == InspectorEditType.Position)
                {
                    member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new MoveCursorEvent((Vector2D)member.GetValue()));
                    member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new ForceModalChangeEvent("move_cursor_mode", null));
                    member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new StartMoveModeEvent(member));
                    return true;
                }
                else return false;
            }
            else if (type == typeof(double))
            {
                member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new ForceModalChangeEvent("number_input", null));
                member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new StartNumberInputEvent((double)member.GetValue(), v => member.SetValue(v), null));
                return true;
            }
            else if (type == typeof(float))
            {
                member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new ForceModalChangeEvent("number_input", null));
                member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new StartNumberInputEvent((float)member.GetValue(), v => member.SetValue((float)v), null));
                return true;
            }
            else if (type == typeof(int))
            {
                member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new ForceModalChangeEvent("number_input", null));
                member.Owner.Owner.Owner.Owner.Events.InvokeEvent(new StartNumberInputEvent((int)member.GetValue(), v => member.SetValue((int)v), false, null));
                return true;
            }
            return false;
        }
    }
}
