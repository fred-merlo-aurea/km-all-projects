﻿CREATE PROCEDURE [dbo].[e_Table_Select]
AS
BEGIN

	set nocount on

	SELECT TABLE_NAME as 'TableName' 
    FROM INFORMATION_SCHEMA.TABLES With(NoLock)
    WHERE TABLE_TYPE = 'BASE TABLE'

END