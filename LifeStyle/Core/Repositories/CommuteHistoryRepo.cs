using Core.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class CommuteHistoryRepo
    {
        public CommuteHistory Get(int id)
        {
            var cmd = Sql.Builder.Select("s.*, Concat(u.FirstName, ' ', u.LastName) as CreatedBy, dm.Value as ModeName, dt.Value as TypeName").From("CommuteHistory s");
            cmd.InnerJoin("DataTypeValues dm").On("dm.Id = s.ModeId");
            cmd.InnerJoin("DataTypeValues dt").On("dt.Id = s.TypeId");
            cmd.InnerJoin("Users u").On("u.Id = s.AddedBy");
            cmd.Where("s.Id=@0", id);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<CommuteHistory>(cmd) ?? new CommuteHistory();
                return result;
            }
        }

        public Page<CommuteHistory> GetAll(int pageNumber, int pageSize, int userId, string query = "" )
        {
            var cmd = Sql.Builder.Select("s.*, Concat(u.FirstName, ' ', u.LastName) as CreatedBy, dm.Value as ModeName, dt.Value as TypeName").From("CommuteHistory s");
            cmd.InnerJoin("DataTypeValues dm").On("dm.Id = s.ModeId");
            cmd.InnerJoin("DataTypeValues dt").On("dt.Id = s.TypeId");
            cmd.InnerJoin("Users u").On("u.Id = s.AddedBy");
            if (userId > 0)
                cmd.Where("u.Id=@0", userId);


            if (!string.IsNullOrWhiteSpace(query) && query.ToLower() != "all")
            {
                cmd.Where("dm.Value like @0 OR dt.Value like @0", $"%{query}%");
            }

            cmd.OrderBy("s.Id");
            using (var db = Utility.Database)
            {
                var result = db.Page<CommuteHistory>(pageNumber, pageSize, cmd);
                return result;
            }
        }

        public CommuteHistory Save(CommuteHistory model)
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
                var result = db.Delete<CommuteHistory>(id);
                return Convert.ToInt32(result);
            }
        }
    }
}
