using Core.ADMS.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using FrameworkUAS.Entity;
using Core_AMS.Utilities;

namespace ADMS.Services.Feature
{
    public class AdHocDimension : ServiceBase
    {
        
        public void ProcessFile(Core.ADMS.Events.FileMoved eventMessage)
        {
            Type clientClass = Type.GetType("ADMS.ClientMethods." + eventMessage.Client.FtpFolder);

            FrameworkUAS.Entity.ClientCustomProcedure ccp = new ClientCustomProcedure();
            if (BillTurner.ClientAdditionalProperties != null && BillTurner.ClientAdditionalProperties.ContainsKey(eventMessage.Client.ClientID))
            {
                ccp = BillTurner.ClientAdditionalProperties[eventMessage.Client.ClientID].ClientCustomProceduresList.Single(x => x.ClientCustomProcedureID == eventMessage.SourceFile.ClientCustomProcedureID);
            }
            else
            {
                FrameworkUAS.BusinessLogic.ClientAdditionalProperties capWork = new FrameworkUAS.BusinessLogic.ClientAdditionalProperties();
                FrameworkUAS.Object.ClientAdditionalProperties cap = new FrameworkUAS.Object.ClientAdditionalProperties();
                cap = capWork.SetObjects(eventMessage.Client.ClientID, false);
                ccp = cap.ClientCustomProceduresList.Single(x => x.ClientCustomProcedureID == eventMessage.SourceFile.ClientCustomProcedureID);
            }
            if (FrameworkUAD_Lookup.Enums.GetProcedureType(ccp.ProcedureType) == FrameworkUAD_Lookup.Enums.ProcedureTypes.NET)
            {
                if (ccp.ProcedureName.Equals("AdHocDimensionDefault"))
                    AdHocDimensionDefault(eventMessage);
                else
                {
                    ConstructorInfo ci = clientClass.GetConstructor(Type.EmptyTypes);
                    object classObj = ci.Invoke(new object[] { });

                    MethodInfo method = clientClass.GetMethod(ccp.ProcedureName);

                    if (method != null)
                        method.Invoke(classObj, new object[] { eventMessage });
                    else
                    {
                        ConsoleMessage("Method not found: " + ccp.ProcedureName);
                    }
                }
            }
            else
            {
                //execute a sql sproc
                ADMS.ClientMethods.ClientSpecialCommon csc = new ADMS.ClientMethods.ClientSpecialCommon();
                csc.ExecuteClientSproc(ccp.ProcedureName);
            }
        }

        public void AdHocDimensionDefault(FileMoved eventMessage)
        {
            FrameworkUAS.BusinessLogic.AdHocDimension ahdData = new FrameworkUAS.BusinessLogic.AdHocDimension();
            ahdData.Delete(eventMessage.SourceFile.SourceFileID);

            Core_AMS.Utilities.FileWorker fw = new FileWorker();
            DataTable dt = fw.GetData(eventMessage.ImportFile);
            List<FrameworkUAS.Entity.AdHocDimension> list = new List<FrameworkUAS.Entity.AdHocDimension>();

            DataRow drMain = dt.Rows[0];
            string groupName = drMain["GroupName"].ToString();
            string standarField = drMain["StandardField"].ToString();
            string createdDimension = drMain["CreatedDimension"].ToString();
            string defaultValue = drMain["DefaultValue"].ToString();
            string myOperator = drMain["Operator"].ToString();

            string completeGroupName = eventMessage.Client.FtpFolder + "_" + groupName + "_" + standarField + "_" + createdDimension;

            FrameworkUAS.BusinessLogic.AdHocDimensionGroup agWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroup();
            FrameworkUAS.Entity.AdHocDimensionGroup adg = agWorker.Select(eventMessage.Client.ClientID, eventMessage.SourceFile.SourceFileID, completeGroupName, true);
            if (adg == null)
            {
                adg = new AdHocDimensionGroup();
                adg.AdHocDimensionGroupName = completeGroupName;
                adg.ClientID = eventMessage.Client.ClientID;
                adg.SourceFileID = eventMessage.SourceFile.SourceFileID;
                adg.IsActive = true;
                adg.OrderOfOperation = 1;
                adg.StandardField = standarField;
                adg.CreatedDimension = createdDimension;
                adg.DefaultValue = defaultValue;
                adg.IsPubcodeSpecific = true;
                agWorker.Save(adg);

                adg = agWorker.Select(eventMessage.Client.ClientID, eventMessage.SourceFile.SourceFileID, completeGroupName, true);
            }
            foreach (DataRow dr in dt.Rows)
            {
                FrameworkUAS.Entity.AdHocDimension ahd = new FrameworkUAS.Entity.AdHocDimension();
                ahd.AdHocDimensionGroupId = adg.AdHocDimensionGroupId;
                ahd.CreatedByUserID = 1;
                ahd.DateCreated = DateTime.Now;
                ahd.DateUpdated = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                ahd.DimensionValue = dr["DimensionValue"].ToString();
                ahd.IsActive = true;
                ahd.MatchValue = dr["MatchValue"].ToString();
                ahd.Operator = myOperator;
                ahd.UpdateUAD = true;
                ahd.UADLastUpdatedDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                if (!string.IsNullOrEmpty(ahd.MatchValue))
                    list.Add(ahd);
            }

            ahdData.SaveBulkSqlInsert(list);

            if (adg.IsPubcodeSpecific == true)
            {
                DataRow dr = dt.Rows[0];
                string[] pubCodes = dr["PubCode"].ToString().Split(',');
                List<FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap> mapList = new List<AdHocDimensionGroupPubcodeMap>();
                foreach (string s in pubCodes)
                {
                    FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap map = new AdHocDimensionGroupPubcodeMap();
                    map.AdHocDimensionGroupId = adg.AdHocDimensionGroupId;
                    map.CreatedByUserID = 1;
                    map.DateCreated = DateTime.Now;
                    map.IsActive = true;
                    map.Pubcode = s.Trim();

                    mapList.Add(map);
                }

                FrameworkUAS.BusinessLogic.AdHocDimensionGroupPubcodeMap mapWorker = new FrameworkUAS.BusinessLogic.AdHocDimensionGroupPubcodeMap();
                mapWorker.SaveBulkSqlInsert(mapList);
            }
        }
    }
}
