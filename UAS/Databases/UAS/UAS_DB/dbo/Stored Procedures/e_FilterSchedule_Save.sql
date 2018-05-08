CREATE PROCEDURE e_FilterSchedule_Save
@FilterScheduleId int,
@FilterId int,
@ExportTypeId int,
@IsRecurring bit,
@RecurrenceTypeId int,
@StartDate date,
@StartTime time,
@EndDate date,
@RunSunday bit,
@RunMonday bit,
@RunTuesday bit,
@RunWednesday bit,
@RunThursday bit,
@RunFriday bit,
@RunSaturday bit,
@MonthScheduleDay int,
@MonthLastDay bit,
@EmailNotification varchar(500),
@Server varchar(50),
@UserName varchar(50),
@Password varchar(50),
@Folder varchar(50),
@ExportFormatTypeId int,
@FileName varchar(50),
@IsDeleted bit,
@ClientID int,
@FilterGroupId_Selected int,
@FilterGroupID_Suppressed int,
@FolderId int,
@GroupId int,
@OperatorsTypeId  int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @FilterScheduleId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE FilterSchedule
			SET FilterId = @FilterId,
				ExportTypeId = @ExportTypeId,
				IsRecurring = @IsRecurring,
				RecurrenceTypeId = @RecurrenceTypeId,
				StartDate = @StartDate,
				StartTime = @StartTime,
				EndDate = @EndDate,
				RunSunday = @RunSunday,
				RunMonday = @RunMonday,
				RunTuesday = @RunTuesday,
				RunWednesday = @RunWednesday,
				RunThursday = @RunThursday,
				RunFriday = @RunFriday,
				RunSaturday = @RunSaturday,
				MonthScheduleDay = @MonthScheduleDay,
				MonthLastDay = @MonthLastDay,
				EmailNotification = @EmailNotification,
				Server = @Server,
				UserName = @UserName,
				Password = @Password,
				Folder = @Folder,
				ExportFormatTypeId = @ExportFormatTypeId,
				FileName = @FileName,
				IsDeleted = @IsDeleted,
				ClientID = @ClientID,
				FilterGroupId_Selected = @FilterGroupId_Selected,
				FilterGroupID_Suppressed = @FilterGroupID_Suppressed,
				FolderId = @FolderId,
				GroupId = @GroupId,
				OperatorsTypeId = @OperatorsTypeId,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE FilterScheduleId = @FilterScheduleId;
		
			SELECT @FilterScheduleId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO FilterSchedule (FilterId,ExportTypeId,IsRecurring,RecurrenceTypeId,StartDate,StartTime,EndDate,RunSunday,RunMonday,RunTuesday,RunWednesday,
										RunThursday,RunFriday,RunSaturday,MonthScheduleDay,MonthLastDay,EmailNotification,Server,UserName,Password,Folder,ExportFormatTypeId,
										FileName,IsDeleted,ClientID,FilterGroupId_Selected,FilterGroupID_Suppressed,FolderId,GroupId,OperatorsTypeId,DateCreated,CreatedByUserID)
			VALUES(@FilterId,@ExportTypeId,@IsRecurring,@RecurrenceTypeId,@StartDate,@StartTime,@EndDate,@RunSunday,@RunMonday,@RunTuesday,@RunWednesday,
					@RunThursday,@RunFriday,@RunSaturday,@MonthScheduleDay,@MonthLastDay,@EmailNotification,@Server,@UserName,@Password,@Folder,@ExportFormatTypeId,
					@FileName,@IsDeleted,@ClientID,@FilterGroupId_Selected,@FilterGroupID_Suppressed,@FolderId,@GroupId,@OperatorsTypeId,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO