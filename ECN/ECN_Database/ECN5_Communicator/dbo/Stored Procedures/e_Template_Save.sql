CREATE  PROC [dbo].[e_Template_Save] 
(
	@TemplateID int = NULL,
	@BaseChannelID int = NULL,
    @TemplateStyleCode varchar(50) = NULL,
    @TemplateName varchar(50) = NULL,
    @TemplateImage varchar(255) = NULL,
    @TemplateDescription varchar(255) = NULL,
    @SortOrder int = NULL,
    @SlotsTotal int = NULL,
    @TemplateSource text = NULL,
    @TemplateText text = NULL,
    @TemplateSubject text = NULL,
    @UserID int = NULL,
    @IsActive  bit = NULL,
    @Category varchar(255) = NULL
)
AS 
BEGIN
	IF @TemplateID is NULL or @TemplateID <= 0
	BEGIN
		INSERT INTO Template
		(
			BaseChannelID,
			TemplateStyleCode,
			TemplateName,
			TemplateImage,
			TemplateDescription,
			SortOrder,
			SlotsTotal,
			TemplateSource,
			TemplateText,
			TemplateSubject,
			IsActive,
			CreatedUserID,		
			CreatedDate,
			IsDeleted,
			Category		
		)
		VALUES
		(
			@BaseChannelID,
			@TemplateStyleCode,
			@TemplateName,
			@TemplateImage,
			@TemplateDescription,
			@SortOrder,
			@SlotsTotal,
			@TemplateSource,
			@TemplateText,
			@TemplateSubject,
			@IsActive,
			@UserID,		
			GetDate(),
			0,
			@Category
		)
		
		SET @TemplateID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE Template
			SET BaseChannelID = @BaseChannelID,
				TemplateStyleCode = @TemplateStyleCode,
				TemplateName = @TemplateName,
				TemplateImage = @TemplateImage,
				TemplateDescription = @TemplateDescription,
				SortOrder = @SortOrder,
				SlotsTotal = @SlotsTotal,
				TemplateSource = @TemplateSource,
				TemplateText = @TemplateText,
				TemplateSubject = @TemplateSubject,
				IsActive = @IsActive,
				UpdatedUserID=@UserID,
				UpdatedDate=GETDATE(),
				Category=@Category
				
		WHERE
			TemplateID = @TemplateID
	END

	SELECT @TemplateID
END