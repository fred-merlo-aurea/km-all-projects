CREATE PROCEDURE [dbo].[e_UserLog_Save]
@ApplicationID int,
@UserLogTypeID int,
@UserID int,
@Object nvarchar(50),
@FromObjectValues text = '',
@ToObjectValues text  = '',
@DateCreated datetime,
@ClientID int = 0,
@GroupTransactionCode varchar(50) = ''
AS
begin
		set nocount on
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated,ClientID,GroupTransactionCode)
		VALUES(@ApplicationID,@UserLogTypeID,@UserID,@Object,@FromObjectValues,@ToObjectValues,@DateCreated,@ClientID,@GroupTransactionCode);SELECT @@IDENTITY;
	end
go
