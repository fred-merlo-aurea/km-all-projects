CREATE proc [dbo].[spGC_DataMart_Export_Details_CSV]
(
@startDate datetime = null,
@endDate datetime = null
)
as
Begin
	declare  @currentmonthstartdate datetime

set nocount on

if (@startDate is null and @endDate is null)
Begin
	select  @currentmonthstartdate = convert(varchar(10),DATEADD(dd, -1 * day(getdate())+1, getdate()),101)
	select  @startDate = dateadd(mm, -1, @currentmonthstartdate)
	select  @endDate = dateadd(ss, -1, @currentmonthstartdate)
End
else
Begin
select @endDate = dateadd(ss, -1, dateadd(dd, 1, @endDate))
End


--set @startDate = '1/1/2010'
--set @endDate = '9/30/2010'

declare @blasts table (blastID int, groupID int, sendtotal int, controlgroup int)
declare @blastsActivity table (blastID int, emailID int, controlgroup int, Opened varchar(1), ClickThru varchar(1), SoftBounce varchar(1), HardBounce varchar(1), Unsubscribed varchar(1))
    
--CREATE UNIQUE CLUSTERED INDEX ba_ind on @blastsActivity (blastID,emailID) with ignore_dup_key
	
insert into @blasts
select	b.BlastID, b.groupID, sendtotal, case when bf.field5='control' then 1 else 0 end 
from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c with (nolock) join 
		[BLAST] b with (nolock) on b.customerID = c.customerID
		join Groups g on b.GroupID = g.groupID left outer join
		BlastFields bf on b.BlastID = bf.blastID
where
		BaseChannelID = 48 and 
		SendTime between @startDate and @endDate and
		c.CustomerID not in (2835, 3012, 3037) and 
		StatusCode='sent' and 
		TestBlast = 'N'
		

insert into @blastsActivity
select b.blastID, eal.emailID, controlgroup,
		max(case when actiontypecode='open' then 'Y' else 'N' end) as Opened,
		max(case when actiontypecode='click' then 'Y' else 'N' end) as ClickThru,
		max(case when actiontypecode='bounce' and ActionValue='softbounce' then 'Y' else 'N' end) as SoftBounce,
		max(case when actiontypecode='bounce' and ActionValue='hardbounce' then 'Y' else 'N' end) as HardBounce,
		max(case when actiontypecode='subscribe' then 'Y' else 'N' end) as Unsubscribed
from 
	@blasts b join
	emailactivitylog eal with (nolock) on eal.blastID = b.blastID 
group by b.blastID,eal.emailID ,controlgroup	


	
	DELETE TMP_GC_BlastDetail

								
	INSERT INTO TMP_GC_BlastDetail (OrderNum, emailID, EmailAddress, FirstName, LastName, Address1, Address2,	City, State, Zip, Cart, Control, InsertDate, 
								AddressNum, AddressName, AddressName2, HHKEY1, MasterOrder,	DoNotProcess, OrderNum2, ErrorID, FlagA, DetailID, X_Salon_F,
								 X_Fam,	Opened, Delivered, ClickThru, SoftBounce, HardBounce,Unsubscribed,SalonNum,Customer_ID)							
	select  ba.blastID as OrderNum, ba.emailID,
		    e.emailaddress as emailaddress,
			e.FirstName as FirstName,
			e.LastName as LastName,
			e.Address as Address1,
			e.Address2 as Address2,
			e.City as City,
			e.State as State,
			e.Zip as Zip,
			'' as Cart,
			controlgroup as control, 
			'' as InsertDate,
			'' as AddressNum,
			'' as AddressName,
			'' as AddressName2,
			'' as HHKEY1,
			ba.blastID as MasterOrder,
			'' as DoNotProcess,
			'' as OrderNum2,
			'' as ErrorID,
			'' as FlagA,
			'' as DetailID,
			'' as X_Salon_F,
			'' as X_Fam,
			ba.Opened,
			case when (ba.SoftBounce = 'Y' or ba.HardBounce ='Y') and Opened='N' and ClickThru='N' then 'N' else 'Y' end as Delivered,
			ba.ClickThru,
			ba.SoftBounce,
			ba.HardBounce,
			ba.Unsubscribed,
			'' as SalonNum,
			'' as Customer_ID
			from 
		@blastsActivity ba join
		emails e with (nolock) on e.emailID = ba.emailID 
	--order by b.BlastID asc)
	
	update TMP_GC_BlastDetail
	set salonNum = edv.DataValue
	from TMP_GC_BlastDetail bd with (nolock) join 
		 @blasts b on bd.OrderNum = b.blastID join
		 EmailDataValues edv with (nolock) on edv.EmailID = bd.emailID join 
		 GroupDatafields gdf with (nolock) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID and gdf.groupID = b.groupID AND gdf.ShortName = 'SALONNUMBER'
		 
	UPDATE TMP_GC_BlastDetail 
	SET Customer_ID =  e.Customer_ID 
	from TMP_GC_BlastDetail bd with (nolock) join GreatClips..Email e with (nolock) on e.SalonID = bd.salonnum  and e.EmailAddress = bd.EmailAddress 
	
	--select * from @blasts compute sum(sendtotal)
--	1. you need to add double quotes to wrap around text.
-->2. you need to drop columns: EmailAddress, UpdateDate, and EmailSource
	
	SELECT OrderNum as OrderNum
		  ,FirstName as FirstName
		  ,LastName as LastName
		  ,Address1 as Address1
		  ,Address2 as Address2
		  ,City as City
		  ,State as State
		  ,Zip as Zip
		  ,Cart as Cart
		  ,CAST(Control as CHAR(1)) as Control
		  ,'' as InsertDate
		  ,AddressNum as AddressNum
		  ,AddressName as AddressName
		  ,AddressName2 as AddressName2
		  ,HHKEY1 as HHKEY1
		  ,MasterOrder as MasterOrder
		  ,CAST(DoNotProcess as CHAR(1)) as DoNotProcess
		  ,OrderNum2 as OrderNum2
		  ,ErrorID as ErrorID
		  ,FlagA as FlagA
		  ,DetailID as DetailID
		  ,X_Salon_F as X_Salon_F
		  ,X_Fam as X_Fam
		  ,Opened as Opened
		  ,Delivered as Delivered
		  ,ClickThru as ClickThru
		  ,SoftBounce as SoftBounce
		  ,HardBounce as HardBounce
		  ,Unsubscribed as Unsubscribed
		  ,SalonNum as SalonNum
		  ,Customer_ID as Customer_ID
	FROM TMP_GC_BlastDetail WITH (NOLOCK)
	ORDER BY OrderNum
end
