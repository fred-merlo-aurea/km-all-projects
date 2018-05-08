BEGIN TRY
	BEGIN TRAN

	IF OBJECT_ID('dbo.LegalBasis', N'U') IS NULL
	BEGIN
		CREATE TABLE dbo.LegalBasis
		(
			ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
			PubSubscriptionID INT NOT NULL,
			Details VARCHAR(300) NOT NULL,
			CanProcess BIT NOT NULL,
			Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
			IsActive BIT NOT NULL DEFAULT 1,

			CONSTRAINT FK_LegalBasis_PubSubscriptions FOREIGN KEY (PubSubscriptionID) REFERENCES dbo.PubSubscriptions(PubSubscriptionID)
		);

		CREATE NONCLUSTERED INDEX IX_LegalBasis_PubSubscriptionID_IsActive ON dbo.LegalBasis(PubSubscriptionID, IsActive) WHERE IsActive = 1;
		CREATE NONCLUSTERED INDEX IX_LegalBasis_CanProcess_IsActive ON dbo.LegalBasis(CanProcess, IsActive) WHERE IsActive = 1;
	END

	PRINT 'LegalBasis table has been created successfully.';

	COMMIT TRAN
END TRY
BEGIN CATCH
	SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage;
	
	ROLLBACK TRAN

	PRINT 'An error occurred while trying to perform the operations and all of the operations have been rolled back.';
END CATCH

/* Rollback scripts
BEGIN TRY
	BEGIN TRAN

	IF OBJECT_ID('dbo.LegalBasis', N'U') IS NOT NULL
	DROP TABLE dbo.LegalBasis;

	PRINT 'LegalBasis table has been dropped successfully.';

	COMMIT TRAN
END TRY
BEGIN CATCH
	SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage;
	
	ROLLBACK TRAN

	PRINT 'An error occurred while trying to perform the operations and all of the operations have been rolled back.';
END CATCH
*/