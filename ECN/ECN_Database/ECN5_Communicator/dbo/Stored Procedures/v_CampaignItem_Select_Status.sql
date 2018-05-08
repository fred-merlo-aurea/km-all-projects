--sent, pending, active and saved
--this returns campaign items that have any part of them that match the criteria
CREATE PROCEDURE [dbo].[v_CampaignItem_Select_Status]   
@CustomerID int,
@Status varchar(10) = NULL
AS
	IF @Status = 'Saved'
	BEGIN
		select * from 
		(
		SELECT	cp.CustomerID, cp.CampaignID, ci.CampaignItemID, ci.CampaignItemName, ci.CampaignItemType,ci.CampaignItemFormatType,
				ci.FromName,ci.FromEmail,ci.ReplyTo,ci.SendTime,ci.CompletedStep,ci.CreatedUserID,ci.CreatedDate,ci.UpdatedUserID, ci.UpdatedDate, 
				MAX(case when ci.CampaignItemType = 'AB' then '-- Multiple Subject lines --' else cib.EmailSubject end) as emailsubject,
				case when COUNT(cib.GroupID) >= 0 then MAX(g.GroupName) else ' -- Multi Tests Groups --' end  as GroupName 
		FROM 
				Campaign cp WITH (NOLOCK)
				join CampaignItem ci WITH (NOLOCK) on cp.CampaignID = ci.CampaignID
				left outer JOIN CampaignItemBlast cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID and cib.IsDeleted = 0
				left outer JOIN Groups g WITH (NOLOCK) ON cib.groupID = g.groupID
		WHERE 
			cp.CustomerID = @CustomerID  and 
			isnull(ci.IsHidden, 0) = 0 and
			isnull(ci.IsDeleted, 0) = 0 and
			isnull(cp.IsDeleted, 0) = 0 and
			isnull(cib.BlastID,0) = 0
		group by 
				ci.CampaignItemName, ci.CampaignItemType,ci.CampaignItemFormatType,cp.CustomerID, cp.CampaignID, ci.CampaignItemID, 
				ci.FromName,ci.FromEmail,ci.ReplyTo,ci.SendTime,ci.CompletedStep,ci.CreatedUserID,ci.CreatedDate,ci.UpdatedUserID, ci.UpdatedDate
	
		) q1
		order by isnull(q1.UpdatedDate, q1.CreatedDate) DESC
	END
	ELSE --IF @Status = 'Pending' or @Status = 'Active'
	BEGIN
		select * from 
		(
			SELECT  cp.CustomerID, cp.CampaignID, ci.CampaignItemID, ci.CampaignItemName, ci.CampaignItemType,ci.CampaignItemFormatType,
				ci.FromName,ci.FromEmail,ci.ReplyTo,ci.SendTime,ci.CompletedStep,ci.CreatedUserID,ci.CreatedDate,ci.UpdatedUserID, ci.UpdatedDate, 
				MAX(case when ci.CampaignItemType = 'AB' then '-- Multiple Subject lines --' else cib.EmailSubject end) as emailsubject,
				case when COUNT(cib.GroupID) >= 0 then MAX(g.GroupName) else ' -- Multi Tests Groups --' end  as GroupName
		FROM 
				Campaign cp WITH (NOLOCK)
				join CampaignItem ci WITH (NOLOCK) on cp.CampaignID = ci.CampaignID
				JOIN CampaignItemBlast cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
				JOIN Groups g WITH (NOLOCK) ON cib.groupID = g.groupID join
				Blast b  WITH (NOLOCK) on b.blastID = cib.blastID
		WHERE 
				cp.CustomerID = @CustomerID  and 
				isnull(ci.IsHidden, 0) = 0 and
				isnull(cib.IsDeleted, 0) = 0 and 
				isnull(cp.IsDeleted, 0) = 0 and
				isnull(ci.IsDeleted, 0) = 0 and
				b.statuscode = @Status
		group by 
				cp.CustomerID, cp.CampaignID, ci.CampaignItemID, ci.CampaignItemName, ci.CampaignItemType,ci.CampaignItemFormatType,
				ci.FromName,ci.FromEmail,ci.ReplyTo,ci.SendTime,ci.CompletedStep,ci.CreatedUserID,ci.CreatedDate,ci.UpdatedUserID, ci.UpdatedDate
		UNION 		
		SELECT  cp.CustomerID, cp.CampaignID, ci.CampaignItemID, ci.CampaignItemName, ci.CampaignItemType,ci.CampaignItemFormatType,
				b.EmailFromName,b.EmailFrom,b.ReplyTo,b.SendTime,ci.CompletedStep,ci.CreatedUserID,ci.CreatedDate,ci.UpdatedUserID, ci.UpdatedDate, 
				MAX(case when ci.CampaignItemType = 'AB' then '-- Multiple Subject lines --' else b.EmailSubject end) as emailsubject,
				case when COUNT(citb.GroupID) >= 0 then MAX(g.GroupName) else ' -- Multi Tests Groups --' end  as GroupName
		FROM 
				Campaign cp WITH (NOLOCK)
				join CampaignItem ci WITH (NOLOCK) on cp.CampaignID = ci.CampaignID
				JOIN CampaignItemTestBlast citb WITH (NOLOCK) ON ci.CampaignItemID = citb.CampaignItemID
				JOIN Groups g WITH (NOLOCK) ON citb.groupID = g.groupID join
				Blast b  WITH (NOLOCK) on b.blastID = citb.blastID
		WHERE 
				cp.CustomerID = @CustomerID  and 
				isnull(ci.IsHidden, 0) = 0 and
				isnull(citb.IsDeleted, 0) = 0 and 
				isnull(cp.IsDeleted, 0) = 0 and
				isnull(ci.IsDeleted, 0) = 0 and
				b.statuscode = @Status
		group by 
				cp.CustomerID, cp.CampaignID, ci.CampaignItemID, ci.CampaignItemName, ci.CampaignItemType,ci.CampaignItemFormatType,
				b.EmailFromName,b.EmailFrom,b.ReplyTo,b.SendTime,ci.CompletedStep,ci.CreatedUserID,ci.CreatedDate,ci.UpdatedUserID, ci.UpdatedDate
		) q1
		order by q1.UpdatedDate DESC
	END
	
