using Newtonsoft.Json;

public class AsyncStateMachineBoxConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        // Assuming `AsyncStateMachineBox` is in the full name; you can adjust this condition based on your actual scenario
        return objectType.FullName?.Contains("AsyncStateMachineBox") == true;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // Handle the conversion or ignore
        writer.WriteNull(); // Alternatively, you can handle it in a more sophisticated way
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return null; // Handle deserialization if needed
    }
}
