using Core.Models;
using NPoco;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class DataTypeRepo
    {
        public DataType Get(int id)
        {
            using (var db = Utility.Database)
            {
                var result = db.SingleOrDefaultById<DataType>(id) ?? new DataType();
                return result;
            }
        }

        public DataType GetByName(string name)
        {
            var cmd = Sql.Builder.Select("a.*").From("DataTypes a");
            cmd.Where("a.Name=@0", name.ToLower());
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<DataType>(cmd) ?? new DataType();
                return result;
            }
        }

        public Page<DataType> GetAll(int pageNumber, int pageSize, string query = "", int teamId =0)
        {
            var cmd = Sql.Builder.Select("a.*").From("DataTypes a");
            if (!string.IsNullOrWhiteSpace(query) && query.ToLower() != "all")
            {
                cmd.Where("a.Name like @0", $"%{query}%");
            }
            cmd.OrderBy("a.Id");
            using (var db = Utility.Database)
            {
                var result = db.Page<DataType>(pageNumber, pageSize, cmd);
                return result;
            }
        }
        public DataType Save(DataType model)
        {
            using (var db = Utility.Database)
            {
                db.Save(model);
                return model;
            }
        }

        public int Delete(int id)
        {
            using (var db = Utility.Database)
            {
                var result = db.Delete<DataType>(id);
                return Convert.ToInt32(result);
            }
        }
    }
}
