using AgileObjects.AgileMapper.Extensions;
using StackExchange.Profiling.Internal;
using System;
using Core.Models;
using Core.RequestModel;
using LifeStyle;
using Core;

namespace LifeStyle.Services
{
    public class AppSettingService
    {
        public string GetWithDetails(string key)
        {
            Common.Log.Data($"Getting AppSetting by key..", $"key:-{key}");
            var setting = Common.Instances.AppSettingInst.GetWithDetails(0, key);
            if (setting.Id <= 0 || setting.Value.IsNullOrWhiteSpace())
                throw Constants.ErrorSetting(key);
            return setting.Value;
        }

        public bool GetBoolByKey(string key)
        {
            var obj = GetWithDetails(key);
            return Convert.ToBoolean(obj);
        }

        public AppSetting Save(AppSettingRequest req, int meId)
        {
            Common.Log.Data($"Saving AppSetting..", $"Req:-{req}, meId:-{meId}");
            var model = new AppSetting();
            if (req.Id > 0) model = Common.Instances.AppSettingInst.Get(req.Id);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            Common.Instances.AppSettingInst.Save(model);
            return model;
        }
    }
}


