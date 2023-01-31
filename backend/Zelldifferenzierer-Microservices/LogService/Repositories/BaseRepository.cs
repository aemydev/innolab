using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LogService.Repositories
{

    /// <summary>
    /// A basic implementation of the repository pattern.
    /// T is the type of the entity
    /// TDb is the context class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDb"></typeparam>
        public abstract class BaseRepository<T, TDb> : IRepository<T>, IDisposable where T : class
            where TDb : DbContext
        {
            protected DbSet<T> RepoTable;
            protected TDb Context;

            protected BaseRepository(TDb context)
            {
                Context = context;
            }

            public void Dispose()
            {
                Context?.Dispose();
            }

            public void Add(T entity)
            {
                RepoTable.Add(entity);
                Context.SaveChanges();
            }

            public abstract Task Update(T entity);

            public async Task AddAsync(T entity)
            {
                await Task.Run(() => RepoTable.Add(entity));
                Context.SaveChanges();
            }

            public void AddRange(IList<T> entities)
            {
                foreach (var entity in entities)
                {
                    RepoTable.Add(entity);
                }

                Context.SaveChanges();
            }

            public async Task AddRangeAsync(IList<T> entities)
            {
                await Task.Run(() =>
                {
                    foreach (var entity in entities)
                    {
                        RepoTable.Add(entity);
                    }

                    Context.SaveChanges();
                });
            }

            public void Save()
            {
                Context.SaveChanges();
            }

            public async Task SaveAsync()
            {
                await Context.SaveChangesAsync();
            }

            //public void DeleteById(int id)
            //{
            //    RepoTable.Remove(RepoTable.Find(id) ?? throw new InvalidOperationException("Entity not found"));
            //    Context.SaveChanges();
            //}

            //public async Task DeleteByIdAsync(int id)
            //{
            //    await Task.Run(() =>
            //        RepoTable.Remove(RepoTable.Find(id) ?? throw new InvalidOperationException("Entity not found")));
            //    Context.SaveChanges();
            //}

            public void Delete(T entity)
            {
                RepoTable.Remove(entity);
                Context.SaveChanges();
            }

            public async Task DeleteAsync(T entity)
            {
                await Task.Run(() => RepoTable.Remove(entity));
                Context.SaveChanges();
            }

            public T GetOne(int? id)
            {
                return RepoTable.Find(id);
            }

            public async Task<T> GetOneAsync(int? id)
            {
                return await Task.Run(() => RepoTable.Find(id));
            }

            public List<T> GetAll()
            {
                return RepoTable.ToList();
            }

            public async Task<List<T>> GetAllAsync()
            {
                return await Task.Run(() => RepoTable.ToList());
            }
        }
    }
