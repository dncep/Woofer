using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Interfaces.Visuals;
using EntityComponentSystem.Saves.Json;
using EntityComponentSystem.Saves.Json.Converter;
using EntityComponentSystem.Saves.Json.Converter.DefaultConverters;
using EntityComponentSystem.Saves.Json.Objects;
using EntityComponentSystem.Scenes;

namespace EntityComponentSystem.Saves
{
    public class LoadOperation
    {
        private readonly string Path;
        private List<IJsonConverter> Converters = new List<IJsonConverter>();

        public LoadOperation(string path) => Path = path;

        public void AddConverter(IJsonConverter converter)
        {
            Converters.Add(converter);
        }

        public Scene Load()
        {
            JsonMaster json = new JsonMaster();
            
            json.RegisterConverter(new ListConverter<long>());
            json.RegisterConverter(new ListConverter<int>());

            json.RegisterConverter(new NumberConverter<int>());
            json.RegisterConverter(new NumberConverter<float>());
            json.RegisterConverter(new NumberConverter<double>());
            json.RegisterConverter(new NumberConverter<long>());

            json.RegisterConverter(new StringConverter());
            json.RegisterConverter(new BooleanConverter());
            foreach (IJsonConverter converter in Converters)
            {
                json.RegisterConverter(converter);
            }

            JsonObject root = json.FromJson(File.ReadAllText(Path));

            Scene scene = new Scene();

            JsonObject sceneRoot = root.Get<JsonObject>("scene");

            scene.CurrentViewport = sceneRoot.Get<CameraView>(json, "viewport");

            foreach(IJsonValue rawEntity in sceneRoot.Get<JsonArray>("entities"))
            {
                JsonObject obj = rawEntity as JsonObject;
                Entity entity = new Entity();
                entity._id = obj.Get<long>(json, "id");
                entity._is_id_set = true;
                entity.Name = obj.Get<JsonString>("name").Value;
                entity.Active = obj.Get<JsonBoolean>("active").Value;

                foreach(KeyValuePair<string, IJsonValue> rawComponent in obj.Get<JsonObject>("components"))
                {
                    Type componentType = Component.TypeForIdentifier(rawComponent.Key);
                    Component component = json.ConvertFromValue(rawComponent.Value, componentType) as Component;
                    entity.Components.Add(component);
                }
                scene.Entities.Add(entity);
            }

            foreach(IJsonValue rawName in sceneRoot.Get<JsonArray>("systems"))
            {
                JsonString name = rawName as JsonString;
                ComponentSystem system = ComponentSystem.TypeForIdentifier(name.Value).GetConstructor(new Type[0]).Invoke(new object[0]) as ComponentSystem;
                scene.Systems.Add(system);
            }

            return scene;
        }
    }
}
