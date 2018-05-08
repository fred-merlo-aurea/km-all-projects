CREATE PROCEDURE [dbo].[e_UserLog_Save]
@ApplicationID int,
@UserLogTypeID int,
@UserID int,
@Object nvarchar(50),
@FromObjectValues text = '',
@ToObjectValues text  = '',
@DateCreated datetime
AS
IF @DateCreated IS NULL
	BEGIN
		SET @DateCreated = GETDATE();
	END
INSERT INTO UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
VALUES(@ApplicationID,@UserLogTypeID,@UserID,@Object,@FromObjectValues,@ToObjectValues,@DateCreated);SELECT @@IDENTITY;
