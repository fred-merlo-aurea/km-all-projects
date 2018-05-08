CREATE PROCEDURE e_IssueSplit_Update
@IssueSplitId int,
@IssueSplitName varchar(MAX),
@WaveMailingID int =NULL,
@KeyCode varchar(50)= NULL,
@UpdatedByUserID int,
@IssueSplitDescription varchar(MAX)= NULL
AS
BEGIN
	SET NOCOUNT ON
	IF @IssueSplitId > 0
		BEGIN
			UPDATE IssueSplit
			SET 
				DateUpdated = GETDATE(),
				UpdatedByUserID = @UpdatedByUserID,
				IssueSplitDescription = @IssueSplitDescription,
				WaveMailingID =@WaveMailingID,
				KeyCode =@KeyCode,
				IssueSplitName=@IssueSplitName
			WHERE IssueSplitId = @IssueSplitId;
		END
	SELECT @IssueSplitId;
END  