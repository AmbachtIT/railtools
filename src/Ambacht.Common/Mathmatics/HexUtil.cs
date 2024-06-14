using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Mathmatics
{
	public static class HexUtil
	{

		public static IEnumerable<HexCell> GetFringe(IEnumerable<HexCell> cells)
		{
			var set = cells.ToHashSet();
			foreach (var candidate in set.SelectMany(c => c.Neighbours()).Distinct())
			{
				if (!set.Contains(candidate))
				{
					yield return candidate;
				}
			}
		}

		public static IEnumerable<HexVertex> GetVertices(IEnumerable<HexEdge> edges)
		{
			var isFirst = true;
			HexEdge? previousEdge = null;
			HexVertex? previousVertex = null;
			foreach (var edge in edges)
			{
				if (previousEdge.HasValue)
				{
					previousVertex = previousEdge.Value.GetCommonVertex(edge) ?? throw new InvalidOperationException();
					if (isFirst)
					{
						isFirst = false;
						yield return previousEdge.Value.GetOtherVertex(previousVertex.Value);
					}

					yield return previousVertex.Value;
				}

				previousEdge = edge;
			}

			if (previousVertex != null)
			{
				yield return previousEdge.Value.GetOtherVertex(previousVertex.Value);
			}
		}



		public static IEnumerable<(TKey, List<TItem>)> GetConnectedItems<TKey, TItem>(IEnumerable<TItem> items,
			Func<TItem, HexCell> getCell, Func<TItem, TKey> getKey)
		{
			var dict = items.ToDictionary(getCell);
			while (dict.Count > 0)
			{
				var fringe = new Queue<(HexCell, TItem)>();
				var first = dict.First();
				dict.Remove(first.Key);
				var key = getKey(first.Value);
				fringe.Enqueue((first.Key, first.Value));
				var result = new List<TItem>();

				while (fringe.Count > 0)
				{
					var (currentCell, currentItem) = fringe.Dequeue();
					result.Add(currentItem);

					foreach (var neighbourCell in currentCell.Neighbours())
					{
						if (dict.TryGetValue(neighbourCell, out var neighbourItem))
						{
							if (object.Equals(key, getKey(neighbourItem)))
							{
								dict.Remove(neighbourCell);
								fringe.Enqueue((neighbourCell, neighbourItem));
							}
						}
					}

				}

				yield return (key, result);
			}

		}

	}
}
