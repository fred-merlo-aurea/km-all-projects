CREATE PROCEDURE [e_TransformDataMapping_Save]
@ID int,
@TransformationID int,
@MatchType varchar(50),
@SourceData varchar(200),
@DesiredData varchar(200)
AS
BEGIN

	set nocount on

	IF @ID > 0
		  BEGIN
				UPDATE TransformDataMap
				SET TransformationID = @TransformationID,
					  MatchType = @MatchType,
					  SourceData = @SourceData,
					  DesiredData = @DesiredData           
				WHERE TransformDataMapID = @ID;

				SELECT @ID;
		  END
	ELSE
		  BEGIN
				INSERT INTO TransformDataMap (TransformationID, MatchType, SourceData, DesiredData)
				VALUES(@TransformationID, @MatchType, @SourceData, @DesiredData);SELECT @@IDENTITY;
		  END

END