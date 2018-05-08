using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class RuleResult
    {
        public int Save(FrameworkUAS.Entity.RuleResult x)
        {
            int ruleResultId = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                ruleResultId = DataAccess.RuleResult.Save(x);
                scope.Complete();
            }
            return ruleResultId;
        }
        public StringBuilder CreateRuleScript(int ruleId)
        {
            string ruleScript = string.Empty;
            //take ruleId and get Rule object - check CustomImportRuleId - valid types are Insert, Update, Delete, ADMS
            //66    Custom Import Rule valid types are Insert, Update, Delete, ADMS
            Rule rWrk = new Rule();
            Entity.Rule rule = rWrk.GetRule(ruleId);
            BusinessLogic.RuleCondition rcWrk = new RuleCondition();
            List<Entity.RuleCondition> ruleCondtions = rcWrk.Select(ruleId);
            StringBuilder where = LinkConditions(ruleCondtions);


            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> custImptRuleTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Custom_Import_Rule);
            FrameworkUAD_Lookup.Entity.Code custImptRuleType = custImptRuleTypes.SingleOrDefault(x => x.CodeId == rule.CustomImportRuleId);

            //get the Rule Action - need to know if it is an ALL action
            List<FrameworkUAD_Lookup.Entity.Code> ruleActionTypes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Rule_Action);
            FrameworkUAD_Lookup.Entity.Code ruleActionType = ruleActionTypes.SingleOrDefault(x => x.CodeId == rule.RuleActionId);
            
            if (custImptRuleType.CodeName.Equals(FrameworkUAD_Lookup.Enums.CustomImportRule.Delete.ToString()))
            {
                //RuleActionId -  Delete, 
                //          these will have no RuleCondtions -Delete All
                //if IsDeleteRule - get RuleConditions by RuleId  
                if (ruleActionType.CodeName.Equals(FrameworkUAD_Lookup.Enums.RuleAction.Delete_All.ToString().Replace("_", " ")))
                {
                    //no where so need to add one
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("Delete PubSubscriptions ");
                    sql.AppendLine(" where PubSubscriptionID in ");
                    sql.AppendLine(" ( ");
                    sql.AppendLine(" select ps.PubSubscriptionID ");
                    sql.AppendLine(" from PubSubscriptions ps with(nolock) ");
                    sql.AppendLine(" join Pubs p with(nolock) on ps.PubID = p.PubID ");
                    sql.AppendLine(" join SubscriberFinal sf with(nolock) on p.PubCode = sf.PubCode ");
                    sql.AppendLine(" where sf.ProcessCode = @processCode ");
                    sql.AppendLine(" )");

                    where = sql;
                }
                else
                    where.Insert(0,"Delete PubSubscriptions ");
            }
            else if (custImptRuleType.CodeName.Equals(FrameworkUAD_Lookup.Enums.CustomImportRule.Update.ToString()))
            {
                //RuleActionId - Update New, Update Exising and File, Update Exising, Update File, 
                //          these will have no RuleCondtions - Updtae Exising All, Update File All, Update All

                //if UpdateNew - by ProcessCode
                //update ps
                //set Field1 = Value1, Field2 = Value2
                //from PubSubscriptions ps
                //join SubscriberFinal sf on ps.SFRecordIdentifier = sf.SFRecordIdentifier
                //where sf.IsNewRecord = 'true' and sf.ProcessCode = @processCode and RuleConditions

                //if Update Existing and File 
                //update ps
                //set Field1 = Value1, Field2 = Value2
                //from PubSubscriptions ps
                //join SubscriberFinal sf on ps.SFRecordIdentifier = sf.SFRecordIdentifier
                //where RuleConditions

                //if  Update Existing - exclude by ProcessCode
                //update ps
                //set Field1 = Value1, Field2 = Value2
                //from PubSubscriptions ps
                //left join SubscriberFinal sf on ps.SFRecordIdentifier = sf.SFRecordIdentifier
                //where sf.ProcessCode = @processCode and sf.SFRecordIdentifier is null and RuleConditions

                //if Update File - only ProcessCode records
                //update ps
                //set Field1 = Value1, Field2 = Value2
                //from PubSubscriptions ps
                //join SubscriberFinal sf on ps.SFRecordIdentifier = sf.SFRecordIdentifier
                //where sf.ProcessCode = @processCode and  RuleConditions and RuleConditions

            }
            else if (custImptRuleType.CodeName.Equals(FrameworkUAD_Lookup.Enums.CustomImportRule.Insert.ToString()))
            {
                //RuleActionId -  Do Not Import, Import
                //PubSubscriptions ps
                //Subscriptions s
                
                //lets just return the where - not sure exactly what Sunil wants
                                
            }


            //if (ruleAction.CodeName.StartsWith("Update"))
            //{
            //    //Update PubSubscriptions.Properties (get from RuleResult) Where xxxxx(get from RuleConditions)
            //}
            //else if (ruleAction.CodeName.StartsWith("Delete"))
            //{
            //    //Delete PubSubscriptions Where xxxxxxxx(get from RuleConditions)
            //}
            //else//Insert
            //{
            //    //Where xxxxx(get from RuleConditions
            //}

            return where;
        }
        private StringBuilder LinkConditions(List<Entity.RuleCondition> ruleCondtions)
        {
            StringBuilder sb = new StringBuilder();
            if (ruleCondtions.Count > 0)
            {
                FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> opsList = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Operators);
                List<FrameworkUAD_Lookup.Entity.Code> chainList = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Rule_Chain);

                List<Entity.RuleCondition> singles = ruleCondtions.Where(x => x.IsGrouped == false).OrderBy(o => o.Line).ToList();
                List<Entity.RuleCondition> groups = ruleCondtions.Where(x => x.IsGrouped == true).OrderBy(o => o.Line).ThenBy(t => t.GroupNumber).ToList();
                sb.Append(" where ");

                if (groups.Count == 0)
                {
                    foreach (Entity.RuleCondition c in singles)
                    {
                        string opsCode = string.Empty;
                        if(c.OperatorId > 0) opsCode = opsList.Single(x => x.CodeId == c.OperatorId).CodeName;
                        //string opSymbol = Core_AMS.Utilities.LogicalOperators.GetOperators().Single(x => x.Key == opsCode).Value;
                        string chain = string.Empty;
                        if (c.ChainId > 0) chain = chainList.SingleOrDefault(x => x.CodeId == c.ChainId).CodeName;
                        //c.ChainId if 0 this is the first condition else will be AND (codeId 3557) / OR (codeId 3558)
                        sb.AppendLine(chain + " (" + c.CompareFieldPrefix + "." + c.CompareField + " " + GetOperandAndValue(opsCode, c.CompareValue) + ")");//opSymbol + " " + c.CompareValue + " ");
                    }
                }
                else
                {
                    //will need to go by line and handle group and single
                    int totalItems = ruleCondtions.Count - 1;
                    int counter = 0;
                    while (counter <= totalItems)
                    {
                        if (singles.Exists(x => x.Line == counter))
                        {
                            Entity.RuleCondition c = singles.Single(x => x.Line == counter);
                            string opsCode = string.Empty;
                            if(c.OperatorId > 0) opsCode = opsList.Single(x => x.CodeId == c.OperatorId).CodeName;
                            string chain = string.Empty;
                            if (c.ChainId > 0) chain = chainList.SingleOrDefault(x => x.CodeId == c.ChainId).CodeName;
                            sb.AppendLine(chain + " (" + c.CompareFieldPrefix + "." + c.CompareField + " " + GetOperandAndValue(opsCode, c.CompareValue) + ")");
                        }
                        else if (groups.Exists(x => x.Line == counter))
                        {
                            Entity.RuleCondition g = groups.Single(x => x.Line == counter);
                            List<Entity.RuleCondition> gClause = groups.Where(x => x.GroupNumber == g.GroupNumber && x.Line != counter).ToList();

                            string opsCode = string.Empty;
                            if(g.OperatorId > 0) opsCode = opsList.Single(x => x.CodeId == g.OperatorId).CodeName;
                            string chain = string.Empty;
                            if (g.ChainId > 0) chain = chainList.SingleOrDefault(x => x.CodeId == g.ChainId).CodeName;
                            sb.AppendLine(chain + "(( " + g.CompareFieldPrefix + "." + g.CompareField + " " + GetOperandAndValue(opsCode, g.CompareValue) + ")");

                            foreach (Entity.RuleCondition gc in gClause)
                            {
                                string gcOpsCode = string.Empty;
                                if(gc.OperatorId > 0) gcOpsCode = opsList.Single(x => x.CodeId == gc.OperatorId).CodeName;
                                string gcChain = string.Empty;
                                if (gc.ChainId > 0) gcChain = chainList.SingleOrDefault(x => x.CodeId == gc.ChainId).CodeName;
                                sb.AppendLine(gcChain + " (" + gc.CompareFieldPrefix + "." + gc.CompareField + " " + GetOperandAndValue(gcOpsCode, gc.CompareValue) + ")");

                                counter++;
                            }
                            sb.AppendLine(")");
                        }
                        counter++;
                    }
                }
            }
            return sb;
        }
        private StringBuilder testLinkConditions(List<Entity.RuleCondition> ruleCondtions)
        {
            StringBuilder sb = new StringBuilder();
            if (ruleCondtions.Count > 0)
            {
                FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                List<FrameworkUAD_Lookup.Entity.Code> opsList = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Operators);
                List<FrameworkUAD_Lookup.Entity.Code> chainList = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Rule_Chain);

                List <Entity.RuleCondition> singles = ruleCondtions.Where(x => x.IsGrouped == false).OrderBy(o => o.Line).ToList();
                List<Entity.RuleCondition> groups = ruleCondtions.Where(x => x.IsGrouped == true).OrderBy(o => o.Line).ThenBy(t => t.GroupNumber).ToList();
                sb.Append(" where ");

                if(groups.Count == 0)
                {
                    foreach (Entity.RuleCondition c in singles)
                    {
                        string opsCode = opsList.Single(x => x.CodeId == c.OperatorId).CodeName;
                        //string opSymbol = Core_AMS.Utilities.LogicalOperators.GetOperators().Single(x => x.Key == opsCode).Value;
                        string chain = string.Empty;
                        if(c.ChainId > 0) chain = chainList.SingleOrDefault(x => x.CodeId == c.ChainId).CodeName;
                        //c.ChainId if 0 this is the first condition else will be AND (codeId 3557) / OR (codeId 3558)
                        sb.AppendLine(chain + " (" + c.CompareFieldPrefix + "." + c.CompareField + " " + GetOperandAndValue(opsCode, c.CompareValue) + ")");//opSymbol + " " + c.CompareValue + " ");
                    }
                }
                else
                {
                    int sMinLine = singles.Min(x => x.Line);
                    int gMinLine = groups.Min(x => x.Line);

                    if(sMinLine < gMinLine)
                    {
                        #region Singles first then Groups
                        foreach (Entity.RuleCondition c in singles)
                        {
                            string opsCode = opsList.Single(x => x.CodeId == c.OperatorId).CodeName;
                            string chain = chainList.SingleOrDefault(x => x.CodeId == c.ChainId).CodeName;
                            sb.AppendLine(chain + " (" + c.CompareFieldPrefix + "." + c.CompareField + " " + GetOperandAndValue(opsCode, c.CompareValue) + ")");
                        }

                        List<int> groupNumbers = new List<int>();
                        foreach(var g in groups.OrderBy(x => x.GroupNumber).ToList())
                        {
                            if (!groupNumbers.Contains(g.GroupNumber))
                                groupNumbers.Add(g.GroupNumber);
                        }

                        foreach (var i in groupNumbers)
                        {
                            var currentGroup = groups.Where(x => x.GroupNumber == i).OrderBy(o => o.Line).ToList();

                            string opsCode = opsList.Single(x => x.CodeId == currentGroup.First().OperatorId).CodeName;
                            string chain = chainList.SingleOrDefault(x => x.CodeId == currentGroup.First().ChainId).CodeName;

                            sb.AppendLine(chain + " (( " + currentGroup.First().CompareFieldPrefix + "." + currentGroup.First().CompareField + " " + GetOperandAndValue(opsCode, currentGroup.First().CompareValue) + ") ");

                            foreach (Entity.RuleCondition gc in currentGroup.Skip(1))//skip first one cause done above
                            {
                                string gcOpsCode = opsList.Single(x => x.CodeId == gc.OperatorId).CodeName;
                                string gcChain = chainList.SingleOrDefault(x => x.CodeId == gc.ChainId).CodeName;
                                sb.AppendLine(gcChain + " (" + gc.CompareFieldPrefix + "." + gc.CompareField + " " + GetOperandAndValue(gcOpsCode, gc.CompareValue) + ") ");
                            }
                            sb.AppendLine(") ");

                            //bool isFirst = true;
                            ////foreach (var g in currentGroup)
                            ////{
                            //    //Entity.RuleCondition g = groups.Single(x => x.Line == counter);
                            //    //Entity.RuleCondition g = groups.Single(x => x.Line == counter);
                            //    //List<Entity.RuleCondition> gClause = groups.Where(x => x.GroupNumber == i).ToList();

                            //    if (isFirst)
                            //    {
                            //        string opsCode = opsList.Single(x => x.CodeId == currentGroup.First().OperatorId).CodeName;
                            //        string chain = chainList.SingleOrDefault(x => x.CodeId == currentGroup.First().ChainId).CodeName;
                            //        //sb.AppendLine(chain + " (( ");
                            //        sb.AppendLine(chain + " (( " + currentGroup.First().CompareFieldPrefix + "." + currentGroup.First().CompareField + " " + GetOperandAndValue(opsCode, currentGroup.First().CompareValue) + ") ");
                            //        foreach (Entity.RuleCondition gc in currentGroup.Skip(1))
                            //        {
                            //            string gcOpsCode = opsList.Single(x => x.CodeId == gc.OperatorId).CodeName;
                            //            string gcChain = chainList.SingleOrDefault(x => x.CodeId == gc.ChainId).CodeName;
                            //            sb.AppendLine(gcChain + " (" + gc.CompareFieldPrefix + "." + gc.CompareField + " " + GetOperandAndValue(gcOpsCode, gc.CompareValue) + ") ");
                            //        }
                            //        sb.AppendLine(") ");
                            //    }
                            //    else
                            //    {

                            //    }
                            //sb.AppendLine(chain + "( " + g.CompareFieldPrefix + "." + g.CompareField + " " + GetOperandAndValue(opsCode, g.CompareValue) + ")");


                            //foreach (Entity.RuleCondition gc in currentGroup)
                            //{
                            //    string gcOpsCode = opsList.Single(x => x.CodeId == gc.OperatorId).CodeName;
                            //    string gcChain = chainList.SingleOrDefault(x => x.CodeId == gc.ChainId).CodeName;
                            //    sb.AppendLine(gcChain + " (" + gc.CompareFieldPrefix + "." + gc.CompareField + " " + GetOperandAndValue(gcOpsCode, gc.CompareValue) + ") ");
                            //}

                            //sb.AppendLine(")"); //moved this to last ) on line 179 - moved first ( from line 172 to 179
                            //isFirst = false;
                            //}
                        }
                        #endregion
                    }
                    else
                    {
                        #region Groups first then singles
                        //if (groups.Exists(x => x.Line == counter))
                        //{
                        //    Entity.RuleCondition g = groups.Single(x => x.Line == counter);
                        //    List<Entity.RuleCondition> gClause = groups.Where(x => x.GroupNumber == g.GroupNumber && x.Line != counter).ToList();

                        //    string opsCode = opsList.Single(x => x.CodeId == g.OperatorId).CodeName;
                        //    string chain = chainList.SingleOrDefault(x => x.CodeId == g.ChainId).CodeName;
                        //    sb.AppendLine(chain + "( " + g.CompareFieldPrefix + "." + g.CompareField + " " + GetOperandAndValue(opsCode, g.CompareValue) + ")");


                        //    foreach (Entity.RuleCondition gc in gClause)
                        //    {
                        //        string gcOpsCode = opsList.Single(x => x.CodeId == gc.OperatorId).CodeName;
                        //        string gcChain = chainList.SingleOrDefault(x => x.CodeId == gc.ChainId).CodeName;
                        //        sb.AppendLine(gcChain + " ((" + gc.CompareFieldPrefix + "." + gc.CompareField + " " + GetOperandAndValue(gcOpsCode, gc.CompareValue) + ")) ");

                        //        counter++;
                        //    }
                        //    // sb.AppendLine(")"); moved this to last ) on line 179 - moved first ( from line 172 to 179
                        //}


                        ////////////groups
                        List<int> groupNumbers = new List<int>();
                        foreach (var g in groups.OrderBy(x => x.GroupNumber).ToList())
                        {
                            if (!groupNumbers.Contains(g.GroupNumber))
                                groupNumbers.Add(g.GroupNumber);
                        }

                        foreach (var i in groupNumbers)
                        {
                            var currentGroup = groups.Where(x => x.GroupNumber == i).OrderBy(o => o.Line).ToList();

                            string opsCode = opsList.Single(x => x.CodeId == currentGroup.First().OperatorId).CodeName;
                            string chain = chainList.SingleOrDefault(x => x.CodeId == currentGroup.First().ChainId).CodeName;

                            sb.AppendLine(chain + " (( " + currentGroup.First().CompareFieldPrefix + "." + currentGroup.First().CompareField + " " + GetOperandAndValue(opsCode, currentGroup.First().CompareValue) + ") ");

                            foreach (Entity.RuleCondition gc in currentGroup.Skip(1))//skip first one cause done above
                            {
                                string gcOpsCode = opsList.Single(x => x.CodeId == gc.OperatorId).CodeName;
                                string gcChain = chainList.SingleOrDefault(x => x.CodeId == gc.ChainId).CodeName;
                                sb.AppendLine(gcChain + " (" + gc.CompareFieldPrefix + "." + gc.CompareField + " " + GetOperandAndValue(gcOpsCode, gc.CompareValue) + ") ");
                            }
                            sb.AppendLine(") ");

                        }

                        ////singles
                        foreach (Entity.RuleCondition c in singles)
                        {
                            string opsCode = opsList.Single(x => x.CodeId == c.OperatorId).CodeName;
                            string chain = chainList.SingleOrDefault(x => x.CodeId == c.ChainId).CodeName;
                            sb.AppendLine(chain + " (" + c.CompareFieldPrefix + "." + c.CompareField + " " + GetOperandAndValue(opsCode, c.CompareValue) + ")");
                        }
                        #endregion
                    }


                    








                    ////will need to go by line and handle group and single
                    //int totalItems = ruleCondtions.Count;
                    //int counter = 0;
                    //while (counter < totalItems)
                    //{
                    //    counter++;//START HERE
                    //    if (singles.Exists(x => x.Line == counter))
                    //    {
                    //        Entity.RuleCondition c = singles.Single(x => x.Line == counter);
                    //        string opsCode = opsList.Single(x => x.CodeId == c.OperatorId).CodeName;
                    //        string chain = chainList.SingleOrDefault(x => x.CodeId == c.ChainId).CodeName;
                    //        sb.AppendLine(chain + " (" + c.CompareFieldPrefix + "." + c.CompareField + " " + GetOperandAndValue(opsCode, c.CompareValue) + ")");
                    //    }

                    //    if (groups.Exists(x => x.Line == counter))
                    //    {
                    //        Entity.RuleCondition g = groups.Single(x => x.Line == counter);
                    //        List<Entity.RuleCondition> gClause = groups.Where(x => x.GroupNumber == g.GroupNumber && x.Line != counter).ToList();

                    //        string opsCode = opsList.Single(x => x.CodeId == g.OperatorId).CodeName;
                    //        string chain = chainList.SingleOrDefault(x => x.CodeId == g.ChainId).CodeName;
                    //        sb.AppendLine(chain + "( " + g.CompareFieldPrefix + "." + g.CompareField + " " + GetOperandAndValue(opsCode, g.CompareValue) + ")");


                    //        foreach (Entity.RuleCondition gc in gClause)
                    //        {
                    //            string gcOpsCode = opsList.Single(x => x.CodeId == gc.OperatorId).CodeName;
                    //            string gcChain = chainList.SingleOrDefault(x => x.CodeId == gc.ChainId).CodeName;
                    //            sb.AppendLine(gcChain + " ((" + gc.CompareFieldPrefix + "." + gc.CompareField + " " + GetOperandAndValue(gcOpsCode, gc.CompareValue) + ")) ");

                    //            counter++;
                    //        }
                    //       // sb.AppendLine(")"); moved this to last ) on line 179 - moved first ( from line 172 to 179
                    //    }
                    //}
                }
            }
            return sb;
        }
        private string GetOperandAndValue(string opsCode, string value)
        {
            if (string.IsNullOrEmpty(opsCode))
                opsCode = "equal";

            string operand = string.Empty;
            switch (opsCode)
            {
                case "multiply":
                    operand = " * " + value;
                    break;
                case "divide":
                    operand = " / " + value;
                    break;
                case "modulus":
                    operand = " % " + value;
                    break;
                case "add":
                    operand = " + " + value;
                    break;
                case "subtract":
                    operand = " - " + value;
                    break;

                case "greater than":
                    operand = " > '" + value + "'";
                    break;
                case "less than":
                    operand = " < '" + value + "'";
                    break;
                case "greater than or equal to":
                    operand = " >= '" + value + "'";
                    break;
                case "less than or equal to":
                    operand = " <= '" + value + "'";
                    break;
                case "is not less than":
                    operand = " !< '" + value + "'";
                    break;
                case "is not greater than":
                    operand = " !> '" + value + "'";
                    break;
                case "is":
                    operand = " = '" + value + "'";
                    break;
                case "equal":
                    operand = " = '" + value + "'";
                    break;
                case "not equal":
                    operand = " != '" + value + "'";
                    break;
                case "contains":
                    operand = " like '%" + value + "%'";
                    break;
                case "starts with":
                    operand = " like '" + value + "%'";
                    break;
                case "ends with":
                    operand = " like '%" + value + "'";
                    break;
                case "in":
                    if (value.Contains(','))
                    {
                        operand = " in (";
                        string[] values = value.Split(',');
                        foreach (string s in values)
                        {
                            operand += "'" + s.Trim() + "',";
                        }
                        operand = operand.TrimEnd(',');
                        operand += ")";
                    }
                    else
                        operand = " in ('" + value.Trim() + "')";
                    break;
                case "not in":
                    if (value.Contains(','))
                    {
                        operand = " not in (";
                        string[] values = value.Split(',');
                        foreach (string s in values)
                        {
                            operand += "'" + s.Trim() + "',";
                        }
                        operand = operand.TrimEnd(',');

                        operand += ")";
                    }
                    else
                        operand = " not in ('" + value.Trim() + "')";
                    break;
            }
            return operand;
        }
    }
}
