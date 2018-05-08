create proc [dbo].[spGC_DataMart_Export_Details_v1]
as
Begin
	declare @startDate varchar(10)
	declare @endDate varchar(10)

	if DATEPART(m,getdate()) = 1
		set @startDate = '12/1/' + CAST(DATEPART(year,getdate()) as CHAR(4))
	else
		set @startDate = CAST(DATEPART(m,getdate())-1 as nvarchar(2)) + '/1/' + CAST(DATEPART(year,getdate()) as CHAR(4))
		
	if DATEPART(m,getdate())-1 = 4 OR DATEPART(m,getdate())-1 = 6 OR DATEPART(m,getdate())-1 = 9 OR DATEPART(m,getdate())-1 = 11
		set @endDate =CAST(DATEPART(m,getdate())-1 as nvarchar(2)) + '/30/' + CAST(DATEPART(year,getdate()) as CHAR(4))
	if DATEPART(m,getdate())-1 = 1 OR DATEPART(m,getdate())-1 = 3 OR DATEPART(m,getdate())-1 = 5 OR DATEPART(m,getdate())-1 = 7 OR DATEPART(m,getdate())-1 = 8 OR DATEPART(m,getdate())-1 = 10 OR DATEPART(m,getdate())-1 = 12
		set @endDate =CAST(DATEPART(m,getdate())-1 as nvarchar(2)) + '/31/' + CAST(DATEPART(year,getdate()) as CHAR(4))
	if DATEPART(m,getdate())-1 = 2
		set @endDate =CAST(DATEPART(m,getdate())-1 as nvarchar(2)) + '/28/' + CAST(DATEPART(year,getdate()) as CHAR(4))
		
	if @startDate is null
		set @startDate = '01/01/2010'

	if @endDate is null
		set @endDate = GETDATE()
	else
		set @endDate = @endDate + ' 23:59:59'
	
	CREATE TABLE #blastDetail (OrderNum int, EmailAddress varchar(255), LastName varchar(50), Address1 varchar(255), Address2 varchar(255),
								City varchar(50), State varchar(50), Zip varchar(50), Cart varchar(50), Control bit, InsertDate datetime, 
								AddressNum varchar(50), AddressName varchar(50), AddressName2 varchar(50), HHKEY1 varchar(50), MasterOrder int, 
								DoNotProcess bit, OrderNum2 int, ErrorID int, FlagA char(1), DetailID int, X_Salon_F varchar(50), X_Fam varchar(50),
								Opened varchar(50), Delivered varchar(50), ClickThru varchar(50), SoftBounce varchar(50), HardBounce varchar(50),
								Unsubscribed varchar(50), SalonNum varchar(50), Customer_ID varchar(50))
	INSERT INTO #blastDetail (OrderNum, EmailAddress, LastName, Address1, Address2,	City, State, Zip, Cart, Control, InsertDate, 
								AddressNum, AddressName, AddressName2, HHKEY1, MasterOrder,	DoNotProcess, OrderNum2, ErrorID, FlagA, DetailID, X_Salon_F,
								 X_Fam,	Opened, Delivered, ClickThru, SoftBounce, HardBounce,Unsubscribed,SalonNum,Customer_ID)							
	(select b.blastID as OrderNum,
			e.emailaddress as emailaddress,
			e.LastName as LastName,
			e.Address as Address1,
			e.Address2 as Address2,
			e.City as City,
			e.State as State,
			e.Zip as Zip,
			'' as Cart,
			'1' as control, --?? 0 if test
			e.DateAdded as InsertDate,
			'' as AddressNum,
			'' as AddressName,
			'' as AddressName2,
			'' as HHKEY1,
			b.blastID as MasterOrder,
			'' as DoNotProcess,
			'' as OrderNum2,
			'' as ErrorID,
			'' as FlagA,
			'' as DetailID,
			'' as X_Salon_F,
			'' as X_Fam,
			count(case when actiontypecode='open' then EAID end) as Opened,
			SendTotal - count(case when actiontypecode='bounce' then EAID end) as Delivered,
			count(case when actiontypecode='click' then EAID end) as ClickThru,
			count(case when actiontypecode='bounce' and ActionValue='softbounce' then EAID end) as SoftBounce,
			count(case when actiontypecode='bounce' and ActionValue='hardbounce' then EAID end) as HardBounce,
			count(case when actiontypecode='subscribe' then EAID end) as Unsubscribed,
			--edv.DataValue as SalonNum,--get from udf - SalonNumber
			(select datavalue from EmailDataValues edv join GroupDatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID where edv.emailID = e.emailID and gdf.groupID = b.groupID AND gdf.ShortName = 'SALONNUMBER') as salonnumber,

			'' as Customer_ID--customer id of the email profile

	from 
		[BLAST] b join 
		[ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on b.customerID = c.customerID join
		emailactivitylog eal on eal.blastID = b.blastID join
		emails e on e.emailID = eal.emailID --and e.CustomerID = c.CustomerID
		--join EmailDataValues edv on e.EmailID = edv.EmailID 
		--join GroupDatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID AND gdf.ShortName = 'SALONNUMBER'
	where 
		BaseChannelID = 48 and --and b.BlastID = 270416
		c.CustomerID not in (2835, 3012) and 
		StatusCode='sent' and 
		TestBlast = 'N' and
		SendTime between @startDate and @endDate
		--AND LEN(EmailAddress) > 0
	group by e.emailID,b.blastID,b.groupID,emailaddress,SendTotal,e.LastName,e.Address,e.Address2,e.City,e.State,e.Zip,e.DateAdded)  --edv.DataValue,
	--order by b.BlastID asc)
			
	--UPDATE #blastDetail 
	--SET Customer_ID = ISNULL((Select Top 1 gcE.Customer_ID 
	--				FROM GreatClips..Entities ent
	--				JOIN GreatClips..Entity_Salons es on ent.EntityID = es.EntityID  
	--				JOIN GreatClips..Salons s on es.SalonID = s.salonID 
	--				JOIN GreatClips..Email gcE on gcE.SalonID = s.salonnum 
	--				WHERE gcE.EmailAddress = #blastDetail.EmailAddress 
	--					AND gcE.SalonID = #blastDetail.SalonNum),'Unknown')
		
	SELECT * FROM #blastDetail
	ORDER BY OrderNum
	FOR XML RAW ( 'Blast' ),
	ROOT ('BlastDetail'),
	ELEMENTS;	
	
	Drop Table #blastDetail	
end
