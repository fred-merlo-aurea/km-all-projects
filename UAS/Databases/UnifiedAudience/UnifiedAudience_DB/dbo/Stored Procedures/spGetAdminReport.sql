CREATE PROCEDURE [dbo].[spGetAdminReport] 
 @OrderBy varchar(255)  
AS
BEGIN

	SET NOCOUNT ON    
	
	if (@OrderBy ='PRODUCT')
		Begin
			select p.pubID as "ProductID", 
				STUFF((SELECT ', ' + cast(pg2.GroupID as varchar(10))
						FROM PubGroups pg2
						WHERE pg2.PubID = pg.PubID
						FOR XML PATH('')),1,1,'') AS ProductGroups, 			
				p.PubCode as "ProductCode", p.score as "Score", p.PubName as "ProductName", rg.ResponseGroupName as "FOXColumnName", cs.Responsevalue as "CodeSheetValue", 
				cs.Responsedesc as "CodeSheetDesc", mg.Name as "MasterGroup", mg.MasterGroupID as "MasterGroupID", mcs.MasterValue as "MasterCodeSheetValue", 
				mcs.MasterDesc as "MasterCodeSheetDesc"
			from Pubs p
				left outer join ResponseGroups rg on p.PubID = rg.PubID
				left outer join CodeSheet cs on rg.ResponseGroupID = cs.ResponseGroupID
				left outer join CodeSheet_Mastercodesheet_Bridge cmb on cs.CodeSheetID = cmb.CodeSheetID
				left outer join Mastercodesheet mcs on cmb.MasterID = mcs.MasterID
				left outer join MasterGroups mg on mcs.MasterGroupID = mg.MasterGroupID
				left outer join PubGroups pg on p.PubID = pg.PubID
			order by ProductID, cs.ResponseGroup, CodeSheetValue
		End
	Else
		Begin
			select p.pubID as "ProductID", 
				STUFF((SELECT ', ' + cast(pg2.GroupID as varchar(10))
						FROM PubGroups pg2
						WHERE pg2.PubID = pg.PubID
						FOR XML PATH('')),1,1,'') AS ProductGroups, 
				p.PubCode as "ProductCode", p.score as "Score", p.PubName as "ProductName", rg.ResponseGroupName as "FOXColumnName", cs.Responsevalue as "CodeSheetValue", 
				cs.Responsedesc as "CodeSheetDesc", mg.Name as "MasterGroup", mg.MasterGroupID as "MasterGroupID", mcs.MasterValue as "MasterCodeSheetValue", 
				mcs.MasterDesc as "MasterCodeSheetDesc"
			from Pubs p
				left outer join ResponseGroups rg on p.PubID = rg.PubID
				left outer join CodeSheet cs on rg.ResponseGroupID = cs.ResponseGroupID
				left outer join CodeSheet_Mastercodesheet_Bridge cmb on cs.CodeSheetID = cmb.CodeSheetID
				left outer join Mastercodesheet mcs on cmb.MasterID = mcs.MasterID
				left outer join MasterGroups mg on mcs.MasterGroupID = mg.MasterGroupID
				left outer join PubGroups pg on p.PubID = pg.PubID
			order by MasterGroup, MasterCodeSheetValue, ProductName, cs.ResponseGroup						
		End		
END