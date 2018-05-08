CREATE PROCEDURE [dbo].[rpt_InStoreSignUPs]
(
	@year int
)
as
Begin
	SET NOCOUNT ON
	
	declare	@GroupID int,
		@CustomerID int
	set @GroupID = 670
	set @CustomerID = 162
	DECLARE @Month TABLE (
	  	MonthID int IDENTITY(1,1), 
	  	MonthName	varchar(25)
	)
	insert into @Month values ('January')
	insert into @Month values ('February')
	insert into @Month values ('March')
	insert into @Month values ('April')
	insert into @Month values ('May')
	insert into @Month values ('June')
	insert into @Month values ('July')
	insert into @Month values ('August')
	insert into @Month values ('September')
	insert into @Month values ('October')
	insert into @Month values ('November')
	insert into @Month values ('Decemeber')
	
	DECLARE @VIPCLUB_EmailList TABLE (
	  	EmailID int, 
	  	Sale_Date	datetime
	)
	
	insert into @VIPCLUB_EmailList
	select  OuterTable1.EmailID, 	
			MIN(OuterTable1.Sale_Date)
	from 
	(
		select  InnerTable1.EmailID, 	
			max([Item_Code]) as Item_Code,
			max([Sale_Date]) as  Sale_Date
		from 
			(   
				select 	Distinct 
						Emails.EmailID, 
						EmailDataValues.EntryID, 
						Emails.User1, 	
						Emails.UserEvent1Date,
						case when GroupDatafields.groupDataFieldsID = 230 then case when IsDate(EmailDataValues.DataValue) = 1 then convert(datetime,EmailDataValues.DataValue,101) else null end else null end as [Sale_Date],
						case when GroupDatafields.groupDataFieldsID = 234 then EmailDataValues.DataValue else null end as [Item_Code]
				from 
					Emails WITH (NOLOCK) join  
					EmailGroups WITH (NOLOCK) on Emails.EmailID = EmailGroups.EmailID join 
					EmailDataValues WITH (NOLOCK) on Emails.EmailID = EmailDataValues.EmailID join  
					GroupDataFields WITH (NOLOCK) on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = EmailGroups.GroupID 
				where   
						EmailGroups.groupID = @GroupID and 
						Emails.CustomerID = @CustomerID and 
						EmailGroups.SubscribeTypeCode = 'S' and
						(len(rtrim(ltrim(User1))) = 0 OR User1 is NULL) AND 
						(len(rtrim(ltrim(UserEvent1Date))) = 0 OR UserEvent1Date is NULL) 
			) 
			as InnerTable1  
			Group by InnerTable1.EmailID, InnerTable1.EntryID
			having max([Sale_Date]) is not null
	)  as OuterTable1
	where OuterTable1.Item_Code = 'VIPCLUB' --and year(OuterTable1.Sale_Date) = @year
	GROUP BY OuterTable1.EmailID
	order by EmailID
	DECLARE @Emails TABLE 
	(
	  	EmailID int, 
		Invoice_ID int,
	  	Sale_Date	datetime,
		Invoice_Amount decimal(18,2)
	)
	
	insert into @Emails
	select  InnerTable1.EmailID, 	
			max([Invoice_ID]),
			max([Sale_Date]),
			max([Item_Extend_Amt])
	from 
		(   
		select 		Distinct
					Emails.EmailID, 
					EmailDataValues.EntryID, 
					Emails.User1, 	
					Emails.UserEvent1Date,
					case when GroupDatafields.groupDataFieldsID = 229 then EmailDataValues.DataValue else null end as [Invoice_ID],
					case when GroupDatafields.groupDataFieldsID = 230 then case when IsDate(EmailDataValues.DataValue) = 1 then convert(datetime,EmailDataValues.DataValue,101) else null end else null end as [Sale_Date],
					case when GroupDatafields.groupDataFieldsID = 234 then EmailDataValues.DataValue else null end as [Item_Code],
					case when GroupDatafields.groupDataFieldsID = 239 then EmailDataValues.DataValue else null end as [Item_Extend_Amt]
			from 
				Emails WITH (NOLOCK) join EmailGroups WITH (NOLOCK) on Emails.EmailID = EmailGroups.EmailID join 
				EmailDataValues WITH (NOLOCK) on Emails.EmailID = EmailDataValues.EmailID join  
				GroupDataFields WITH (NOLOCK) on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = EmailGroups.GroupID  
			where   
				Emails.EmailID in (select EmailID from @VIPCLUB_EmailList)
		) 
		as InnerTable1  
		Group by InnerTable1.EmailID, InnerTable1.EntryID
		having    
				max([Invoice_ID]) is not null and   
				max([Sale_Date]) is not null and   
				max([Item_Extend_Amt]) is not null and   
				ISNUMERIC(max([Item_Extend_Amt])) = 1 and   
				max([Item_Extend_Amt]) not between CONVERT (DECIMAL(11,2), '0') and CONVERT (DECIMAL(11,2), '40') and   
				Convert(datetime,max([Sale_Date]),101) > (select Convert(datetime,Sale_Date,101) from @VIPCLUB_EmailList E where E.EmailID = InnerTable1.EmailID) and 
				year(Convert(datetime,max([Sale_Date]),101)) = @Year  
	
	select 	@year as Year, 
			M.MonthID, 
			M.MonthName, 
			(select count(distinct EmailID) from @VIPCLUB_EmailList where Month(Sale_Date) = M.MonthID and year(Sale_Date) = @Year) as SignUps,
			InnerTable1.Quantity, 
			InnerTable1.Invoice_Amount  
	from  
	(   
		select  Month(Sale_Date) as MonthNo,  
				count(distinct Invoice_ID) as Quantity,  
				Sum(Invoice_Amount) as Invoice_Amount  
		from 
				@Emails E   
		Group by 
				Month(Sale_Date)  
	) as InnerTable1 right outer join 
	@Month M on M.MonthID = InnerTable1.MonthNo  
	SET NOCOUNT ON
End