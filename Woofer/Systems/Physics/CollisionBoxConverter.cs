using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityComponentSystem.Components;
using EntityComponentSystem.ComponentSystems;
using EntityComponentSystem.Entities;
using EntityComponentSystem.Events;
using EntityComponentSystem.Saves.Json;
using EntityComponentSystem.Saves.Json.Converter;
using EntityComponentSystem.Saves.Json.Objects;

namespace WooferGame.Systems.Physics
{
    class CollisionBoxConverter : IJsonConverter
    {
        public Type GetWorkingType() => typeof(CollisionBox);
        public T FromJson<T>(JsonMaster json, IJsonValue value)
        {
            JsonObject obj = value as JsonObject;
            CollisionBox box = new CollisionBox();
            JsonArray bounds = obj.Get<JsonArray>("bounds");
            box.X = (bounds[0] as JsonNumber).Value;
            box.Y = (bounds[1] as JsonNumber).Value;
            box.Width = (bounds[2] as JsonNumber).Value;
            box.Height = (bounds[3] as JsonNumber).Value;

            JsonArray faces = obj.Get<JsonArray>("faces");
            box.TopFaceProperties = FaceFromJson(faces[0] as JsonObject);
            box.RightFaceProperties = FaceFromJson(faces[1] as JsonObject);
            box.BottomFaceProperties= FaceFromJson(faces[2] as JsonObject);
            box.LeftFaceProperties = FaceFromJson(faces[3] as JsonObject);
            return (T)Convert.ChangeType(box, typeof(T));
        }

        private CollisionFaceProperties FaceFromJson(JsonObject obj)
        {
            return new CollisionFaceProperties()
            {
                Enabled = obj.Get<JsonBoolean>("enabled").Value,
                Friction = obj.Get<JsonNumber>("friction").Value,
                Snap = obj.Get<JsonBoolean>("snap").Value
            };
        }

        public IJsonValue ToJson(JsonMaster json, object rawBox)
        {
            CollisionBox box = (CollisionBox)rawBox;
            JsonObject obj = new JsonObject();

            JsonArray rect = new JsonArray();
            obj.AddProperty("bounds", rect);
            rect.Add(box.X);
            rect.Add(box.Y);
            rect.Add(box.Width);
            rect.Add(box.Height);
            JsonArray faces = new JsonArray();
            obj.AddProperty("faces", faces);
            foreach(CollisionFaceProperties face in box.GetFaceProperties())
            {
                JsonObject faceObj = new JsonObject();
                faces.Add(faceObj);
                faceObj.AddProperty("enabled", face.Enabled);
                faceObj.AddProperty("friction", face.Friction);
                faceObj.AddProperty("snap", face.Snap);
            }
            return obj;
        }
    }
}
