﻿CREATE PROCEDURE [dbo].[e_PubSubscriptionsExtensionMapper_Exists_ByIDCustomField]
	@PubSubscriptionsExtensionMapperID int, 
	@CustomField varchar(255),
	@PubID int
AS
BEGIN

	SET NOCOUNT ON

	IF EXISTS (
		SELECT TOP 1 PubSubscriptionsExtensionMapperID
		FROM PubSubscriptionsExtensionMapper WITH (NOLOCK)
		WHERE CustomField = @CustomField and PubSubscriptionsExtensionMapperID <> @PubSubscriptionsExtensionMapperID and PubID = @PubID
	) SELECT 1 ELSE SELECT 0

END