using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Saves.Json;
using EntityComponentSystem.Saves.Json.Converter;
using EntityComponentSystem.Saves.Json.Converter.DefaultConverters;
using EntityComponentSystem.Saves.Json.Objects;
using EntityComponentSystem.Scenes;
using EntityComponentSystem.Util;

namespace EntityComponentSystem.Saves
{
    public class SaveOperation
    {
        private readonly Scene Scene;
        private List<IJsonConverter> Converters = new List<IJsonConverter>();

        public SaveOperation(Scene scene) => Scene = scene;

        public void AddConverter(IJsonConverter converter)
        {
            Converters.Add(converter);
        }

        public void Save(string path)
        {
            JsonMaster json = new JsonMaster();
            
            json.RegisterConverter(new ListConverter<long>());
            json.RegisterConverter(new ListConverter<int>());
            foreach (IJsonConverter converter in Converters)
            {
                json.RegisterConverter(converter);
            }

            JsonObject root = new JsonObject();

            JsonObject sceneRoot = new JsonObject();
            root.AddProperty("scene", sceneRoot);

            sceneRoot.AddProperty("viewport", Scene.CurrentViewport);
            
            {
                JsonArray entities = new JsonArray();
                sceneRoot.AddProperty("entities", entities);
                foreach(Entity entity in Scene.Entities)
                {
                    JsonObject entityObj = new JsonObject();
                    entities.Add(entityObj);
                    entityObj.AddProperty("name", entity.Name);
                    entityObj.AddProperty("id", entity.Id);
                    JsonObject components = new JsonObject();
                    entityObj.AddProperty("active", entity.Active);
                    entityObj.AddProperty("components", components);
                    foreach (Component component in entity.Components)
                    {
                        components.AddProperty(component.ComponentName, component);
                    }
                }
            }
            
            {
                JsonArray systems = new JsonArray();
                sceneRoot.AddProperty("systems", systems);
                foreach(ComponentSystem system in Scene.Systems)
                {
                    systems.Add(system.SystemName);
                }
            }

            File.WriteAllText(path, json.ToJson(root));
            //Console.WriteLine(json.ToJson(root));
        }
    }
}
