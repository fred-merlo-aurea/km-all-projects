CREATE  PROC [dbo].[e_SmartFormsHistory_Save] 
(
	@SmartFormID int = NULL,
	@GroupID int = NULL,
	@SubscriptionGroupIDs varchar(500) = NULL,
    @SmartFormName varchar(500) = NULL,
    @SmartFormHtml text = NULL,
    @SmartFormFields varchar(8000) = NULL,
    @Response_UserMsgSubject varchar(500) = NULL,
    @Response_UserMsgBody text = NULL,
    @Response_UserScreen text = NULL,
    @Response_FromEmail varchar(500) = NULL,
    @Response_AdminEmail varchar(500) = NULL,
    @Response_AdminMsgSubject varchar(500) = NULL,
    @Response_AdminMsgBody text = NULL,
    @Type varchar(10) = NULL,
    @DoubleOptIn bit = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	IF @SmartFormID is NULL or @SmartFormID <= 0
	BEGIN
		INSERT INTO SmartFormsHistory
		(
			GroupID,SubscriptionGroupIDs,SmartFormName,SmartFormHtml,SmartFormFields,Response_UserMsgSubject,Response_UserMsgBody,
			Response_UserScreen,Response_FromEmail,Response_AdminEmail,Response_AdminMsgSubject,Response_AdminMsgBody,[Type],DoubleOptIn,
			CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@GroupID,@SubscriptionGroupIDs,@SmartFormName,@SmartFormHtml,@SmartFormFields,@Response_UserMsgSubject,@Response_UserMsgBody,
			@Response_UserScreen,@Response_FromEmail,@Response_AdminEmail,@Response_AdminMsgSubject,@Response_AdminMsgBody,@Type,@DoubleOptIn,
			@UserID,GetDate(),0
		)
		SET @SmartFormID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE SmartFormsHistory
			SET GroupID=@GroupID,SubscriptionGroupIDs=@SubscriptionGroupIDs,SmartFormName=@SmartFormName,SmartFormHtml=@SmartFormHtml,
				SmartFormFields=@SmartFormFields,Response_UserMsgSubject=@Response_UserMsgSubject,Response_UserMsgBody=@Response_UserMsgBody,
				Response_UserScreen=@Response_UserScreen,Response_FromEmail=@Response_FromEmail,Response_AdminEmail=@Response_AdminEmail,
				Response_AdminMsgSubject=@Response_AdminMsgSubject,Response_AdminMsgBody=@Response_AdminMsgBody,[Type]=@Type,DoubleOptIn=@DoubleOptIn,
				UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			SmartFormID = @SmartFormID
	END

	SELECT @SmartFormID
END