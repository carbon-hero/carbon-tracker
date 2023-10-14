using AgileObjects.AgileMapper.Extensions;
using System;
using Core.Models;
using Core.RequestModel;
using LifeStyle;

namespace LifeStyle.Services
{
    public class TemplateService
    {
        public Template Save(TemplateRequest req, int meId)
        {
            Common.Log.Data($"Saving template..", $"Req:-{req}, meId:-{meId}");
            var model = new Template();
            if (req.Id > 0) model = Common.Instances.TemplateInst.Get(req.Id);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            Common.Instances.TemplateInst.Save(model);
            return model;
        }
    }
}

