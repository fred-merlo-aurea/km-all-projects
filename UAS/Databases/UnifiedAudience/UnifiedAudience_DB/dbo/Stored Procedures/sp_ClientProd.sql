CREATE PROCEDURE [dbo].[sp_ClientProd](
@PubTypeID1 int, 
@PubTypeID2 int, 
@PubTypeID3 int
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

	DECLARE @Filter1 varchar(50);
	DECLARE @Filter2 varchar(50);
	DECLARE @Filter3 varchar(50);

	select @Filter1=PubTypeDisplayName 
	from PubTypes 
	where  PubTypeID=@PubTypeID1;

	select @Filter2=PubTypeDisplayName 
	from PubTypes 
	where  PubTypeID=@PubTypeID2;

	select @Filter3=PubTypeDisplayName 
	from PubTypes 
	where  PubTypeID=@PubTypeID3;


	with cte (PubTypeDisplayName, SubscriptionID, ColumnReference, PubTypeID, SortOrder)
	as
	(
	  select distinct PubTypeDisplayName, subscriptionID, ColumnReference, pt.PubTypeID, 
				case	when @PubTypeID1 = @PubTypeID1 then 1 
						when @PubTypeID1 = @PubTypeID2 then 2
						when @PubTypeID1 = @PubTypeID3 then 3
				end as SortOrder
	from PubTypes pt 
		left outer join Pubs p  on p.PubTypeID = pt.PubTypeID 
		left outer join PubSubscriptions ps on ps.PubID = p.PubID   
	where pt.PubTypeID in (@PubTypeID1, @PubTypeID2, @PubTypeID3) 
	)
	select PubTypeID, PubTypeDisplayName as 'Product', COUNT(distinct SubscriptionID) as 'SubscriberCount', SortOrder
	from cte
	group by PubTypeID, PubTypeDisplayName, SortOrder
	UNION ALL
		select 0, @Filter1+', '+@Filter3, COUNT(1), 6 from (
			select subscriptionID 
			from cte
			where PubTypeID=@PubTypeID3
			group by ColumnReference, subscriptionID
			Intersect 
				select  SubscriptionID
				from cte
				where PubTypeID=@PubTypeID1
				group by ColumnReference, SubscriptionID
		) a
	UNION ALL
		select  0, @Filter1+', '+@Filter2, COUNT(1), 4 from (
			select  SubscriptionID
			from cte
			where PubTypeID=@PubTypeID1
			group by ColumnReference, SubscriptionID
			Intersect 
				select  SubscriptionID
				from cte
				where PubTypeID=@PubTypeID2
				group by ColumnReference, SubscriptionID
		) a
	UNION ALL
		select  0, @Filter3+', '+@Filter2, COUNT(1), 5 from (
			select  SubscriptionID
			from cte
			where PubTypeID=@PubTypeID3
			group by ColumnReference, SubscriptionID
			Intersect 
				select  SubscriptionID
				from cte
				where PubTypeID=@PubTypeID2
				group by ColumnReference, SubscriptionID
		) a
	UNION ALL 
		select 0, 'ALL', COUNT(1), 7 from (
			select  SubscriptionID
			from cte
			where PubTypeID=@PubTypeID3
			group by ColumnReference, SubscriptionID
			Intersect 
				select  SubscriptionID
				from cte
				where PubTypeID=@PubTypeID2
				group by ColumnReference, SubscriptionID
			Intersect 
				select  SubscriptionID
				from cte
				where PubTypeID=@PubTypeID1
				group by ColumnReference, SubscriptionID
		) a
	order by 4
END