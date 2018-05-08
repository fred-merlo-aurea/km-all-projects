CREATE PROCEDURE [dbo].[e_DQMQue_Select_ProcessCode]
@ProcessCode varchar(50)
as
	begin
		set nocount on
		select * from DQMQue with(nolock) where ProcessCode = @ProcessCode
	end
go
