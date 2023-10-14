using NPoco;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.Repositories
{
    public class AppSettingRepo
    {
        public AppSetting Get(int id)
        {
            using (var db = Utility.Database)
            {
                var result = db.SingleOrDefaultById<AppSetting>(id) ?? new AppSetting();
                return result;
            }
        }

        public AppSetting GetWithDetails(int id, string key = "")
        {
            var cmd = Sql.Builder.Select("s.*,u.FirstName,u.LastName").From("AppSettings s");
            cmd.InnerJoin("Users u").On("u.Id = s.AddedBy");
            if (id > 0) cmd.Where("s.Id=@0", id);
            if (!key.IsNullOrWhiteSpace())
                cmd.Where("s.Key=@0", key);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<AppSetting>(cmd) ?? new AppSetting();
                return result;
            }
        }


        public Page<AppSetting> GetAll(int pageNumber, int pageSize, string query = "", bool isPublic = true)
        {
            var cmd = Sql.Builder.Select("s.*,u.FirstName,u.LastName").From("AppSettings s");
            cmd.InnerJoin("Users u").On("u.Id = s.AddedBy");
            if (isPublic)
                cmd.Where("s.IsPublic =@0", isPublic);
            if (!string.IsNullOrWhiteSpace(query))
            {
                cmd.Where("s.Key like @0", $"%{query}%");
            }

            cmd.OrderBy("Id");
            using (var db = Utility.Database)
            {
                var result = db.Page<AppSetting>(pageNumber, pageSize, cmd);
                return result;
            }
        }

        public AppSetting Save(AppSetting model)
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
                var result = db.Delete<AppSetting>(id);
                return Convert.ToInt32(result);
            }
        }
    }
}


