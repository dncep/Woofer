using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.Entities;

namespace WooferGame.Meta.LevelEditor.Systems.EntityView
{
    class EntityViewHelper
    {
        private readonly EntityViewSystem Owner;
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
                Components[component.ComponentName] = new ComponentSummary(component);
            }
        }
    }

    internal class ComponentSummary
    {
        public string ComponentName;
        public Dictionary<string, IMemberSummary> Members = new Dictionary<string, IMemberSummary>();

        public ComponentSummary(Component component)
        {
            ComponentName = component.ComponentName;
            
            foreach(PropertyInfo property in component.GetType().GetProperties())
            {
                if (property.DeclaringType == typeof(Component)) continue;
                if (property.GetCustomAttribute(typeof(HideInInspector)) != null) continue;
                Members[property.Name] = new PropertySummary(property, component);
            }

            foreach (FieldInfo field in component.GetType().GetFields())
            {
                if (field.DeclaringType == typeof(Component)) continue;
                if (field.GetCustomAttribute(typeof(HideInInspector)) != null) continue;
                Members[field.Name] = new FieldSummary(field, component);
            }
        }
    }

    internal interface IMemberSummary
    {
        string Name { get; }
        bool CanSet { get; }

        object GetValue();
        bool TriggerEdit();
    }

    internal class PropertySummary : IMemberSummary
    {
        private readonly PropertyInfo property;
        private readonly Component component;

        public string Name { get; private set; }
        public bool CanSet { get; private set; }

        public PropertySummary(PropertyInfo property, Component component)
        {
            this.property = property;
            this.component = component;

            Name = property.Name;
            CanSet = property.CanWrite;
        }

        public object GetValue() => property.GetValue(component);

        public bool TriggerEdit()
        {
            if(property.PropertyType == typeof(bool))
            {
                property.SetValue(component, !(bool)GetValue());
            }
            return false;
        }
    }

    internal class FieldSummary : IMemberSummary
    {
        private readonly FieldInfo field;
        private readonly Component component;

        public string Name { get; private set; }
        public bool CanSet { get; private set; }

        public FieldSummary(FieldInfo field, Component component)
        {
            this.field = field;
            this.component = component;

            Name = field.Name;
            CanSet = !field.IsInitOnly;
        }

        public object GetValue() => field.GetValue(component);

        public bool TriggerEdit()
        {
            if (field.FieldType == typeof(bool))
            {
                field.SetValue(component, !(bool)GetValue());
            }
            return false;
        }
    }
}
