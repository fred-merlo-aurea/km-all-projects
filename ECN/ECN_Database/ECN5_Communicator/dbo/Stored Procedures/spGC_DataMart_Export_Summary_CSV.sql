CREATE proc [dbo].[spGC_DataMart_Export_Summary_CSV]
(
@startDate datetime = null,
@endDate datetime = null
)
as
Begin
	declare  @currentmonthstartdate datetime
	
	
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
	
	--set @startDate = Cast('1/1/2010' as datetime)
	--set @endDate = cast('9/30/2010' as datetime)

	select	b.BlastID as MasterOrder,
			SendTotal as TotalOrderQTY,
			case when c.CustomerID = 2519 then 'NAF' else 'SBS' end as FORM,
			case when f.foldername like '%PROSPECT%' then 'E' else 'D' end as FileNum,	
			'' as Salon1,
			'' as Salon2,
			b.BlastID as OrderNum,			
			Convert(decimal(20,5),isnull((case when ltrim(rtrim(l.DesignCost)) = '' then '0' else ltrim(rtrim(l.DesignCost)) end),0)) + 
			Convert(decimal(20,5),isnull((case when ltrim(rtrim(l.OtherCost)) = '' then '0' else ltrim(rtrim(l.OtherCost)) end),0)) + 
			((case when CAST(l.OutboundCost as real) = 0 or l.OutboundCost = '' then CAST((case when ltrim(rtrim(isnull(customer_udf1,''))) = '' then 0.00000 else customer_udf1 end) as decimal(20,5))  else isnull(CAST(l.OutboundCost as decimal(20,5)),0)  end) * SendTotal) as CostperorderNum,
			CONVERT(VARCHAR(10), sendtime,101) as MailDate,
			sendTotal as Quantity,
			bf.field2 as CouponValue1,-- dbo.fn_getGCCouponData(cd.CodeDisplay, 'value') 
			bf.field4 as CouponExpiration1,-- dbo.fn_getGCCouponData(cd.CodeDisplay, 'exp')
			bf.field3 as CouponType1,-- dbo.fn_getGCCouponData(cd.CodeDisplay, 'type')
			'' as CouponValue2,
			'' as CouponExpiration2,
			'' as CouponType2,
			'' as CouponValue3,
			'' as CouponExpiration3,
			'' as CouponType3,
			'' as CouponValue4,
			'' as CouponExpiration4,
			'' as CouponType4,
			'' as CouponValue5,
			'' as CouponExpiration5,
			'' as CouponType5,
			'' as CouponValue6,
			'' as CouponExpiration6,
			'' as CouponType6,
			'' as CouponValue7,
			'' as CouponExpiration7,
			'' as CouponType7,
			'' as CouponValue8,
			'' as CouponExpiration8,
			'' as CouponType8,
			'' as InsertDate,
			(case when CAST(l.OutboundCost as real) = 0 or l.OutboundCost = '' then CAST((case when ltrim(rtrim(isnull(customer_udf1,''))) = '' then 0.00000 else customer_udf1 end) as decimal(20,5)) else isnull(CAST(l.OutboundCost as decimal(20,5)),0)  end) * SendTotal as  [qty_piece_price],
			(case when CAST(l.OutboundCost as real) = 0 or l.OutboundCost = '' then CAST((case when ltrim(rtrim(isnull(customer_udf1,''))) = '' then 0.00000 else customer_udf1 end) as decimal(20,5)) else isnull(CAST(l.OutboundCost as decimal(20,5)),0)  end) as Piece_price,
			'' as HHMatched,
			'' as SourceFlag --case when f.foldername ='PROSPECT' then 'E' else 'D' end
	from 
		[BLAST] b with (nolock) join 
		Groups g with (nolock) on b.GroupID = g.groupID left outer join 
		[FOLDER] f with (nolock) on f.FolderID = g.FolderID join
		[LAYOUT] l with (nolock) on l.LayoutID = b.LayoutID join
		[ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c with (nolock) on b.customerID = c.customerID left outer join
		Code cd with (nolock) on b.codeID = cd.codeID left outer join
		BlastFields bf on b.BlastID = bf.BlastID
	where 
		BaseChannelID = 48 and 
		c.CustomerID not in (2835, 3012, 3037) and 
		StatusCode='sent' and 
		TestBlast = 'N' and
		SendTime between @startDate and @endDate and
		SendTotal > 0
	order by SendTime asc
	
end
