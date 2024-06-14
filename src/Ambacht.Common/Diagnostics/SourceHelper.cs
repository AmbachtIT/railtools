using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Diagnostics
{
    public static class SourceHelper
    {

	    public static IEnumerable<string> AllSourceFiles() =>
		    AllFiles("*.cs")
			    .Concat(AllFiles("*.razor"));

		public static IEnumerable<string> AllFiles(string pattern)
	    {
		    var path = GetSourceRoot("src");
		    foreach (var file in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories))
		    {
				if (file.Contains("\\obj\\") || file.Contains("\\bin\\")){
					continue;
				}
			    yield return file.Substring(path.Length + 1);
		    }
	    }

        public static string GetSourceRoot(params string[] paths)
        {
            var path = new FileInfo(typeof(SourceHelper).Assembly.Location).Directory;
            while (Directory.GetFiles(path.FullName, "*.sln").Length == 0)
            {
                path = path.Parent;
            }

            var pathParts = new[]
            {
	            path.FullName
            }.Concat(paths).ToArray();
            var dirParts = pathParts;
            if (pathParts.Last().Contains("."))
            {
	            dirParts = dirParts.Take(dirParts.Length - 1).ToArray();
            }

            var dir = Path.Combine(dirParts);
            if (!Directory.Exists(dir))
            {
				Directory.CreateDirectory(dir);
            }

            var result = Path.Combine(pathParts);
            return result;
        }



        public static Stream OpenEmbedded(Type type, string name)
        {
	        var path = $"{type.Namespace}.{name}";
	        var assembly = type.Assembly;
	        return assembly.GetManifestResourceStream(path);
        }

    }
}
