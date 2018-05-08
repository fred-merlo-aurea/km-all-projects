CREATE PROCEDURE [dbo].[spGetWizardSentBlasts]
	@UserID int = NULL
AS
BEGIN
	SELECT w.WizardID, w.WizardName, b.SendTime, w.CampaignItemID, '../ecn.communicator/main/blasts/reports.aspx?CampaignItemID='+convert(varchar,w.CampaignItemID) as 'ReportURL'
	FROM
		Wizard w WITH (NOLOCK)
		JOIN CampaignItem ci WITH (NOLOCK) ON w.CampaignItemID = ci.CampaignItemID
		JOIN CampaignItemBlast cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
		JOIN Blast b WITH (NOLOCK) ON cib.BlastID = b.BlastID
	WHERE
		b.StatusCode = 'Sent' AND
		w.StatusCode = 'Completed' AND
		ci.IsDeleted = 0 AND 
		cib.IsDeleted = 0 AND
		(w.CreatedUserID = @UserID OR w.UpdatedUserID = @UserID)
	GROUP BY
		w.WizardID, w.WizardName, b.SendTime, w.CampaignItemID
	UNION	
	SELECT w.WizardID, w.WizardName, b.SendTime, w.CampaignItemID, '../ecn.communicator/main/blasts/reports.aspx?CampaignItemID='+convert(varchar,w.CampaignItemID) as 'ReportURL'
	FROM
		Wizard w WITH (NOLOCK)
		JOIN CampaignItem ci WITH (NOLOCK) ON w.CampaignItemID = ci.CampaignItemID
		JOIN CampaignItemTestBlast citb WITH (NOLOCK) ON ci.CampaignItemID = citb.CampaignItemID
		JOIN Blast b WITH (NOLOCK) ON citb.BlastID = b.BlastID
	WHERE
		b.StatusCode = 'Sent' AND
		w.StatusCode = 'Completed' AND
		ci.IsDeleted = 0 AND 
		citb.IsDeleted = 0 AND
		(w.CreatedUserID = @UserID OR w.UpdatedUserID = @UserID)
	GROUP BY
		w.WizardID, w.WizardName, b.SendTime, w.CampaignItemID	
		

	--SELECT w.wizardID, w.WizardName, b.SendTime, w.BlastID, ISNULL(wm.BlastGroupID,0) AS 'BlastGroupID', 
	--CASE
	--WHEN ISNULL(wm.BlastGroupID,0)>0 THEN '../ecn.communicator/main/blasts/reports.aspx?BlastGroupID='+CONVERT(VARCHAR,wm.BlastGroupID)
	--ELSE '../ecn.communicator/main/blasts/reports.aspx?BlastID='+CONVERT(VARCHAR,w.BlastID)
	--END AS 'ReportURL'
	--FROM Wizard w
	--JOIN [BLAST] b ON w.BlastID = b.BlastID
	--JOIN wizardMisc wm ON w.WizardID = wm.wizardID
	--WHERE b.statuscode='sent' AND w.StatusCode='completed' AND w.CreatedUserID = @UserID-- ORDER BY sendtime DESC
	--union
	--select w.wizardID, w.WizardName + ' : ' + b.EmailSubject, b.SendTime, w.BlastID, isnull(wm.BlastGroupID,0) as 'BlastGroupID', 
	--'../ecn.communicator/main/blasts/reports.aspx?BlastID='+convert(varchar,b.BlastID) as 'ReportURL'
	--from 
	--	Wizard w with (nolock)
	--	JOIN wizardMisc wm with (nolock) on w.WizardID = wm.wizardID
	--	join [SAMPLE] s with (nolock) on w.BlastID = s.BlastID
	--	join SampleBlasts sb with (nolock) on s.SampleID = sb.SampleID 
	--	join [BLAST] b with (nolock) on sb.BlastID = b.BlastID 
	--where 
	--	b.StatusCode = 'sent' and
	--	w.StatusCode = 'completed' and
	--	w.CreatedUserID = @UserID
	--order by
	--	b.SendTime desc
END