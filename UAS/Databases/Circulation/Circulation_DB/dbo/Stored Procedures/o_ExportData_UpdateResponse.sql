CREATE PROCEDURE [dbo].[o_ExportData_UpdateResponse]
@UserID int,
@Import DEImportTable readonly,
@SubscriptionID int,
@PublicationID int,
@Column varchar(50),
@Codes varchar(8000)
as

Update SubscriptionResponseMap
SET IsActive = 1, DateUpdated = GETDATE(), UpdatedByUserID = @UserID
FROM @Import i
where ResponseID in
(
	Select ResponseID from Response 
	where 
	PublicationID = @PublicationID
	AND ResponseCode in (Select * from Circulation.dbo.fn_SplitColumn(@Codes, ','))
	AND ResponseName in 
	( 
		select TN.N.value('local-name(.)', 'sysname') as ColumnName from 
		  ( select TV.* from (select 1) as D(N)
			outer apply ( select top(0) * from @Import ) as TV
			for xml path(''), elements xsinil, type) as TX(X)
		cross apply TX.X.nodes('*') as TN(N)
		WHERE TN.N.value('local-name(.)', 'sysname') in 
		(
			Select distinct ResponseName from Response where PublicationID = @PublicationID
		)
	)
	and ResponseName = @Column
)
and SubscriptionResponseMap.SubscriptionID = @SubscriptionID

