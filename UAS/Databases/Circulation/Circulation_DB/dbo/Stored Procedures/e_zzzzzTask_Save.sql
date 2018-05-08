
CREATE PROCEDURE [e_zzzzzTask_Save]
@TaskID int,
@TaskName varchar(50),
@HasSubTask bit,
@IsActive bit,
@IsSubTask bit,
@ParentTaskID int,
@IsAdminTask bit,
@DisplayOrder int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @TaskID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Task
		SET TaskName = @TaskName,
			HasSubTask = @HasSubTask,
			IsActive = @IsActive,
			IsSubTask = @IsSubTask,
			ParentTaskID = @ParentTaskID,
			IsAdminTask = @IsAdminTask,
			DisplayOrder = @DisplayOrder,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE TaskID = @TaskID;
		
		SELECT @TaskID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Task (TaskName,HasSubTask,IsActive,IsSubTask,ParentTaskID,IsAdminTask,DisplayOrder,DateCreated,CreatedByUserID)
		VALUES(@TaskName,@HasSubTask,@IsActive,@IsSubTask,@ParentTaskID,@IsAdminTask,@DisplayOrder,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
