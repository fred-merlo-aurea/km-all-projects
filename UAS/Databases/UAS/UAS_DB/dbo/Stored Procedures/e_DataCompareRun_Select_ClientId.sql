CREATE PROCEDURE [dbo].[e_DataCompareRun_Select_ClientId]
@clientId int
as
	begin
		set nocount on
		select * from DataCompareRun with(nolock) where ClientId = @clientId
	end
go
