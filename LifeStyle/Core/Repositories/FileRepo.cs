using NPoco;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;
using File = Core.Models.File;

namespace Core.Repositories
{
    public class FileRepo
    {
        public File Get(int id)
        {
            var cmd = Sql.Builder.Select(@"p.*, (select Concat(u.FirstName, ' ',u.LastName) from Users u where u.Id = p.AddedBy) as CreatedName").From("Files p");
            cmd.Where("p.Id=@0", id);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<File>(cmd) ?? new File();
                return result;
            }
        }

        public List<File> GetByIds(string ids)
        {
            var cmd = Sql.Builder.Select(@"p.*, (select Concat(u.FirstName, ' ',u.LastName) from Users u where u.Id = p.AddedBy) as CreatedName").From("Files p");
            cmd.Where($"p.Id in ({ids})");
            using (var db = Utility.Database)
            {
                var result = db.Fetch<File>(cmd) ?? new List<File>();
                return result;
            }
        }

        public Page<File> GetAll(int pageNumber, int pageSize, string query)
        {
            var cmd = Sql.Builder.Select(@"p.*, (select Concat(u.FirstName, ' ',u.LastName) from Users u where u.Id = p.AddedBy) as CreatedName").From("Files p");

            if (!string.IsNullOrWhiteSpace(query))
                cmd.Where("p.FileName like @0", $"%{query}%");
            cmd.OrderBy("p.AddedOn desc");
            using (var db = Utility.Database)
            {
                var result = db.Page<File>(pageNumber, pageSize, cmd);
                return result;
            }
        }

        public File Save(File file, int userId)
        {
            using (var db = Utility.Database)
            {
                if (file.Id <= 0)
                {
                    file.AddedBy = userId;
                    file.AddedOn = DateTime.UtcNow;
                }
                else
                {
                    file.ModifiedBy = userId;
                    file.ModifiedOn = DateTime.UtcNow;
                }
                db.Save(file);
                return file;
            }
        }

        public int Delete(int id)
        {
            using (var db = Utility.Database)
            {
                var result = db.Delete<File>(id);
                return Convert.ToInt32(result);
            }
        }

    }
}

