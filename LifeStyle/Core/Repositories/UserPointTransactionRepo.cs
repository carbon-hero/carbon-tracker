using AgileObjects.AgileMapper.Extensions;
using Core.Models;
using Core.RequestModel;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class UserPointTransactionRepo
    {
        public UserPointTransaction Get(int id)
        {
            var cmd = Sql.Builder.Select("*").From("UserPointTransactions");
            cmd.Where("Id=@0", id);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<UserPointTransaction>(cmd) ?? new UserPointTransaction();
                return result;
            }
        }

        public Page<UserPointTransaction> GetAll(int pageNumber, int pageSize, int userId = 0)
        {
            var cmd = Sql.Builder.Select("u.*").From("UserPointTransactions u");
            cmd.Where("u.UserId=@0", userId);
            cmd.OrderBy("u.Id DESC");
            using (var db = Utility.Database)
            {
                var result = db.Page<UserPointTransaction>(pageNumber, pageSize, cmd);
                foreach (var data in result.Items)
                    data.Balance = CheckBalance(data.UserId, data.Id).Balance;
                return result;
            }
        }
        public BalanceResponse CheckBalance(int userId, int lastEntryId)
        {
            Sql cmd;
            if (lastEntryId > 0)
            {
                cmd = Sql.Builder.Select($@"IFNULL((SELECT SUM(Points) FROM UserPointTransactions up WHERE TransactionType='Earned' AND up.UserId={userId}  and up.Id <= {lastEntryId}), 0) as Earned,
                       IFNULL((SELECT SUM(Points) FROM UserPointTransactions up WHERE TransactionType='Spent' AND up.UserId={userId} and up.Id <= {lastEntryId}), 0) AS Spent").From("UserPointTransactions");
            }
            else
            {
                cmd = Sql.Builder.Select($@"IFNULL((SELECT SUM(Points) FROM UserPointTransactions up WHERE TransactionType='Earned' AND up.UserId={userId}), 0) as Earned,
                      IFNULL((SELECT SUM(Points) FROM UserPointTransactions up WHERE TransactionType='Spent' AND up.UserId={userId} ), 0) AS Spent").From("UserPointTransactions");

            }
            cmd.Where("UserId=@0", userId);
            using (var db = Utility.Database)
            {
                var result = db.FirstOrDefault<BalanceResponse>(cmd) ?? new BalanceResponse();
                if (result != null) result.Balance = result.Earned - result.Spent;
                return result;
            }
        }


        public UserPointTransaction Points(UserPointTransactionRequest req, int meId)
        {
            var model = new UserPointTransaction();
            if (req.Id > 0) model = Get(req.Id);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            model.UserId = meId;
            Save(model);
            return model;

        }

        public UserPointTransaction Save(UserPointTransaction res)
        {
            using (var db = Utility.Database)
            {
                res.TransactionDate = DateTime.UtcNow;
                db.Save(res);
                return res;
            }
        }

        public int Delete(int id)
        {
            using (var db = Utility.Database)
            {
                var result = db.Delete<UserPointTransaction>(id);
                return Convert.ToInt32(result);
            }
        }
    }
}

