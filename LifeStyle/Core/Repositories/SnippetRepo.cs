using NPoco;
using StackExchange.Profiling.Internal;
using System;
using Core.Models;

namespace Core.Repositories
{
    public class SnippetRepo
    {
        public Snippet Get(int id)
        {
            var cmd = Sql.Builder.Select("*").From("Snippets");
                cmd.Where("Id=@0", id);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<Snippet>(cmd) ?? new Snippet();
                return result;
            }
        }
        public Snippet GetByKey(string key)
        {
            var cmd = Sql.Builder.Select("*").From("Snippets");
            cmd.Where("`Key`=@0", key);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<Snippet>(cmd);
                if (result == null && result.Id <= 0)
                    throw new Exception(Constants.ResponseMessage.NotFound);
                return result;
            }
        }

        public Page<Snippet> GetAll(int pageNumber, int pageSize, string query = "")
        {
            var cmd = Sql.Builder.Select("s.*,u.FirstName,u.LastName").From("Snippets s");
            cmd.InnerJoin("Users u").On("u.Id = s.AddedBy");
            if (!string.IsNullOrWhiteSpace(query) && query.ToLower() != "all")
            {
                cmd.Where("s.Key like @0", $"%{query}%");
            }

            cmd.OrderBy("Id");
            using (var db = Utility.Database)
            {
                var result = db.Page<Snippet>(pageNumber, pageSize, cmd);
                return result;
            }
        }

        public Snippet Save(Snippet model)
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
                var result = db.Delete<Snippet>(id);
                return Convert.ToInt32(result);
            }
        }
    }
}


