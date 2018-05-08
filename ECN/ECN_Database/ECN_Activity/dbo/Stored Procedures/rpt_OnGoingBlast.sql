CREATE PROCEDURE [dbo].[rpt_OnGoingBlast]
(
	@BlastCodeID 	varchar(200),
	@StartDate 	datetime,
	@EndDate	datetime,
	@Days 	int
)
AS
Begin
	--local variables
	declare	@GroupID	int,
			@CustomerID int,
			@BlastName varchar(100)
	DECLARE @Blasts TABLE
	(
	  	BlastID int,
		BlastName varchar(100),
		GroupID int,
		send_date datetime
	)
	insert into @Blasts
	Select 	BlastID,
			EmailSubject,
			GroupID,
			convert(varchar(10), SendTime,101)
	from
			ecn5_communicator..[BLAST]
	where
			CodeID = @BlastCodeID and
			convert(datetime,convert(varchar(10),SendTime,101)) between convert(varchar(10), @StartDate,101) and convert(varchar(10), @EndDate,101)
	--select top 1 @BlastName = BlastName from @Blasts
	select top 1 @BlastName = CodeDisplay from ecn5_communicator..Code where CodeID = @BlastCodeID
	DECLARE @Emails TABLE
	(
		BlastID int,
	  	EmailID int,
		StoreID int,
		StoreName varchar(100),
	  	Sale_Date	datetime,
		Invoice_Amount decimal(18,2)
	)
	insert into @Emails
	select  InnerTable1.BlastID,
			InnerTable1.EmailID,
			max([Store_Location_ID]),
			max([Store_Location_Name]),
			max([Sale_Date]),
			max([Item_Extend_Amt])
	from
		(
			select 	B.BlastID,
					Emails.EmailID,
					EmailDataValues.EntryID,
					B.send_date,
					case when GroupDatafields.groupDataFieldsID = 231 then EmailDataValues.DataValue else null end as [Store_Location_ID],
					case when GroupDatafields.groupDataFieldsID = 232 then EmailDataValues.DataValue else null end as [Store_Location_Name],
					case when GroupDatafields.groupDataFieldsID = 230 then (case when isDate(EmailDataValues.DataValue)=1 then convert(datetime,EmailDataValues.DataValue) else null end) else null end as [Sale_Date],
					case when GroupDatafields.groupDataFieldsID = 239 then EmailDataValues.DataValue else null end as [Item_Extend_Amt]
			from
					ecn5_communicator..Emails 
					join ecn5_communicator..EmailDataValues on Emails.EmailID = EmailDataValues.EmailID 
					join ecn5_communicator..GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID 
					join @Blasts B on GroupDataFields.groupID = B.groupID
		 	where
					Emails.EmailID in (select EmailID from BlastActivitySends where BlastID = B.BlastID)
		)
		as InnerTable1
	Group by
			InnerTable1.BlastID,
			InnerTable1.EmailID,
			InnerTable1.EntryID
	having
			max([Sale_Date]) is not null and
			max([Item_Extend_Amt]) is not null
	order by
			InnerTable1.EmailID
	select 	1 as BlastID,
		@BlastName as 'BlastName',
		StoreID,
		StoreName,
		@StartDate as StartDate,
		@EndDate as EndDate,
		@Days as Days,
		Month(Sale_Date) as Sale_Month,
		sum(Invoice_Amount) as Invoice_Amount
	from
		@Blasts B join @Emails E on E.BlastID = B.BlastID
	Where
		CONVERT (DECIMAL(11,2), Invoice_Amount) not between CONVERT (DECIMAL(11,2), '0') and CONVERT (DECIMAL(11,2), '40') and
	  	convert(datetime,Sale_Date) between convert(datetime,DATEADD(dd, 1,B.send_date)) and DATEADD(dd, @Days, B.send_date)
	Group by StoreID, StoreName, Month(Sale_Date)
	order by StoreID, StoreName
/*
select E.EmailID, B.BlastID,B.send_date, StoreID, StoreName, Sale_Date, Invoice_Amount 
from @Blasts B join @Emails E on E.BlastID = B.BlastID 
Where  CONVERT (DECIMAL(11,2), Invoice_Amount) not between CONVERT (DECIMAL(11,2), '0') and CONVERT (DECIMAL(11,2), '40')
and convert(datetime,Sale_Date) between convert(datetime,DATEADD(dd, 1,B.send_date)) and DATEADD(dd, @Days, B.send_date)
order by  StoreID, EmailID
*/
End
