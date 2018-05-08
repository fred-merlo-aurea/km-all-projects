-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_PersonalizedContent_Save]
	@PersonalizedContentID BIGINT,
	@BlastID BIGINT,
	@EmailAddress varchar(255),
	@EmailSubject varchar(255),
	@HTMLContent varchar(MAX),
	@TEXTContent varchar(MAX),
	@IsValid bit =1,
	@IsProcessed bit = 0,
	@IsDeleted bit = 0,
	@CreatedUserID int = null,
	@UpdatedUserID int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	if @PersonalizedContentID <= 0
		select @PersonalizedContentID = PersonalizedContentID from PersonalizedContent pc with(nolock) where pc.EmailAddress = @EmailAddress and pc.BlastID = @BlastID
	
	if @PersonalizedContentID > 0
	BEGIN
		UPDATE PersonalizedContent
		set BlastID = @BlastID,
			EmailAddress = @EmailAddress,
			EmailSubject = @EmailSubject,
			HTMLContent = @HTMLContent,
			TEXTContent = @TEXTContent,
			IsValid = @IsValid,
			IsProcessed = @IsProcessed,
			IsDeleted = @IsDeleted,
			UpdatedDate = GETDATE(),
			UpdatedUserID = ISNULL(@UpdatedUserID,@CreatedUserID),
			Failed = 0
		WHERE PersonalizedContentID = @PersonalizedContentID
	END
	ELSE 
	BEGIN

		INSERT INTO PersonalizedContent (BlastID,EmailAddress,EmailSubject,HTMLContent,TEXTContent,IsValid,IsProcessed,IsDeleted,CreatedDate,CreatedUserID, Failed)
		VALUES(@BlastID, @EmailAddress, @EmailSubject, @HTMLContent, @TEXTContent, @IsValid, @IsProcessed, @IsDeleted, GETDATE(), @CreatedUserID,0)

		SET @PersonalizedContentID = SCOPE_IDENTITY();
				
	END

	SELECT @PersonalizedContentID	
END