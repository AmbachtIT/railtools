using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambacht.Common.Serialization;
using Ambacht.Common.Services;

namespace Ambacht.Common.Repository
{
	public class BackupService<T>(IRepository<T> repository, IJsonSerializer serializer, string path, Func<T, string> getKey) 
	{

		public async Task Backup(CancellationToken token)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			foreach (var item in repository.Query())
			{
				var json = serializer.SerializeObject(item);
				await File.WriteAllTextAsync(Path.Combine(path, $"{getKey(item)}.json"), json, token);
			}
		}

		public async Task Restore(CancellationToken token)
		{
			if (repository is IAsyncInit init)
			{
				await init.InitAsync(token);
			}

			var count = 0;
			foreach (var file in Directory.EnumerateFiles(path, "*.json"))
			{
				var id = new FileInfo(file).Name.Replace(".json", "");
				var json = await File.ReadAllTextAsync(file, token);
				var item = serializer.DeserializeObject<T>(json);
				await repository.Store(id, item, token);
				count++;
			}
		}

	}
}
