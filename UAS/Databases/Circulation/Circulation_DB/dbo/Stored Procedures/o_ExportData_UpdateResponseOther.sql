CREATE PROCEDURE [dbo].[o_ExportData_UpdateResponseOther]
@UserID int,
@Import DEImportTable readonly,
@SubscriptionID int,
@PublicationID int,
@Column varchar(50),
@Codes varchar(8000),
@Other varchar(2000)
as

IF EXISTS (SELECT ResponseID FROM Response WHERE ResponseCode in (Select * from Circulation.dbo.fn_SplitColumn(@Codes, ','))
			AND ResponseName = @Column AND PublicationID = @PublicationID 
			AND ResponseID in (Select ResponseID FROM Response where DisplayName like '%other%'))
BEGIN
	Update SubscriptionResponseMap
	SET IsActive = 1, ResponseOther = @Other, DateUpdated = GETDATE(), UpdatedByUserID = @UserID
	FROM @Import i
	where ResponseID in
	(
		SELECT ResponseID FROM Response WHERE ResponseCode in (Select * from Circulation.dbo.fn_SplitColumn(@Codes, ','))
			AND ResponseName = @Column AND PublicationID = @PublicationID 
			AND ResponseID in (Select ResponseID FROM Response where DisplayName like '%other%')
	)
	and SubscriptionResponseMap.SubscriptionID = @SubscriptionID
END