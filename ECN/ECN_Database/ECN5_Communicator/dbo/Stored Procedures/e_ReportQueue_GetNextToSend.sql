CREATE PROCEDURE [dbo].[e_ReportQueue_GetNextToSend]
	@BlastID int = null
AS
	IF(@BlastID is not null)
		BEGIN
			if exists(	SELECT TOP 1 rq.* 
						FROM ReportQueue rq with(nolock)
						join ReportSchedule rs with(nolock) on rq.ReportScheduleID = rs.ReportScheduleID
						WHERE rs.BlastID = @BlastID and rq.Status = 'Pending' and rq.SendTime < GETDATE()
					)
				BEGIN
					SELECT TOP 1 rq.* 
					FROM ReportQueue rq with(nolock)
					join ReportSchedule rs with(nolock) on rq.ReportScheduleID = rs.ReportScheduleID
					WHERE rs.BlastID = @BlastID and rq.Status = 'Pending' and rq.SendTime < GETDATE()
				END
			ELSE
				BEGIN
					SELECT TOP 1 *
					FROM ReportQueue rq with(nolock)
					where rq.Status = 'Pending' and rq.SendTime < GETDATE()
					order by rq.SendTime asc
				END
		END
	ELSE
		BEGIN
			SELECT TOP 1 *
			FROM ReportQueue rq with(nolock)
			where rq.Status = 'Pending' and rq.SendTime < GETDATE()
			order by rq.SendTime asc

		END
