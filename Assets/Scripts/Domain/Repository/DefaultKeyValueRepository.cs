﻿using System;
using CAFU.Core.Data.Entity;
using CAFU.Core.Domain.Repository;
using CAFU.KeyValueStore.Data.DataStore;

namespace CAFU.KeyValueStore.Domain.Repository
{
    public interface IKeyValueRepository : IRepository
    {
        TEntity GetEntity<TEntity>(string key) where TEntity : class, IEntity;

        TEntity GetOrCreateEntity<TEntity>(string key) where TEntity : class, IEntity, new();

        void SetEntity<TEntity>(string key, TEntity value) where TEntity : class, IEntity;

        void Save();

        void Load();
    }

    public class DefaultKeyValueRepository : IKeyValueRepository
    {
        public class Factory : DefaultRepositoryFactory<DefaultKeyValueRepository>
        {
            private string savePath;

            protected override void Initialize(DefaultKeyValueRepository instance)
            {
                base.Initialize(instance);

                var dataStore = new DefaultKeyValueDataStore();
                dataStore.Initialize(savePath);
                instance.DataStore = dataStore;
            }

            /// <summary>
            /// Please use Create(savePath)
            /// </summary>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public override DefaultKeyValueRepository Create()
            {
                throw new NotImplementedException("Please use Factory.Create(savePath)");
            }

            public DefaultKeyValueRepository Create(string path)
            {
                savePath = path;
                return base.Create();
            }
        }

        public IKeyValueDataStore DataStore { get; set; }

        public TEntity GetEntity<TEntity>(string key) where TEntity : class, IEntity
        {
            return DataStore.GetEntity<TEntity>(key);
        }

        public TEntity GetOrCreateEntity<TEntity>(string key) where TEntity : class, IEntity, new()
        {
            return GetEntity<TEntity>(key) ?? new TEntity();
        }

        public void SetEntity<TEntity>(string key, TEntity value) where TEntity : class, IEntity
        {
            DataStore.SetEntity(key, value);
        }

        public void Save()
        {
            DataStore.Save();
        }

        public void Load()
        {
            DataStore.Load();
        }
    }
}