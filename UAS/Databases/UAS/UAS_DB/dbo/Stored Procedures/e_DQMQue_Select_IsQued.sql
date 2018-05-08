CREATE PROCEDURE [dbo].[e_DQMQue_Select_IsQued]
@IsDemo bit,
@IsADMS bit,
@IsQued bit
AS
BEGIN

	set nocount on

	select *
	from DQMQue With(NoLock)
	where IsQued = @IsQued and IsDemo = @IsDemo and IsADMS = @IsADMS
	order by DateCreated

END
GO