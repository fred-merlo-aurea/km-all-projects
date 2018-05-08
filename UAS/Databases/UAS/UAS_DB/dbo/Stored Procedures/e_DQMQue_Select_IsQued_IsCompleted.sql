CREATE PROCEDURE [dbo].[e_DQMQue_Select_IsQued_IsCompleted]
@IsDemo bit,
@IsADMS bit,
@IsQued bit,
@IsCompleted bit
AS
BEGIN

	set nocount on

	select *
	from DQMQue With(NoLock)
	where IsQued = @IsQued 
	and IsCompleted = @IsCompleted
	and IsDemo = @IsDemo
	and IsADMS = @IsADMS
	order by DateCreated

END
GO