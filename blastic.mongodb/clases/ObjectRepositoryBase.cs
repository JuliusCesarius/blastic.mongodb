using System.Collections.Generic;
//using System.Data.Objects;
//using System.Data.Objects.DataClasses;
//using blastic.patterns.extensions;
using System.Linq;
using System.Collections;
using System.Data;
using System;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using blastic.mongodb.interfaces;
using MongoDB.Bson;

namespace blastic.mongodb.clases
{
    public class ObjectRepositoryBase<T> : blastic.mongodb.interfaces.IObjectRepository<T>, IDisposable
        where T : IBson
    {
        const string CONNECTIONSTRINGNAME = "mongoDBConnection";
        const string CONNECTIONNOTFINDMESSAGE = "mongoDBConnection not found";

        #region Private methods

        private MongoDatabase _database;
        private MongoCollection<T> _collection;
        private List<T> _tempCollection;

        private MongoCollection<T> Collection
        {
            get
            {
                if (_collection == null)
                {
                    _collection = Database.GetCollection<T>(typeof(T).Name + "s");
                }
                return _collection;
            }
        }

        private List<T> TempCollection
        {
            get
            {
                if (_tempCollection == null)
                {
                    _tempCollection = new List<T>();
                }
                return _tempCollection;
            }
        }

        public MongoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    //Obtain the connection string
                    var connectionString = GetConnectionString();
                    var client = new MongoClient(connectionString);
                    var server = client.GetServer();
                    string databaseName = GetConnectionParts(connectionString)["Database"];
                    _database = (MongoDatabase)server.GetDatabase(databaseName);
                }
                return _database;
            }
        }

        private Dictionary<string, string> GetConnectionParts(string connectionString)
        {
            //TODO:Desglosar la cadena de conexión para obtener el nombre de la bd
            var parts = new Dictionary<string, string>();
            parts.Add("Database", "pawhubdb");
            return parts;
        }

        private string GetConnectionString()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[CONNECTIONSTRINGNAME];
            if (connectionString == null)
            {
                throw new MongoConnectionException(CONNECTIONNOTFINDMESSAGE);
            }
            return connectionString.ConnectionString;
        }

        #endregion

        public WriteConcernResult LastWriteConcernResult;

        public ObjectRepositoryBase(MongoDatabase database)
        {
            _database = database;
        }

        public ObjectRepositoryBase()
        {

        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Add(T Obj)
        {
            TempCollection.Add(Obj);
            return true;
        }

        public bool Insert(T Obj)
        {
            LastWriteConcernResult = Collection.Insert(Obj);
            return LastWriteConcernResult.Ok;
        }

        public bool Update(T Obj)
        {
            ////Necesito una asignación de nuevos valores para el objeto a actualizar y no los tengo debido a que es genérico
            //var query = Query<T>.EQ(x => x._id, Obj._id);
            //var entity = Collection.FindOne(query);
            //LastWriteConcernResult = _collection.Update(query, ---- );
            //return LastWriteConcernResult.Ok;
            throw new NotImplementedException();
        }
        public bool Update(ObjectId id, UpdateBuilder<T> updateBuilder)
        {
            var query = Query<T>.EQ(x => x._id, id);
            LastWriteConcernResult = Collection.Update(query, updateBuilder);
            return LastWriteConcernResult.Ok;
        }

        public IEnumerable<T> ListAll()
        {
            return Collection.FindAllAs<T>();
        }

        public bool Delete(T Obj)
        {
            var query = Query<T>.EQ(x => x._id, Obj._id);
            LastWriteConcernResult = Collection.Remove(query);
            return LastWriteConcernResult.Ok;
        }

        public T LoadById(ObjectId Id)
        {
            var query = Query<T>.EQ(x => x._id, Id);
            return Collection.FindOne(query);
        }

        public void Dispose()
        {
            //TODO:Implementar dispose
        }

        public T LoadById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
