CREATE PROCEDURE e_DQMQue_Select_ClientID_IsQued
@ClientID int,
@IsDemo bit,
@IsADMS bit,
@IsQued bit
AS
	select *
	from DQMQue With(NoLock)
	where IsQued = @IsQued and IsDemo = @IsDemo and IsADMS = @IsADMS and ClientID = @ClientID 
	order by DateCreated
GO
