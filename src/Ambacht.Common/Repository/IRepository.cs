﻿namespace Ambacht.Common.Repository
{
    public interface IRepository<TKey, TValue>
    {

        Task Store(TKey key, TValue item, CancellationToken token);

        Task Delete(TKey key, CancellationToken token);

        Task<TValue> Get(TKey key, CancellationToken token);

        IQueryable<TValue> Query();

        Task BulkUpsert(List<TValue> items, CancellationToken token);

        IAsyncEnumerable<Group<string>> Group(string fieldName, params string[] filters);


    }


    public interface IRepository<T> : IRepository<string, T>
    {

    }
}
