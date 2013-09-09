using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace blastic.mongodb.interfaces
{
    public interface IBson
    {
        ObjectId _id { get; set; }
    }
}
