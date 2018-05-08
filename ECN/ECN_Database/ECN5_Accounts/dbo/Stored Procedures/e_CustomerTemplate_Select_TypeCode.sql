CREATE PROCEDURE [dbo].[e_CustomerTemplate_Select_TypeCode]   
@CustomerID int = NULL,
@TemplateTypeCode varchar(50) = null
AS
	SELECT * FROM CustomerTemplate WHERE CustomerID = @CustomerID and TemplateTypeCode = @TemplateTypeCode and IsActive = 1 and IsDeleted = 0
