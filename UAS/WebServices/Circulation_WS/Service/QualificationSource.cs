using Circulation_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace Circulation_WS.Service
{
    public class QualificationSource : ServiceBase, IQualificationSource
    {
        public Response<int> Save(Guid accessKey, FrameworkCirculation.Entity.QualificationSource x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core.Utilities.JsonFunctions jf = new Core.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkCirculation.Entity.QualificationSource>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "QualificationSource", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.QualificationSource worker = new FrameworkCirculation.BusinessLogic.QualificationSource();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<List<FrameworkCirculation.Entity.QualificationSource>> Select(Guid accessKey)
        {
            Response<List<FrameworkCirculation.Entity.QualificationSource>> response = new Response<List<FrameworkCirculation.Entity.QualificationSource>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "QualificationSource", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkCirculation.BusinessLogic.QualificationSource worker = new FrameworkCirculation.BusinessLogic.QualificationSource();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
