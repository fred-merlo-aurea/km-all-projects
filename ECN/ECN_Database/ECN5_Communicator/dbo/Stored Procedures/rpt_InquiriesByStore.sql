CREATE PROCEDURE [dbo].[rpt_InquiriesByStore]
(
	@GroupID int,
	@StartDate varchar(10),
	@EndDate varchar(10)--,	
--	@Days int
)
AS
Begin
	SET NOCOUNT ON
	declare @ParentGroupID int,
			@CustomerID int,
			@IssueType varchar(100)
	select @IssueType = GroupName from Groups where GroupID = @GroupID
	set @ParentGroupID = 670
	set @CustomerID = 162
	DECLARE @Inquiry TABLE (
	  	EmailID int,
		EmailAddress varchar(100),
	  	InquiryDate	datetime
	)
	
	DECLARE @EmailList TABLE (
	  	EmailID int,
		EmailAddress varchar(100)
	)
	DECLARE @Emails TABLE 
	(
		EmailID int, 
		EmailAddress varchar(100),
		StoreID int,
		StoreName varchar(100),
		Invoice_ID int,
	  	Sale_Date	datetime,
		Purchase_Amount decimal(18,2),
		Purchase_Cost decimal(18,2) 
	)
	-- Get the Inquires for the Group.
	insert into @Inquiry
	select 
			e.emailID, 
			e.emailAddress,
			case  when eg.LastChanged is null then eg.CreatedOn else eg.LastChanged end 
	From 
		emails e WITH (NOLOCK) join 
		emailgroups eg WITH (NOLOCK) on e.emailID = eg.emailID  
	where 
			eg.groupID = @GroupID and 
			convert(DateTime,case when eg.LastChanged is null then eg.CreatedOn else eg.LastChanged end) between @startDate and @endDate
--select * from @Inquiry
	
	-- check if the email existing in groupID 670
	insert into  @EmailList
	select 	Emails.EmailID, Emails.EmailAddress
	from 
			Emails WITH (NOLOCK) join 
			EmailGroups WITH (NOLOCK) on Emails.emailID = EmailGroups.emailID
	Where 
			GroupID = @ParentGroupID and 
			CustomerID = @CustomerID and 
--			Emails.EmailID in (select EmailID from @Inquiry)
			Emails.EmailAddress in (select EmailAddress from @Inquiry)
	
--select * from @EmailList
	-- Filter the Emails using the EmailList Table
	insert into @Emails
	select  InnerTable1.EmailID,
			InnerTable1.EmailAddress, 	
			max([Store_Location_ID]),
			max([Store_Location_Name]),
			convert(int,max([Invoice_ID])),
			max([Sale_Date]),
			convert(decimal(18,2),max([Item_Extend_Amt])),
			convert(decimal(18,2),max([Item_Total_Cost]))
	from 
		(   
		select 		distinct 
					Emails.EmailID, 
					Emails.EmailAddress, 
					EmailDataValues.EntryID, 
					Emails.User1, 	
					Emails.UserEvent1Date,
					case when GroupDatafields.groupDataFieldsID = 231 then EmailDataValues.DataValue else null end as [Store_Location_ID],
					case when GroupDatafields.groupDataFieldsID = 232 then EmailDataValues.DataValue else null end as [Store_Location_Name],
					case when GroupDatafields.groupDataFieldsID = 229 then EmailDataValues.DataValue else null end as [Invoice_ID],
					case when GroupDatafields.groupDataFieldsID = 230 then 
						case when IsDate(EmailDataValues.DataValue) = 1 then convert(datetime,EmailDataValues.DataValue,101) else null end else null end as [Sale_Date],
					case when GroupDatafields.groupDataFieldsID = 239 then EmailDataValues.DataValue else null end as [Item_Extend_Amt],
					case when GroupDatafields.groupDataFieldsID = 240 then EmailDataValues.DataValue else null end as [Item_Total_Cost]
			from 
				Emails WITH (NOLOCK) join  
				EmailGroups WITH (NOLOCK) on Emails.EmailID = EmailGroups.EmailID join 
				EmailDataValues WITH (NOLOCK) on Emails.EmailID = EmailDataValues.EmailID join  
				GroupDataFields WITH (NOLOCK) on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = EmailGroups.GroupID 
			where Emails.EmailID in (select EmailID from @EmailList)
		) 
		as InnerTable1  
		Group by InnerTable1.EmailID, InnerTable1.EmailAddress, InnerTable1.EntryID
		having 	
				max([Invoice_ID]) is not null and 
				max([Sale_Date]) is not null and 
				max([Item_Extend_Amt]) is not null and 
				ISNUMERIC(max([Item_Extend_Amt])) = 1 and
				ISDATE(max([Sale_Date])) = 1 --and 
				--convert(decimal(18,2), max([Item_Extend_Amt])) > 0 
--select * from @Emails
	select  @IssueType as InquiryType,
			@StartDate as StartDate,
			@EndDate as EndDate,
--			@Days as Days,
			case when E.StoreID is null then '' else E.StoreID end as StoreNo,
			case when E.StoreName is null then '' else E.StoreName end as StoreName,			
			count(distinct I.EmailID) as TotalInquiries,
			count(distinct E.Invoice_ID) as NoOfPurchase,
			case when sum(Purchase_Amount) is null then '0' else sum(Purchase_Amount) end as TotalPurchases,
			case when sum(Purchase_Amount - Purchase_Cost) is null then '0' else sum(Purchase_Amount - Purchase_Cost) end as TotalMargin			 
--	from @Emails E join @Inquiry I on E.EmailAddress *= I.EmailAddress
	from @Inquiry I LEFT OUTER JOIN @Emails E on I.EmailAddress = E.EmailAddress
--	Where 
--			Convert(datetime,Sale_Date,101) between Convert(datetime,InquiryDate,101) and dateAdd(dd, @Days, Convert(datetime,InquiryDate,101))
--			Convert(datetime,I.InquiryDate,101) between Convert(datetime,@StartDate,101) and dateAdd(dd, @Days, Convert(datetime,@EndDate,101))
	Group by StoreID, StoreName
	SET NOCOUNT OFF
End