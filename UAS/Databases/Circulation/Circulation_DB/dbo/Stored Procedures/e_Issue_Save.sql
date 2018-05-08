CREATE PROCEDURE e_Issue_Save
@IssueId int,
@PublicationId int,
@IssueName varchar(100),
@IssueCode varchar(50),
@DateOpened datetime,
@OpenedByUserID int,
@IsClosed bit,
@DateClosed datetime,
@ClosedByUserID int,
@IsComplete bit,
@DateComplete datetime,
@CompleteByUserID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE Issue
		SET PublicationId = @PublicationId,
			IssueName = @IssueName,
			IssueCode = @IssueCode,
			DateOpened = @DateOpened,
			OpenedByUserID = @OpenedByUserID,
			IsClosed = @IsClosed,
			DateClosed = @DateClosed,
			ClosedByUserID = @ClosedByUserID,
			IsComplete = @IsComplete,
			DateComplete = @DateComplete,
			CompleteByUserID = @CompleteByUserID,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueId = @IssueId;

		SELECT @IssueId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into Issue (PublicationId,IssueName,IssueCode,DateOpened,OpenedByUserID,IsClosed,DateClosed,ClosedByUserID,IsComplete,DateComplete,CompleteByUserID,DateCreated,CreatedByUserID)
		VALUES(@PublicationId,@IssueName,@IssueCode,@DateOpened,@OpenedByUserID,@IsClosed,@DateClosed,@ClosedByUserID,@IsComplete,@DateComplete,@CompleteByUserID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO
