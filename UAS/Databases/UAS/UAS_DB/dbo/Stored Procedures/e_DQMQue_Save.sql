CREATE PROCEDURE [dbo].[e_DQMQue_Save]
@ProcessCode   varchar(50),
@ClientID	  int,
@IsDemo bit,
@IsADMS bit = false,
@DateCreated   datetime,
@IsQued	      bit,
@DateQued	  datetime,
@IsCompleted   bit,
@DateCompleted datetime,
@SourceFileId int = 0
AS
BEGIN

	set nocount on

	IF EXISTS (Select ProcessCode From DQMQue with(nolock) Where ProcessCode = @ProcessCode and ClientID = @ClientID and IsDemo = @IsDemo)
		BEGIN
			UPDATE DQMQue
			SET IsQued = @IsQued,
				DateQued = @DateQued,
				IsCompleted = @IsCompleted,
				DateCompleted = @DateCompleted
			WHERE ProcessCode = @ProcessCode and ClientID = @ClientID;
		
			SELECT @ProcessCode;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO DQMQue (ProcessCode,ClientID,IsDemo,IsADMS,DateCreated,IsQued,DateQued,IsCompleted,DateCompleted,SourceFileId)
			VALUES(@ProcessCode,@ClientID,@IsDemo,@IsADMS,@DateCreated,@IsQued,@DateQued,@IsCompleted,@DateCompleted,@SourceFileId);SELECT @ProcessCode;
		END

END