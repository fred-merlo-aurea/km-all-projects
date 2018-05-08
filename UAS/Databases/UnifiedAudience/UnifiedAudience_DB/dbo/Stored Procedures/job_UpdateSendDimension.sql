CREATE PROCEDURE [job_UpdateSendDimension]
@MastergroupID int,
@Xml xml
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @docHandle int

	CREATE TABLE #import 
		(  
			SubscriptionID int, MasterID int , IsDelete bit
		) 
	CREATE INDEX EA_1 on #import (SubscriptionID)
	CREATE INDEX EA_2 on #import (IsDelete)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml;  

	With cte_Sends (EmailAddress,masterid,IsDelete)
	as
	(
		Select EmailAddress,MasterID,IsDelete
		FROM OPENXML(@docHandle, N'/XML/SendDimension')
		WITH   
			(  
				EmailAddress varchar(100) 'email', MasterID varchar(50) 'masterid', IsDelete bit  'isdelete'
			) y
	)
	Insert Into #import (SubscriptionID,MasterID,IsDelete)
	select distinct s.SubscriptionID, c.masterID, c.isdelete
	from cte_Sends c  join PubSubscriptions s on s.EMAIL = c.emailaddress
	
	EXEC sp_xml_removedocument @docHandle    
	
	if exists (select top 1 SubscriptionID from #import)
	Begin
	
		delete sd
		from SubscriptionDetails sd 
			join #import i on sd.SubscriptionID = i.SubscriptionID and sd.MasterID = i.MasterID
		where i.IsDelete = 1
	
		Insert into SubscriptionDetails (SubscriptionID, MasterID)
		select distinct i.SubscriptionID, i.MasterID from #import i 
			left outer join SubscriptionDetails sd on sd.SubscriptionID = i.SubscriptionID and sd.MasterID = i.MasterID
		where i.IsDelete = 0 and sd.sdID is null
		
	End
	
	drop table #import

	--exec Usp_MergeSubscriberMasterValuesByMasterGroupId @MastergroupID
End