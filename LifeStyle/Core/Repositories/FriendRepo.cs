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
    public class FriendRepo
    {
        public Friend Get(int id)
        {
            var cmd = Sql.Builder.Select("s.*, Concat(u.FirstName, ' ', u.LastName) as CreatedBy,  (select  Concat(f.FirstName, ' ', f.LastName) from Users f where f.Id = s.FriendId) as FriendName").From("Friends s");
            cmd.InnerJoin("Users u").On("u.Id = s.UserId");
            cmd.Where("s.Id=@0", id);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<Friend>(cmd) ?? new Friend();
                return result;
            }
        }

        public Friend GetByUserId(int friendId , int meId)
        {
            var cmd = Sql.Builder.Select("s.*, Concat(u.FirstName, ' ', u.LastName) as CreatedBy,  (select  Concat(f.FirstName, ' ', f.LastName) from Users f where f.Id = s.FriendId) as FriendName").From("Friends s");
            cmd.InnerJoin("Users u").On("u.Id = s.UserId");
            cmd.Where("s.UserId=@0 and s.FriendId=@1", meId, friendId);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<Friend>(cmd) ?? new Friend();
                return result;
            }
        }

        public Page<User> GetAll(int pageNumber, int pageSize, string query = "", int meId = 0)
        {
            var cmd = Sql.Builder.Select("u.*").From("Friends s");
            cmd.InnerJoin("Users u").On("u.Id = s.FriendId");

            if(meId > 0)
                cmd.Where("s.UserId=@0", meId);

            if (!string.IsNullOrWhiteSpace(query) && query.ToLower() != "all")
                cmd.Where("f.FirstName like @0 OR f.LastName like @0", $"%{query}%");

            cmd.OrderBy("s.Id");
            using (var db = Utility.Database)
            {
                var result = db.Page<User>(pageNumber, pageSize, cmd);
                return result;
            }
        }

        public Friend Save(Friend model)
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
                var result = db.Delete<Friend>(id);
                return Convert.ToInt32(result);
            }
        }
    }
}

