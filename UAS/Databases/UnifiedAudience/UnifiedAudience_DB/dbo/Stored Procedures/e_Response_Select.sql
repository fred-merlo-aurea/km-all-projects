﻿CREATE PROCEDURE e_Response_Select
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM Response With(NoLock)

END