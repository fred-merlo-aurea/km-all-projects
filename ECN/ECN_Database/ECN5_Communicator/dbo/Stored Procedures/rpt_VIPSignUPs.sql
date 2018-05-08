Create PROCEDURE [dbo].[rpt_VIPSignUPs]
(
	@Month int,
	@year int
)
as
Begin
	SET NOCOUNT ON
	
	declare	@GroupID int,
		@CustomerID int
	set @GroupID = 670
	set @CustomerID = 162
	DECLARE @InnerTable TABLE (
	  	EmailID 	int,
		EntryID		varchar(100),
		User1 	varchar(100),
	  	UserEvent1Date 	datetime,
		Sale_Date 	datetime,
		Item_Code varchar(100),
		IsWebSite bit
	)
	DECLARE @EmailList TABLE (
	  	EmailID int, 
	  	EmailDate	datetime,
		IsWebSite bit
	)
	insert into @InnerTable
	select 	distinct
			Emails.EmailID, 
			EmailDataValues.EntryID, 
			Emails.User1, 	
			Emails.UserEvent1Date,
			case when GroupDatafields.groupDataFieldsID = 230 then case when IsDate(EmailDataValues.DataValue) = 1 then convert(datetime,EmailDataValues.DataValue,101) else null end  else null end as [Sale_Date],
			case when GroupDatafields.groupDataFieldsID = 234 then EmailDataValues.DataValue else null end as [Item_Code],
			case when rtrim(ltrim(User1)) = 'VIP' AND UserEvent1Date is NOT NULL AND YEAR(UserEvent1Date) = @year then 1 else 0 end 
	from 
		Emails join  
		EmailGroups on Emails.EmailID = EmailGroups.EmailID join 
		EmailDataValues on Emails.EmailID = EmailDataValues.EmailID join  
		GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = EmailGroups.GroupID 
	where   
			EmailGroups.groupID = @GroupID and 
			Emails.CustomerID = @CustomerID and 
			EmailGroups.SubscribeTypeCode = 'S' and
			--WebSiteSignups
			(
				(
					rtrim(ltrim(User1)) = 'VIP' AND 
					UserEvent1Date is NOT NULL AND YEAR(UserEvent1Date) = @year
				) 
				OR
			--InStoreSignUps
				(
					(len(rtrim(ltrim(User1))) = 0 OR User1 is NULL) AND 
					(len(rtrim(ltrim(UserEvent1Date))) = 0 OR UserEvent1Date is NULL)
				)
			)
			
	-- Instore
	insert into @EmailList
	select  EmailID, 
			MIN(Sale_Date),
			0
	from 
		(
			select  EmailID, 	
				max([Item_Code]) as Item_Code,
				max([UserEvent1Date]) as  UserEvent1Date,
				max([Sale_Date]) as  Sale_Date
			from 
				@InnerTable
			where IsWebSite = 0
				Group by EmailID, EntryID
				having min([Sale_Date]) is not null 
		) as OuterTable
	where Item_Code = 'VIPCLUB' 
	GROUP BY EmailID
	--website 
	insert into @EmailList
	select  EmailID, 
			MIN(UserEvent1Date),
			1
	from 
		(
			select  EmailID, 	
				max([Item_Code]) as Item_Code,
				max([UserEvent1Date]) as  UserEvent1Date,
				max([Sale_Date]) as  Sale_Date
			from 
				@InnerTable
			where IsWebSite = 1
				Group by EmailID, EntryID
				having min([UserEvent1Date]) is not null 
		) as OuterTable
	where Item_Code <> 'VIPCLUB' 
	GROUP BY EmailID
	DECLARE @Emails TABLE 
	(
		EmailID int, 
		StoreID int,
		StoreName varchar(100),
		Invoice_ID int,
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
		select 		distinct 
					Emails.EmailID, 
					EmailDataValues.EntryID, 
					Emails.User1, 	
					Emails.UserEvent1Date,
					case when GroupDatafields.groupDataFieldsID = 231 then EmailDataValues.DataValue else null end as [Store_Location_ID],
					case when GroupDatafields.groupDataFieldsID = 232 then EmailDataValues.DataValue else null end as [Store_Location_Name],
					case when GroupDatafields.groupDataFieldsID = 229 then EmailDataValues.DataValue else null end as [Invoice_ID],
					case when GroupDatafields.groupDataFieldsID = 230 then case when IsDate(EmailDataValues.DataValue) = 1 then convert(datetime,EmailDataValues.DataValue,101) else null end else null end as [Sale_Date],
					case when GroupDatafields.groupDataFieldsID = 239 then EmailDataValues.DataValue else null end as [Item_Extend_Amt]
			from 
				Emails join  
				EmailGroups on Emails.EmailID = EmailGroups.EmailID join 
				EmailDataValues on Emails.EmailID = EmailDataValues.EmailID join  
				GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = EmailGroups.GroupID 
			where Emails.EmailID in (select EmailID from @EmailList)
		) 
		as InnerTable1  
		Group by InnerTable1.EmailID, InnerTable1.EntryID
		having 	
				max([Invoice_ID]) is not null and 
				max([Sale_Date]) is not null and 
				max([Item_Extend_Amt]) is not null and 
				ISNUMERIC(max([Item_Extend_Amt])) = 1 and
				max([Item_Extend_Amt]) not between CONVERT (DECIMAL(11,2), '0') and CONVERT (DECIMAL(11,2), '40') and 
				year(Convert(datetime,max([Sale_Date]),101)) = @Year and 
				Convert(datetime,max([Sale_Date]),101) > (select Convert(datetime,EmailDate,101) from @EmailList E where E.EmailID = InnerTable1.EmailID)
	select  @Month as MonthNo,
			@Year as YearID,
			E1.StoreID as StoreNo,
			E1.StoreName,
			count(distinct (case when E2.IsWebSite=1 and Month(E1.Sale_Date)=@Month then E1.Invoice_ID end)) WebQty,
			count(distinct (case when E2.IsWebSite=1 and Month(E1.Sale_Date)<=@Month then E1.Invoice_ID end)) WebQty_YTD,
			Sum(case when E2.IsWebSite=1 and Month(E1.Sale_Date)=@Month  then E1.Invoice_Amount else 0 end) WebSales,
			Sum(case when E2.IsWebSite=1 and Month(E1.Sale_Date)<=@Month  then E1.Invoice_Amount else 0 end) WebSales_YTD,
			count(distinct  case when E2.IsWebSite=0 and Month(E1.Sale_Date)=@Month then E1.Invoice_ID end) StoreQty,
			count(distinct  case when E2.IsWebSite=0 and Month(E1.Sale_Date)<=@Month then E1.Invoice_ID end) StoreQty_YTD,
			Sum(case when E2.IsWebSite=0 and Month(E1.Sale_Date)=@Month  then E1.Invoice_Amount else 0 end) StoreSales,
			Sum(case when E2.IsWebSite=0 and Month(E1.Sale_Date)<=@Month  then E1.Invoice_Amount else 0 end) StoreSales_YTD			
	from @Emails E1 join @EmailList E2 on E1.EmailID = E2.EmailID
	Where Month(Sale_Date)<=@Month and Year(Sale_Date) = @year
	Group by StoreID, StoreName
--	ORDER BY Monthno, YearID, StoreNo, StoreName
	--COMPUTE 
	--		SUM(count(distinct (case when E2.IsWebSite=1 and Month(E1.Sale_Date)=@Month then E1.Invoice_ID end))) , 
	--		SUM(count(distinct (case when E2.IsWebSite=1 and Month(E1.Sale_Date)<=@Month then E1.Invoice_ID end))), 
	--		sum(Sum(case when E2.IsWebSite=1 and Month(E1.Sale_Date)=@Month  then E1.Invoice_Amount else 0 end)), 
	--		SUM(Sum(case when E2.IsWebSite=1 and Month(E1.Sale_Date)<=@Month  then E1.Invoice_Amount else 0 end)),
	--		SUM(count(distinct  case when E2.IsWebSite=0 and Month(E1.Sale_Date)=@Month then E1.Invoice_ID end)),
	--		SUM(count(distinct  case when E2.IsWebSite=0 and Month(E1.Sale_Date)<=@Month then E1.Invoice_ID end)),
	--		SUM(Sum(case when E2.IsWebSite=0 and Month(E1.Sale_Date)=@Month  then E1.Invoice_Amount else 0 end)),
	--		SUM(Sum(case when E2.IsWebSite=0 and Month(E1.Sale_Date)<=@Month  then E1.Invoice_Amount else 0 end))
	--		BY Monthno
	SET NOCOUNT OFF
End
