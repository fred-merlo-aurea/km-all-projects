CREATE PROCEDURE [dbo].[e_AdmsLog_Select_ClientId_SourceFileId]
@ClientId int,
@SourceFileId int
as
	begin
		set nocount on
		select * 
		from AdmsLog with(nolock)
		where ClientId = @ClientId and SourceFileId = @SourceFileId
	end
go
