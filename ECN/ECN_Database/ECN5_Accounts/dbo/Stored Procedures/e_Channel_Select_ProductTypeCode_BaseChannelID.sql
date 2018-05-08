CREATE PROCEDURE [dbo].[e_Channel_Select_ProductTypeCode_BaseChannelID] 
	@ProductTypeCode varchar(100),
	@BaseChannelID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT  *
     FROM Channel WITH(NOLOCK)
	 WHERE ChannelTypeCode = @ProductTypeCode AND BaseChannelID = @BaseChannelID and IsDeleted = 0
END
