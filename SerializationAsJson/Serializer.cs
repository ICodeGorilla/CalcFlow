namespace SerializationAsJson
{
    using Newtonsoft.Json;

    using SerializationContract;

    public class Serializer : ISerializer
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
                                                                               {
                                                                                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                                                                   PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                                                                                   TypeNameHandling = TypeNameHandling.All,
                                                                                   NullValueHandling = NullValueHandling.Ignore,
                                                                                   MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
                                                                               };

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(new Wrapper<T> { Value = value }, JsonSettings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<Wrapper<T>>(json, JsonSettings).Value;
        }

        public class Wrapper<T>
        {
            public T Value { get; set; }
        }
    }
}
