using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using blastic.patterns.interfaces;

namespace blastic.mongodb.interfaces
{
    public interface IObjectRepository<T> : IRepository<T>
    {
        T LoadById(ObjectId Id);
    }
}
