CREATE PROCEDURE [dbo].[e_DQMQue_Select_ClientID]
@ClientID int,
@IsDemo bit,
@IsADMS bit
AS
BEGIN

	set nocount on

	select *
	from DQMQue With(NoLock)
	where ClientID = @ClientID and IsDemo = @IsDemo and IsADMS = @IsADMS
	order by DateCreated

END
GO