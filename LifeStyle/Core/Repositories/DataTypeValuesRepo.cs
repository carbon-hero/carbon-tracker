using Core.Models;
using NPoco;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class DataTypeValuesRepo
    {
        public DataTypeValue Get(int id)
        {
            using (var db = Utility.Database)
            {
                var result = db.SingleOrDefaultById<DataTypeValue>(id) ?? new DataTypeValue();
                return result;
            }
        }
        public List<DataTypeValue> GetAll(string typeName, string query)
        {
            var cmd = Sql.Builder.Select("dtv.*, dt.Name as DataTypeName").From("DataTypes dt");
            cmd.InnerJoin("DataTypeValues dtv").On("dtv.TypeId = dt.Id");
            if (!typeName.IsNullOrWhiteSpace())
                cmd.Where("Lower(dt.Name)=@0", typeName.ToLower());

            if (!string.IsNullOrWhiteSpace(query) && query.ToLower() != "all")
                cmd.Where("dtv.Value like @0", $"%{query}%");

            cmd.OrderBy("dtv.Id");
            using (var db = Utility.Database)
            {
                var result = db.Fetch<DataTypeValue>(cmd) ?? new List<DataTypeValue>();
                return result;
            }
        }
        public List<DataTypeValue> DatatypeSetup(string name = "")
        {
            var cmd = Sql.Builder.Select("dtv.*, dt.Name as DataTypeName").From("DataTypes dt");
            cmd.InnerJoin("DataTypeValues dtv").On("dtv.TypeId = dt.Id");
            cmd.Where("dt.Name =@1", name);
            cmd.OrderBy("dt.Id");
            using (var db = Utility.Database)
            {
                var result = db.Fetch<DataTypeValue>(cmd) ?? new List<DataTypeValue>();
                return result;
            }

        }

        public DataTypeValue GetDataTypeValue(int id, string name = "")
        {
            var cmd = Sql.Builder.Select("dtv.*, dt.Name as DataTypeName").From("DataTypes dt");
            cmd.InnerJoin("DataTypeValues dtv").On("dtv.TypeId = dt.Id");
            cmd.Where("dtv.Id=@0 and dt.Name =@1", id, name);
            cmd.OrderBy("dt.Id");
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<DataTypeValue>(cmd) ?? new DataTypeValue();
                return result;
            }

        }

        public DataTypeValue Save(DataTypeValue model)
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
                var result = db.Delete<DataTypeValue>(id);
                return Convert.ToInt32(result);
            }
        }
    }
}
