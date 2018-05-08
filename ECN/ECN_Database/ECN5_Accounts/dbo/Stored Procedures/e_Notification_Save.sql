CREATE PROCEDURE [dbo].[e_Notification_Save]
@NotificationID int, 
@NotificationName varchar(100),
@NotificationText text, 
@StartDate varchar(10),
@StartTime varchar(10), 
@EndDate varchar(10), 
@EndTime varchar(10), 
@UserID int,

@BackGroundColor varchar(20),
@CloseButtonColor varchar(20)

AS  

BEGIN   
	SET NOCOUNT ON;  

	if (@NotificationID > 0)
	Begin
		Update Notification
		SET NotificationName = @NotificationName, 
			NotificationText = @NotificationText, 
			StartDate = @StartDate, 
			StartTime = @StartTime,
			EndDate = @EndDate, 
			EndTime = @EndTime,
			UpdatedUserID = @UserID,
			UpdatedDate = GETDATE(),
			BackGroundColor = @BackGroundColor,
			CloseButtonColor = @CloseButtonColor	
			
			Where NotificationID = @NotificationID
		 
		 select @NotificationID
	End
	Else
	Begin
		INSERT INTO Notification	
			(NotificationName, 
			NotificationText, 
			StartDate, 
			StartTime,
			EndDate,
			EndTime, 
			CreatedUserID,
			CreatedDate,
			UpdatedUserID,
			UpdatedDate,
			IsDeleted,
			BackGroundColor,
			CloseButtonColor) 		 
		VALUES 
		(	@NotificationName,
			@NotificationText,
			@StartDate,
			@StartTime,
			@EndDate,
			@EndTime,
			@UserID,
			getdate(),
			@UserID,
			getdate()			
			,0,
			@BackGroundColor,
			@CloseButtonColor)
		
		SELECT @@IDENTITY;
	End
END