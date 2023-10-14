using NPoco;
using StackExchange.Profiling.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.Repositories
{
    public class UserRepo
    {
        public User Login(string email = "", string password = "")
        {
            var cmd = Sql.Builder.Select($@"u.*, f.FileUrl").From("Users u");
            cmd.LeftJoin("Files f").On("f.Id=u.ProfilePicId");
            cmd.Where("u.Email=@0 AND u.Password=@1", email, password);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<User>(cmd) ?? new User();
                return result;
            }
        }

        public User GetByEmail(string email = "")
        {
            var cmd = Sql.Builder.Select($@"u.*, f.FileUrl").From("Users u");
            cmd.LeftJoin("Files f").On("f.Id=u.ProfilePicId");
            cmd.Where("u.Email=@0", email);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<User>(cmd) ?? new User();
                return result;
            }
        }

        public User Get(int id)
        {
            var cmd = Sql.Builder.Select($@"u.*, f.FileUrl").From("Users u");
            cmd.LeftJoin("Files f").On("f.Id=u.ProfilePicId");
            cmd.Where("u.Id=@0",id);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<User>(cmd) ?? new User();
                return result;
            }
        }

        public List<User> GetUserByIds(string ids)
        {
            var cmd = Sql.Builder.Select($@"u.*, f.FileUrl").From("Users u");
            cmd.LeftJoin("Files f").On("f.Id=u.ProfilePicId");
            cmd.Where($"u.Id in ({ids})");
            using (var db = Utility.Database)
            {
                var result = db.Fetch<User>(cmd) ?? new List<User>();
                return result;
            }
        }


        public Page<User> GetAll(int pagenumber, int pagesize, string query = "", int meId = 0)
        {
            var cmd = Sql.Builder.Select($@"u.*, f.FileUrl").From("Users u");
            cmd.LeftJoin("Files f").On("f.Id=u.ProfilePicId");
            if (meId > 0)
                cmd.Where("u.AddedBy=@0", meId);

            if (!string.IsNullOrWhiteSpace(query) && query.ToLower() != "all")
            {
                cmd.Where("Lower(u.Email) like @0 OR Lower(u.FirstName) LIKE @0 OR Lower(u.LastName) like @0 ", $"%{query.ToLower()}%");
            }
                cmd.OrderBy("u.AddedOn desc");
            using (var db = Utility.Database)
            {
                var result = db.Page<User>(pagenumber, pagesize, cmd);
                return result;
            }
        }
       
        public User Save(User user)
        {
            using (var db = Utility.Database)
            {
                db.Save(user);
                return user;
            }
        }

        public int Delete(int Id)
        {
            using (var db = Utility.Database)
            {
                var result = db.Delete<User>(Id);
                return Convert.ToInt32(result);
            }
        }
    }
}

