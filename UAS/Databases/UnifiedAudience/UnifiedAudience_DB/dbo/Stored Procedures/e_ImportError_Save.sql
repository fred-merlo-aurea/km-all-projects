CREATE PROCEDURE e_ImportError_Save
	@SourceFileID int,
	@RowNumber int,
	@FormattedException varchar(Max),
	@ClientMessage varchar(4000),
	@MAFField varchar(255),
	@BadDataRow varchar(Max),
	@ThreadID int,
	@DateCreated datetime,
	@ProcessCode varchar(50),
	@IsDimensionError bit
AS
BEGIN

	SET NOCOUNT ON

	INSERT INTO ImportError (SourceFileID,RowNumber,FormattedException,ClientMessage,MAFField,BadDataRow,ThreadID,DateCreated,ProcessCode,IsDimensionError)
	VALUES(@SourceFileID,@RowNumber,@FormattedException,@ClientMessage,@MAFField,@BadDataRow,@ThreadID,@DateCreated,@ProcessCode,@IsDimensionError);SELECT @@IDENTITY;

END
GO