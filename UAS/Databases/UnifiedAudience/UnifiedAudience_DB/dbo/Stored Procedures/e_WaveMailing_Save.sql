CREATE PROCEDURE [e_WaveMailing_Save]
@WaveMailingID int,
@IssueID int,
@WaveMailingName varchar(100),
@WaveNumber int,
@PublicationID int,
@DateSubmittedToPrinter datetime,
@DateCreated datetime,
@DateUpdated datetime,
@SubmittedToPrinterByUserID int,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF @WaveMailingID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE WaveMailing
			SET IssueID = @IssueID,
				WaveMailingName = @WaveMailingName,
				WaveNumber = @WaveNumber,
				PublicationID = @PublicationID,
				DateSubmittedToPrinter = @DateSubmittedToPrinter,
				DateUpdated = @DateUpdated,
				SubmittedToPrinterByUserID = @SubmittedToPrinterByUserID,
				UpdatedByUserID = @UpdatedByUserID
			WHERE WaveMailingID = @WaveMailingID;

			SELECT @WaveMailingID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT intO WaveMailing(IssueID,WaveMailingName,WaveNumber,PublicationID,DateCreated,CreatedByUserID)
			VALUES(@IssueID,@WaveMailingName,@WaveNumber,@PublicationID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO