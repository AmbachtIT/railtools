using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Serialization
{
    public interface IJsonSerializer
    {

        string SerializeObject(object obj);

        T DeserializeObject<T>(string json);

    }


    public static class JsonSerializerExtensions
    {

	    public static async Task SerializeObject(this IJsonSerializer serializer, object obj, string path)
	    {
			var json = serializer.SerializeObject(obj);
			await File.WriteAllTextAsync(path, json);
	    }

    }
}
