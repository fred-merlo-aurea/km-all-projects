CREATE PROCEDURE [dbo].[e_MasterCodeSheet_Save]
	@MasterID int, 
	@MasterGroupID int, 
	@MasterValue varchar(100), 
	@MasterDesc varchar(255),
	@MasterDesc1 varchar(255),
	@EnableSearching bit,
	@SortOrder int,
	@DateCreated datetime,
	@DateUpdated datetime,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
BEGIN

	SET NOCOUNT ON

	IF @MasterID > 0
		BEGIN
			UPDATE Mastercodesheet
			SET MasterGroupID = @MasterGroupID, 
				MasterValue = @MasterValue, 
				MasterDesc = @MasterDesc,
				MasterDesc1 = @MasterDesc1,
				EnableSearching = @EnableSearching,
				SortOrder = @SortOrder,
				CreatedByUserID = @CreatedByUserID,
				UpdatedByUserID = @UpdatedByUserID
			WHERE MasterID = @MasterID	

			SELECT @MasterID;					
		END			
	ELSE
		BEGIN
			INSERT INTO [Mastercodesheet]([MasterGroupID], [MasterValue], [MasterDesc], [MasterDesc1], [EnableSearching], [SortOrder], [DateCreated], [CreatedByUserID])
				VALUES (@MasterGroupID, @MasterValue, @MasterDesc, @MasterDesc1, @EnableSearching, @SortOrder, @DateCreated, @CreatedByUserID);SELECT @@IDENTITY;
		END

END