using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ambacht.Common.Serialization
{
    public class CodeReferenceJsonConverter : JsonConverter<CodeReference>
    {
        public override CodeReference Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
	        var system = reader.ExpectString();
	        var code = reader.ExpectString();
	        reader.ExpectTokenType(JsonTokenType.EndArray);
	        return new CodeReference(system, code);
        }

        public override void Write(Utf8JsonWriter writer, CodeReference value, JsonSerializerOptions options)
        {
			writer.WriteStartArray();
			writer.WriteStringValue(value.System);
			writer.WriteStringValue(value.Code);
			writer.WriteEndArray();
		}
    }
}
