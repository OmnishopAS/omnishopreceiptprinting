using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Omnishop.ReceiptPrinting
{

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        protected abstract T Create(Type objectType, JObject jsonObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var target = Create(objectType, jsonObject);
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }
    }

    public class ReceiptDocumentJsonConverter : JsonCreationConverter<ReceiptDocumentLine>
    {
        protected override ReceiptDocumentLine Create(Type objectType, JObject jsonObject)
        {
            string elementType = (jsonObject["elementType"]).ToString();
            switch (elementType)
            {
                case "RDLFont":
                    return new RDLFont();
                case "RDLLineSettings":
                    return new RDLLineSettings();
                case "RDLText":
                    return new RDLText();
                default:
                    return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jobject = JObject.FromObject(value, serializer);
            jobject.AddFirst(new JProperty("elementType", value.GetType().Name));
            jobject.WriteTo(writer);
        }
    }
}
