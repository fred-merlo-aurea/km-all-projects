
/****** Object:  StoredProcedure [dbo].[sp_UpdateSMTPMessagetoLive]    Script Date: 10/31/2016 16:00:34 ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

CREATE PROCEDURE [dbo].[sp_UpdateSMTPMessagetoLive]
	
AS
Begin
	SET NOCOUNT ON
	
	-- Execute sp_UpdateSMTPMessagetoLive
	
	declare @batch int = 50000,
			@msg varchar(8000),
            @startID bigint = (SELECT MIN(ID) from SMTPMessage_LogFileData with (NOLOCK)),
            @endID bigint = (SELECT MAX(ID) from SMTPMessage_LogFileData with (NOLOCK))

    set @msg = ' start UpdateSMTPMessagetoLive :  ' + convert(varchar(100), getdate(), 109)
    
    RAISERROR (@msg, 0, 1) WITH NOWAIT

	UPDATE lf
	SET 
			lf.SendID = bas.SendID
	FROM 
			BlastActivitySends bas with (NOLOCK)
	INNER JOIN 
			SMTPMessage_LogFileData lf 
	ON 
			lf.BlastID = bas.BlastID 
			and lf.EmailID = bas.EmailID
	 where bas.BlastID in (select distinct BlastID from  SMTPMessage_LogFileData with (NOLOCK))

    set @msg = ' Update SendID :  ' + convert(varchar(100), getdate(), 109) 
    
    RAISERROR (@msg, 0, 1) WITH NOWAIT
    
    
    while (@startID <= @endID)
    BEGIN
        
        set @msg = convert(varchar(100), @startID ) + ' - ' + convert(varchar(100), @startID + @batch ) + ' / ' + convert(varchar(100), getdate(), 109)
        
        RAISERROR (@msg, 0, 1) WITH NOWAIT
                  
        UPDATE bas
         SET 
                bas.SMTPMessage = lf.SMTPMessage,
                bas.SourceIP=lf.SourceIP
        FROM 
                BlastActivitySends bas 
        INNER JOIN 
                SMTPMessage_LogFileData lf with (NOLOCK)       
        ON 
                bas.SendID = lf.sendID
        WHERE
            ID between @startID and @startID + @batch 

        if @startID < @endID
        Begin
			set @startID = @startID + @batch +1
			
			if @startID > @endID
				set @startID = @endID
        End
        else if @startID = @endID
			set @startID = @startID + 1
	
    END

    TRUNCATE TABLE SMTPMessage_LogFileData
END    
GO


