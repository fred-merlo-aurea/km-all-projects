CREATE PROCEDURE [dbo].[e_CustomerContact_Select_ContactID]
@ContactID int
AS

SELECT * FROM CustomerContact WHERE ContactID = @ContactID  and IsDeleted=0
