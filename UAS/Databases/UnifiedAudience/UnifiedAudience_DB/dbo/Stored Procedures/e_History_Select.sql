﻿CREATE PROCEDURE e_History_Select
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM History With(NoLock) 

END