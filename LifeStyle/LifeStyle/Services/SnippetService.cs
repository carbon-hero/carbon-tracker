using AgileObjects.AgileMapper.Extensions;
using Microsoft.Net.Http.Headers;
using System;
using Core.Models;
using Core.RequestModel;
using LifeStyle;

namespace LifeStyle.Services
{
    public class SnippetService
    {
        public Snippet Save(SnippetRequest req, int meId)
        {
            Common.Log.Data($"Saving snippet..", $"Req:-{req}, meId:-{meId}");
            var model = new Snippet();
            if (req.Id > 0) model = Common.Instances.SnippetInst.Get(req.Id);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            Common.Instances.SnippetInst.Save(model);
            return model;
        }
    }
}

