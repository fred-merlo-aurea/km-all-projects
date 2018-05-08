CREATE PROCEDURE [dbo].[rpt_Diskmonitor]
	@ChannelID int,
	@Month int,
	@ShowOverlimit varchar(1)
AS

if @ShowOverlimit = 'n'
Begin
	select 	bc.basechannelID as channelID, BC.BaseChannelName, cd.customerID,C.customerName,
			(
				SELECT isnull(sum(Quantity * (case when Convert(int,(replace(replace(Code,'OSLIB',''),'MB',''))) = '' then 25 else Convert(int,(replace(replace(Code,'OSLIB',''),'MB',''))) end)),0) + 10
				FROM	[QuoteItem] qi JOIN [Quote] q ON q.QuoteID = qi.QuoteID 
				WHERE	qi.Code like 'OSLIB%' AND q.Status = '1' AND getDate() BETWEEN q.ApproveDate AND (q.ApproveDate+365) and q.customerID = Cd.customerID 
			) as AllowedStorage,
			avg(convert(decimal(18,2),SizeInBytes)) as SizeInBytes
	from 	CustomerDiskUsage CD join
			[Customer] C on CD.customerID = C.CustomerID join
			[BaseChannel] BC on c.basechannelID = BC.basechannelID 
	where 	
			Month(DateMonitored) = @Month and year(DateMonitored) = year(getdate()) and
			BC.basechannelID = (case when @ChannelID > 0 then @ChannelID else BC.basechannelID end)
	group by bc.basechannelID, cd.customerID, BC.BaseChannelName, C.customerName 
	having avg(convert(decimal(18,2),SizeInBytes)) > 0
	order by  BC.BaseChannelName, C.customerName
end
Else
Begin
	select ChannelID, customerID, BaseChannelName, customerName,AllowedStorage, SizeInBytes
	from
	(
		select 	bc.basechannelID as channelID, BC.BaseChannelName, cd.customerID, c.customerName,
				(
					SELECT isnull(sum(Quantity * (case when Convert(int,(replace(replace(Code,'OSLIB',''),'MB',''))) = '' then 25 else Convert(int,(replace(replace(Code,'OSLIB',''),'MB',''))) end)),0) + 10
					FROM	[QuoteItem] qi JOIN [Quote] q ON q.QuoteID = qi.QuoteID 
					WHERE	qi.Code like 'OSLIB%' AND q.Status = '1' AND getDate() BETWEEN q.ApproveDate AND (q.ApproveDate+365) and q.customerID = Cd.customerID 
				) as AllowedStorage,
				avg(convert(decimal(18,2),SizeInBytes)) as SizeInBytes
		from 	CustomerDiskUsage CD join
				[Customer] C on CD.customerID = C.CustomerID join
				[BaseChannel] BC on c.basechannelID = BC.basechannelID 
		where 	
				Month(DateMonitored) = @Month and year(DateMonitored) = year(getdate()) and
				BC.basechannelID = (case when @ChannelID > 0 then @ChannelID else BC.basechannelID end)
		group by bc.baseChannelID, BC.BaseChannelName, cd.customerID, C.customerName 
		having avg(convert(decimal(18,2),SizeInBytes)) > 0
	) inn
	where ((SizeInBytes/1024)/1024) > AllowedStorage
	order by  BaseChannelName, CustomerName
End
