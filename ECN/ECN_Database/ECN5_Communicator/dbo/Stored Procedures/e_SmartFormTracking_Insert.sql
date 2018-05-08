CREATE PROCEDURE [dbo].[e_SmartFormTracking_Insert]
  @BlastID int = NULL,
  @CustomerID int = NULL,
  @SmartFormID int = NULL,
  @GroupID int = NULL,
  @ReferringURL varchar(max)

	AS
	BEGIN
	INSERT INTO SmartFormTracking
	(
		BlastID, CustomerID, SmartFormID, GroupID, ReferringURL,ActivityDate)
	VALUES
	(
		@BlastID, @CustomerID, @SmartFormID, @GroupID, @ReferringURL, GetDate()
	)
	END
