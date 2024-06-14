using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common
{
	public static class ParallelUtils
	{

		/// <summary>
		/// Can be used to suppress all parallelism, which may facilitate debugging. May seriously impede performance.
		///
		/// This will only be respected in DEBUG builds.
		/// </summary>
		public static int MaxParallelism = -1;


		/// <summary>
		/// Parallellizes production of items and yields them in the same order as the input.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="items"></param>
		/// <param name="getResult"></param>
		/// <returns></returns>
		public static IEnumerable<TResult> ParallelProduce<TItem, TResult>(this IEnumerable<TItem> items,
			Func<TItem, TResult> getResult, int maxParallelism = -1, CancellationToken token = default)
		{
			maxParallelism = GetEffectiveMaxParallelism(maxParallelism);
			if (maxParallelism == 1)
			{
				return items.Select(getResult);
			}
			
			var results = items.Select(i => new ParallelResult<TItem, TResult>()
			{
				Item = i
			}).ToList();

			Parallel.ForEach(results, new ParallelOptions()
			{
				MaxDegreeOfParallelism = maxParallelism,
				CancellationToken = token
			}, r =>
			{
				r.Result = getResult(r.Item);
			});
			return results.Select(r => r.Result);
		}


		/// <summary>
		/// Parallellizes production of items and yields them in the same order as the input.
		/// </summary>
		/// <typeparam name="TItem"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="items"></param>
		/// <param name="getResult"></param>
		/// <returns></returns>
		public static async Task<IEnumerable<TResult>> ParallelProduceAsync<TItem, TResult>(this IEnumerable<TItem> items,
			Func<TItem, ValueTask<TResult>> getResult, int maxParallelism = -1, CancellationToken token = default)
		{
			var results = items.Select(i => new ParallelResult<TItem, TResult>()
			{
				Item = i
			}).ToList();

			maxParallelism = GetEffectiveMaxParallelism(maxParallelism);
			if (maxParallelism == 1)
			{
				foreach (var result in results)
				{
					result.Result = await getResult(result.Item);
				}
			}
			else
			{
				await Parallel.ForEachAsync(results, token, async (result, _) =>
				{
					result.Result = await getResult(result.Item);
				});
			}



			return results.Select(r => r.Result);
		}

		private static int GetEffectiveMaxParallelism(int maxParallelism)
		{
#if DEBUG
			if (maxParallelism != -1)
			{
				if (MaxParallelism != -1)
				{
					maxParallelism = Math.Min(maxParallelism, MaxParallelism);
				}
			}
			else if (MaxParallelism != -1)
			{
				maxParallelism = MaxParallelism;
			}
#endif
			return maxParallelism;
		}

		private class ParallelResult<TItem, TResult>
		{
			public TItem Item { get; set; }

			public TResult Result { get; set; }
		}

	}
}
