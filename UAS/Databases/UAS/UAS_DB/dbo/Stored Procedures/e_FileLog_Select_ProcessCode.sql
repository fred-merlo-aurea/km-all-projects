CREATE PROCEDURE [dbo].[e_FileLog_Select_ProcessCode]
@ProcessCode varchar(50)
as
	begin
		set nocount on
		select * from FileLog with(nolock) where ProcessCode = @ProcessCode
	end
go
