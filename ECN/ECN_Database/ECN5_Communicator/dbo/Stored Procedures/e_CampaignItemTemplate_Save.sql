CREATE  PROC [dbo].[e_CampaignItemTemplate_Save] 
(
	@CampaignItemTemplateID int = NULL,	
	@TemplateName varchar(255) = NULL,
	@CustomerID int = NULL,
	@GroupID int = NULL,
	@SuppressionGroupID int = NULL,
	@BlastField1 varchar(255) = NULL,
	@BlastField2 varchar(255) = NULL,
	@BlastField3 varchar(255) = NULL,
	@BlastField4 varchar(255) = NULL,
	@BlastField5 varchar(255) = NULL,
	@Omniture1 varchar(255) = NULL,
	@Omniture2 varchar(255) = NULL,
	@Omniture3 varchar(255) = NULL,
	@Omniture4 varchar(255) = NULL,
	@Omniture5 varchar(255) = NULL,
	@Omniture6 varchar(255) = NULL,
	@Omniture7 varchar(255) = NULL,
	@Omniture8 varchar(255) = NULL,
	@Omniture9 varchar(255) = NULL,
	@Omniture10 varchar(255) = NULL,
	@FromName varchar(255) = NULL,
	@FromEmail varchar(100) = NULL,
	@ReplyTo varchar(100) = NULL,
	@Subject varchar(255) = NULL,
	@UserID int = null	,
	@Archived bit = null,
	@LayoutID int = null,
	@OptOutMasterSuppression bit = null,	
	@OmnitureCustomerSetup bit = null,
	@OptOutSpecificGroup bit = null,
	@CampaignID int = null
)
AS 
BEGIN
	IF @CampaignItemTemplateID is NULL or @CampaignItemTemplateID <= 0
	BEGIN
		INSERT INTO CampaignItemTemplates
		(
			TemplateName, CustomerID, GroupID,BlastField1, BlastField2, BlastField3,BlastField4, BlastField5, Omniture1, Omniture2, Omniture3, Omniture4, Omniture5, Omniture6,
			Omniture7, Omniture8, Omniture9, Omniture10,
			FromName,FromEmail,ReplyTo,[Subject],CreatedUserID,CreatedDate, IsDeleted, SuppressionGroupID,Archived, LayoutID, OptOutMasterSuppression, OptOutSpecificGroup, OmnitureCustomerSetup,CampaignID
		)
		VALUES
		(
			@TemplateName, @CustomerID, @GroupID,@BlastField1, @BlastField2, @BlastField3,@BlastField4, @BlastField5, @Omniture1, @Omniture2, @Omniture3, @Omniture4, @Omniture5,@Omniture6,
			@Omniture7, @Omniture8, @Omniture9, @Omniture10,
			@FromName,@FromEmail,@ReplyTo,@Subject,@UserID,GetDate(),0, @SuppressionGroupID,0, @LayoutID, @OptOutMasterSuppression, @OptOutSpecificGroup,@OmnitureCustomerSetup, @CampaignID
		)
		SET @CampaignItemTemplateID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE CampaignItemTemplates
			SET 	
			TemplateName=@TemplateName, 
			CustomerID=@CustomerID,
			GroupID=@GroupID,
			SuppressionGroupID=@SuppressionGroupID,
			BlastField1=@BlastField1, 
			BlastField2=@BlastField2, 
			BlastField3=@BlastField3,
			BlastField4=@BlastField4, 
			BlastField5=@BlastField5, 
			Omniture1 = @Omniture1,
			Omniture2 = @Omniture2,
			Omniture3 = @Omniture3,
			Omniture4 = @Omniture4,
			Omniture5 = @Omniture5,
			Omniture6 = @Omniture6,
			Omniture7 = @Omniture7,
			Omniture8 = @Omniture8,
			Omniture9 = @Omniture9,
			Omniture10 = @Omniture10,
			FromName=@FromName,
			FromEmail=@FromEmail,
			ReplyTo=@ReplyTo,
			[Subject]=@Subject,
			UpdatedUserID=@UserID,
			UpdatedDate=GETDATE(),
			Archived = @Archived,
			LayoutID = @LayoutID,
			OptOutMasterSuppression = @OptOutMasterSuppression,			
			OmnitureCustomerSetup = @OmnitureCustomerSetup,
			OptOutSpecificGroup = @OptOutSpecificGroup,
			CampaignID=@CampaignID
		WHERE
			CampaignItemTemplateID = @CampaignItemTemplateID
	END

	SELECT @CampaignItemTemplateID
END