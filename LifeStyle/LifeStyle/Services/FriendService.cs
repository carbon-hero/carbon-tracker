using AgileObjects.AgileMapper.Extensions;
using Core.Models;
using Core.RequestModel;
using System.Security.Cryptography.Xml;

namespace LifeStyle.Services
{
    public class FriendService
    {
        public Friend AddFriend(FriendRequest req, int meId)
        {
            Common.Log.Data($"Saving friend..", $"Req:-{req.Id}, meId:-{meId}");
            var existingFriend = Common.Instances.FriendInst.GetByUserId(req.Id, meId);
            
            if (existingFriend.Id > 0)
                return existingFriend;

            var existingUser = Common.Instances.UserInst.GetByEmail(req.Email);
            if(existingUser.Id <= 0)
            {
                var model = req.Map().Over(new SignupRequest());
                existingUser = new UserService().Signup(model, meId);
            }

            existingFriend.FriendId = existingUser.Id;
            existingFriend.UserId= meId;
            Common.Instances.FriendInst.Save(existingFriend);
            return existingFriend;
        }
    }
}
