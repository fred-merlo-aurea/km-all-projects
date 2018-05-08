CREATE PROCEDURE [dbo].[e_TransformationPubMap_Save]
@TransformationPubMapID int,
@TransformationID int,
@PubID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF NOT EXISTS 
	(
	SELECT * FROM TransformationPubMap with(nolock)
	WHERE TransformationID = @TransformationID
	AND PubID = @PubID
	)
	Begin
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO TransformationPubMap (TransformationId, PubID, IsActive, DateCreated, CreatedByUserID)
		VALUES(@TransformationID, @PubID, @IsActive, @DateCreated, @CreatedByUserID);SELECT @PubID;
	End
	Else
	Begin
		Select 0;
	End

END