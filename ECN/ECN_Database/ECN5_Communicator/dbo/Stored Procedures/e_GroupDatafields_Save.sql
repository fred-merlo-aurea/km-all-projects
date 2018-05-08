CREATE  PROC [dbo].[e_GroupDatafields_Save] 
(
	@GroupDatafieldsID int = NULL,
	@GroupID int = NULL,
	@ShortName varchar(50) = NULL,
	@LongName varchar(255) = NULL,
	@SurveyID int = NULL,
	@DatafieldSetID int = NULL,
	@IsPublic varchar(1) = NULL,
	@IsPrimaryKey bit = null,
	@UserID int = null
)
AS 
BEGIN
	IF @GroupDatafieldsID is NULL or @GroupDatafieldsID <= 0
	BEGIN
		INSERT INTO GroupDatafields
		(
			GroupID,ShortName,LongName,SurveyID,DatafieldSetID,IsPublic,IsPrimaryKey,CreatedUserID,CreatedDate,IsDeleted
		)
		VALUES
		(
			@GroupID,@ShortName,@LongName,@SurveyID,@DatafieldSetID,@IsPublic,@IsPrimaryKey,@UserID,GetDate(),0
		)
		SET @GroupDatafieldsID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE GroupDatafields
			SET GroupID=@GroupID,ShortName=@ShortName,LongName=@LongName,SurveyID=@SurveyID,DatafieldSetID=@DatafieldSetID,IsPublic=@IsPublic,
				IsPrimaryKey=@IsPrimaryKey,UpdatedUserID=@UserID,UpdatedDate=GETDATE()
		WHERE
			GroupDatafieldsID = @GroupDatafieldsID
	END

	SELECT @GroupDatafieldsID
END