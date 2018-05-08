CREATE Procedure [dbo].[spTag_MasterSupressedEmails_as_M]
as
Begin
	set nocount on
--select c.CustomerID, count(emailID) as mscount, (select COUNT(distinct emailID) from EmailGroups where SubscribeTypeCode='M' and GroupID in (select GroupID from Groups where CustomerID = c.CustomerID))
--from	[ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c join 
--		Groups g on g.customerID = c.customerID join
--		EmailGroups eg on eg.groupID = g.GroupID 
--where c.ActiveFlag='Y' and MasterSupression = '1'
--group by c.customerID 

--go
	declare @EmailID int, @i int, @dt datetime, @MasterGroupID int

	declare @MSGroupID Table (GroupID int)

	insert into @MSGroupID
	select  g.groupID
	from	[ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c join 
			Groups g on g.customerID = c.customerID
	where c.ActiveFlag='Y' and MasterSupression = '1' 

	set @dt = GETDATE()

	--select count(distinct emailID)
	--from EmailGroups eg join @MSGroupID ms  on eg.groupID = ms.GroupID and convert(date,ISNULL(LastChanged,CreatedOn)) = CONVERT(date, getdate()-1)

	set @i = 0

	DECLARE c_MSList  CURSOR FORWARD_ONLY FOR 
	select distinct emailID, ms.groupID
	from	EmailGroups eg join @MSGroupID ms  on eg.groupID = ms.GroupID and convert(date,ISNULL(LastChanged,CreatedOn)) >= CONVERT(date, getdate()-1)
	for READ ONLY
	
	OPEN c_MSList  
	FETCH NEXT FROM c_MSList INTO @EmailID, @MasterGroupID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		update EmailGroups set SubscribeTypeCode='M', LastChanged = @dt where EmailID = @EmailID and GroupID <> @MasterGroupID and SubscribeTypeCode<>'U'

		--if @i % 10000  = 0
		--	print (convert(varchar,@i) + '  /  ' + convert(varchar(20), getdate(), 114))
		
		set @i = @i + 1
		
		FETCH NEXT FROM c_MSList INTO @EmailID, @MasterGroupID
	END
	
	Print ' Total Updated : ' + convert(varchar,@i)


	CLOSE c_MSList  
	DEALLOCATE c_MSList  
End

