Create PROCEDURE [dbo].[e_Customer_Select_PlatformClientID]
@PlatformClientID int
AS

SELECT * FROM Customer WITH(NOLOCK) WHERE PlatformClientID = @PlatformClientID  and IsDeleted=0