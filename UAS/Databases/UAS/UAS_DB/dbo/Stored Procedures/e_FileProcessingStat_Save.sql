create procedure e_FileProcessingStat_Save
@ClientId int,
@FileCount int = 0,
@ProfileCount int = 0,
@DemographicCount int = 0,
@ProcessDate date = ''
as
BEGIN

	set nocount on

	if(len(@ProcessDate) = 0)
		set @ProcessDate = GETDATE()
	
	if not exists(select ClientId from FileProcessingStat where ClientId = @ClientId and ProcessDate = @ProcessDate)
		begin
			insert into FileProcessingStat (ClientId,FileCount,ProfileCount,DemographicCount,ProcessDate)
			values(@ClientId,@FileCount,@ProfileCount,@DemographicCount,@ProcessDate)
		end
	else
		begin
			update FileProcessingStat
			set FileCount = @FileCount,
				ProfileCount = @ProfileCount,
				DemographicCount = @DemographicCount 
			where ClientId = @ClientId and ProcessDate = @ProcessDate 
		end

END
go