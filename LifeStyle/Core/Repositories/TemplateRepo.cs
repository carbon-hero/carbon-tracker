using NPoco;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.Repositories
{
    public class TemplateRepo
    {
        public Template Get(int id)
        {
            using (var db = Utility.Database)
            {
                var result = db.SingleOrDefaultById<Template>(id) ?? new Template();
                return result;
            }
        }
        public Template GetByKey(string key)
        {
            var cmd = Sql.Builder.Select("*").From("Templates");
            cmd.Where("`Key`=@0", key);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<Template>(cmd);
                if (result == null)
                    throw new Exception(Constants.ResponseMessage.TemplateNotFound);
                return result;
            }
        }

        public Page<Template> GetAll(int pageNumber, int pageSize, string query = "")
        {
            var cmd = Sql.Builder.Select("s.*,u.FirstName,u.LastName").From("Templates s");
            cmd.InnerJoin("Users u").On("u.Id = s.AddedBy");

            if (!string.IsNullOrWhiteSpace(query) && query.ToLower() != "all")
            {
                cmd.Where("s.Key like @0", $"%{query}%");
            }

            cmd.OrderBy("Id");
            using (var db = Utility.Database)
            {
                var result = db.Page<Template>(pageNumber, pageSize, cmd);
                return result;
            }
        }

        public Template Save(Template model)
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
                var result = db.Delete<Template>(id);
                return Convert.ToInt32(result);
            }
        }
    }
}


