﻿CREATE  PROC [dbo].[e_Wizard_Save] 
(
    @WizardID int,
    @IsNewMessage bit,
    @WizardName varchar(100),
    @EmailSubject varchar(255),
    @FromName varchar(100),
    @FromEmail varchar(100),
    @ReplyTo varchar(100),
    @UserID int,
    @GroupID int,
    @ContentID int,
    @LayoutID int,
    @BlastID int,
    @FilterID int,
    @StatusCode varchar(25),
    @CompletedStep int,
    @CardcvNumber varchar(50),
    @CardExpiration varchar(50),
    @CardHolderName varchar(100),
    @CardNumber varchar(50),
    @CardType varchar(20),
    @TransactionNo varchar(50),
    @MultiGroupIDs varchar(2000),
    @SuppressionGroupIDs varchar(255),
    @PageWatchID int,
    @BlastType varchar(10),
    @EmailSubject2 varchar(255),
    @ContentID2 int,
    @LayoutID2 int,
    @SampleWizardID int,
    @RefBlastID int,
    @CampaignID int,
    @DynamicFromEmail varchar(100),
    @DynamicFromName varchar(100),
    @DynamicReplyToEmail varchar(100)
)
AS 
BEGIN
	IF @WizardID is NULL or @WizardID <= 0
	BEGIN
		INSERT INTO Wizard
		(
            IsNewMessage,
            WizardName,
            EmailSubject,
            FromName,
            FromEmail,
            ReplyTo,
            GroupID,
            ContentID,
            LayoutID,
            BlastID,
            FilterID,
            StatusCode,
            CompletedStep,
            CardcvNumber,
            CardExpiration,
            CardHolderName,
            CardNumber,
            CardType,
            TransactionNo,
            MultiGroupIDs,
            SuppressionGroupIDs,
            PageWatchID,
            BlastType,
            EmailSubject2,
            ContentID2,
            LayoutID2,
            SampleWizardID,
            RefBlastID,
            DynamicFromEmail,
            DynamicFromName,
            DynamicReplyToEmail,
			CreatedUserID,		
			CreatedDate,
			IsDeleted		
		)
		VALUES
		(
            @IsNewMessage,
            @WizardName,
            @EmailSubject,
            @FromName,
            @FromEmail,
            @ReplyTo,
            @GroupID,
            @ContentID,
            @LayoutID,
            @BlastID,
            @FilterID,
            @StatusCode,
            @CompletedStep,
            @CardcvNumber,
            @CardExpiration,
            @CardHolderName,
            @CardNumber,
            @CardType,
            @TransactionNo,
            @MultiGroupIDs,
            @SuppressionGroupIDs,
            @PageWatchID,
            @BlastType,
            @EmailSubject2,
            @ContentID2,
            @LayoutID2,
            @SampleWizardID,
            @RefBlastID,
            @DynamicFromEmail,
            @DynamicFromName,
            @DynamicReplyToEmail,
			@UserID,		
			GetDate(),
			0
		)
		
		SET @WizardID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE wizard
			SET             
			IsNewMessage = @IsNewMessage,
            WizardName = @WizardName,
            EmailSubject = @EmailSubject,
            FromName = @FromName,
            FromEmail = @FromEmail,
            ReplyTo = @ReplyTo,
            GroupID = @GroupID,
            ContentID = @ContentID,
            LayoutID = @LayoutID,
            BlastID = @BlastID,
            FilterID = @FilterID,
            StatusCode = @StatusCode,
            CompletedStep = @CompletedStep,
            CardcvNumber = @CardcvNumber,
            CardExpiration = @CardExpiration,
            CardHolderName = @CardHolderName,
            CardNumber = @CardNumber,
            CardType = @CardType,
            TransactionNo = @TransactionNo,
            MultiGroupIDs = @MultiGroupIDs,
            SuppressionGroupIDs = @SuppressionGroupIDs,
            PageWatchID = @PageWatchID,
            BlastType = @BlastType,
            EmailSubject2 = @EmailSubject2,
            ContentID2 = @ContentID2,
            LayoutID2 = @LayoutID2,
            SampleWizardID = @SampleWizardID,
            RefBlastID = @RefBlastID,
            DynamicFromEmail = @DynamicFromEmail,
            DynamicFromName = @DynamicFromName,
            DynamicReplyToEmail = @DynamicReplyToEmail,
			UpdatedUserID=@UserID,
			UpdatedDate=GETDATE()
				
		WHERE
			WizardID = @WizardID
	END

	SELECT @WizardID  
END