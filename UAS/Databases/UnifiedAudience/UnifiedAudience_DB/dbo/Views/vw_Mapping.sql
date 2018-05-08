CREATE VIEW dbo.vw_Mapping
AS
SELECT     dbo.Pubs.PubID, dbo.Pubs.PubCode, dbo.ResponseGroups.ResponseGroupID, dbo.ResponseGroups.ResponseGroupName, 
                      dbo.ResponseGroups.DisplayName AS ResponseGroup_DisplayName, dbo.CodeSheet.CodeSheetID, dbo.CodeSheet.Responsevalue, dbo.CodeSheet.Responsedesc, 
                      dbo.Mastercodesheet.MasterID, dbo.Mastercodesheet.MasterValue, dbo.Mastercodesheet.MasterDesc, dbo.MasterGroups.MasterGroupID, dbo.MasterGroups.Name, 
                      dbo.MasterGroups.DisplayName, dbo.MasterGroups.ColumnReference
FROM         dbo.ResponseGroups INNER JOIN
                      dbo.CodeSheet ON dbo.ResponseGroups.ResponseGroupID = dbo.CodeSheet.ResponseGroupID INNER JOIN
                      dbo.Pubs ON dbo.ResponseGroups.PubID = dbo.Pubs.PubID INNER JOIN
                      dbo.CodeSheet_Mastercodesheet_Bridge ON dbo.CodeSheet.CodeSheetID = dbo.CodeSheet_Mastercodesheet_Bridge.CodeSheetID INNER JOIN
                      dbo.Mastercodesheet ON dbo.CodeSheet_Mastercodesheet_Bridge.MasterID = dbo.Mastercodesheet.MasterID INNER JOIN
                      dbo.MasterGroups ON dbo.Mastercodesheet.MasterGroupID = dbo.MasterGroups.MasterGroupID