CREATE PROCEDURE [dbo].[e_TransformDataMap_Save]
@TransformDataMapID int,
@TransformationID int,
@PubID int,
@MatchType varchar(50),
@SourceData varchar(200),
@DesiredData varchar(200),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @TransformDataMapID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE TransformDataMap
			SET TransformationID = @TransformationID,
				PubID = @PubID,
				MatchType = @MatchType,
				SourceData = @SourceData,
				DesiredData = @DesiredData,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE TransformDataMapID = @TransformDataMapID;
		
			SELECT @TransformDataMapID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO TransformDataMap (TransformationID,PubID,MatchType,SourceData,DesiredData,IsActive,DateCreated,CreatedByUserID)
			VALUES(@TransformationID,@PubID,@MatchType,@SourceData,@DesiredData,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END