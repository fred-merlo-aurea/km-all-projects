CREATE PROCEDURE [dbo].[MovedToActivity_rpt_OneTimeBlasts]  
(  
 @BlastID int,  
 @EndDate datetime
)  
AS  
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('rpt_OneTimeBlasts', GETDATE())
	--local variables
	declare	@GroupID	int,
			@CustomerID int,
			@StartDate datetime,
			@BlastName varchar(100)
	
	Select @BlastName = EmailSubject, @GroupID = GroupID, @CustomerID = CustomerID, @StartDate = convert(varchar(10), FinishTime,101)  from [BLAST] where BlastID = @BlastID
	
	-- Criteria
--	select @BlastID as BlastID, @GroupID as GroupID, @CustomerID as CustomerID, @StartDate as StartDate, @EndDate as EndDate
	DECLARE @Emails TABLE 
	(
	  	EmailID int, 
		StoreID int,
		StoreName varchar(100),
	  	Invoice int,
	  	Sale_Date	datetime,
		Invoice_Amount decimal(18,2)
	)
	
	insert into @Emails
	select  InnerTable1.EmailID, 	
			max([Store_Location_ID]),
			max([Store_Location_Name]),
			max([Invoice_ID]),
			max([Sale_Date]),
			max([Item_Extend_Amt])
	from 
		(   
			select Emails.EmailID, EmailDataValues.EntryID, 
				case when GroupDatafields.groupDataFieldsID = 231 then EmailDataValues.DataValue else null end as [Store_Location_ID],
				case when GroupDatafields.groupDataFieldsID = 232 then EmailDataValues.DataValue else null end as [Store_Location_Name],
				case when GroupDatafields.groupDataFieldsID = 229 then EmailDataValues.DataValue else null end as [Invoice_ID],
				case when GroupDatafields.groupDataFieldsID = 230 then convert(datetime,EmailDataValues.DataValue) else null end as [Sale_Date],
				case when GroupDatafields.groupDataFieldsID = 239 then EmailDataValues.DataValue else null end as [Item_Extend_Amt]
			from 
			Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join  
			Emails on Emails.EmailID = EmailGroups.EmailID join EmailDataValues on Emails.EmailID = EmailDataValues.EmailID join  
			GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = Groups.GroupID  
			where   
					Groups.groupID = @GroupID and Groups.CustomerID = @CustomerID and EmailGroups.SubscribeTypeCode = 'S'
	  				and Emails.EmailID in 
						(
							select EmailID from emailactivitylog join [BLAST] on emailactivitylog.BlastID = [BLAST].BlastID 
							where [BLAST].blastID = @BlastID and Actiontypecode = 'send'
						)
		) 
		as InnerTable1  
	
		Group by InnerTable1.EmailID, InnerTable1.EntryID
		having max([Invoice_ID]) is not null and max([Sale_Date]) is not null and max([Item_Extend_Amt]) is not null and 
				max([Sale_Date]) between @StartDate and @EndDate
	    
		select 	@BlastID as BlastID,
				@BlastName as BlastName,
				StoreID,
				StoreName,
				@StartDate as StartDate,
		  		count(distinct Invoice) as Invoice_Nos,
				sum(Invoice_Amount) as Invoice_Amount
		from @Emails
--		Where  Invoice_Amount > 0
		Where Invoice_Amount not between 0 and 40
		Group by StoreID, StoreName
End
