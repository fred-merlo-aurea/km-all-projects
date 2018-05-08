﻿CREATE  PROC [dbo].[e_APILogging_Insert] 
(
	@AccessKey uniqueidentifier = NULL,
	@APIMethod varchar(255) = NULL,
	@Input varchar(MAX) = NULL
)
AS 
BEGIN
	INSERT INTO APILogging
	(
		AccessKey,APIMethod,Input,StartTime
	)
	VALUES
	(
		@AccessKey, @APIMethod, @Input, GETDATE()
	)
	SELECT @@IDENTITY
END