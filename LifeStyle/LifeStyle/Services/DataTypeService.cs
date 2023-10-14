using AgileObjects.AgileMapper.Extensions;
using Core;
using Core.Models;
using Core.RequestModel;

namespace LifeStyle.Services
{
    public class DataTypeService
    {
        public DataType SaveDataType(DataTypeRequest req, int meId)
        {
            Common.Log.Data($"Saving data type..", $"Req:-{req}, meId:-{meId}");
            var model = new DataType();
            if (req.Id > 0) model = Common.Instances.DataTypeInst.Get(req.Id);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                var existingRecord = Common.Instances.DataTypeInst.GetByName(req.Name);
                if (existingRecord.Id > 0)
                    throw Constants.ErrorAlready(req.Name);

                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            Common.Instances.DataTypeInst.Save(model);
            return model;
        }

        public DataTypeValue SaveDataTypeValue(DataTypeValueRequest req, int meId)
        {
            Common.Log.Data($"Saving data type value..", $"Req:-{req}, meId:-{meId}");
            var model = new DataTypeValue();
            if (req.Id > 0) model = Common.Instances.DataTypeValuesInst.Get(req.Id);
            model = req.Map().Over(model);
            if (model.Id <= 0)
            {
                model.AddedOn = DateTime.UtcNow;
                model.AddedBy = meId;
            }
            model.ModifiedOn = DateTime.UtcNow;
            model.ModifiedBy = meId;
            Common.Instances.DataTypeValuesInst.Save(model);
            return model;
        }
    }
}
