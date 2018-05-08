CREATE proc [dbo].[spGC_DataMart_Export_Details]
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


declare @blasts table (blastID int, groupID int, sendtotal int)
declare @blastsActivity table (blastID int, emailID int, Opened varchar(1), ClickThru varchar(1), SoftBounce varchar(1), HardBounce varchar(1), Unsubscribed varchar(1))
    
--CREATE UNIQUE CLUSTERED INDEX ba_ind on @blastsActivity (blastID,emailID) with ignore_dup_key
	
insert into @blasts
select	b.BlastID, b.groupID, sendtotal 
from	[ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c with (nolock) join 
		[BLAST] b with (nolock) on b.customerID = c.customerID
		join Groups g on b.GroupID = g.groupID
where
		BaseChannelID = 48 and 
		SendTime between @startDate and @endDate and
		c.CustomerID not in (2835, 3012) and 
		StatusCode='sent' and 
		TestBlast = 'N'				

insert into @blastsActivity
select b.blastID, eal.emailID,
		max(case when actiontypecode='open' then 'Y' else 'N' end) as Opened,
		max(case when actiontypecode='click' then 'Y' else 'N' end) as ClickThru,
		max(case when actiontypecode='bounce' and ActionValue='softbounce' then 'Y' else 'N' end) as SoftBounce,
		max(case when actiontypecode='bounce' and ActionValue='hardbounce' then 'Y' else 'N' end) as HardBounce,
		max(case when actiontypecode='subscribe' then 'Y' else 'N' end) as Unsubscribed
from 
	@blasts b join
	emailactivitylog eal on eal.blastID = b.blastID 
group by b.blastID,eal.emailID 		


	
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
			'0' as control, --?? 0 if test
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
		emails e on e.emailID = ba.emailID 
	--order by b.BlastID asc)
	
	update TMP_GC_BlastDetail
	set salonNum = edv.DataValue
	from TMP_GC_BlastDetail bd join 
		 @blasts b on bd.OrderNum = b.blastID join
		 EmailDataValues edv on edv.EmailID = bd.emailID join 
		 GroupDatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID and gdf.groupID = b.groupID AND gdf.ShortName = 'SALONNUMBER'
		 
	UPDATE TMP_GC_BlastDetail 
	SET Customer_ID =  e.Customer_ID 
	from TMP_GC_BlastDetail bd join GreatClips..Email e on e.SalonID = bd.salonnum  and e.EmailAddress = bd.EmailAddress 
		
	--select * from @blasts compute sum(sendtotal)	
	SELECT OrderNum, FirstName, LastName, Address1, Address2,	City, State, Zip, Cart, Control, '' as InsertDate, 
								AddressNum, AddressName, AddressName2, HHKEY1, MasterOrder,	DoNotProcess, OrderNum2, ErrorID, FlagA, DetailID, X_Salon_F,
								 X_Fam,	Opened, Delivered, ClickThru, SoftBounce, HardBounce,Unsubscribed,SalonNum,Customer_ID
	FROM TMP_GC_BlastDetail
	ORDER BY OrderNum
	--FOR XML RAW ( 'Blast' ),
	--ROOT ('BlastDetail'),
	--ELEMENTS;	

end
