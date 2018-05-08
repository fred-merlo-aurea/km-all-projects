CREATE PROCEDURE [dbo].[e_PersonalizedContent_Update_Processed]
(
	@PersonalizedContentID bigint,
	@HTMLContent varchar(max),
	@TEXTContent varchar(max),
	@IsProcessed bit
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Update PersonalizedContent 
	set
		HTMLContent = @HTMLContent,
		TEXTContent = @TEXTContent,
		IsProcessed = @isProcessed,
		UpdatedDate = getdate()
	where
		personalizedContentID = @PersonalizedContentID

END