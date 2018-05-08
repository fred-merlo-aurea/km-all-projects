﻿CREATE PROCEDURE [dbo].[e_Select_Groups_CustomerID]   
@customerID int
AS
	SELECT 	
		GroupID,
		CustomerID,
		FolderID,
		GroupName,
		GroupDescription,
		OwnerTypeCode,
		MasterSupression,
		PublicFolder,
		OptinHTML,
		OptinFields,
		AllowUDFHistory,
		IsSeedList
	FROM Groups WITH(NOLOCK) WHERE CustomerID = @customerID