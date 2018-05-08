--IF EXISTS (SELECT 1 FROM Sysobjects where name = 'job_Standard_RollUpToMaster')
--DROP Proc job_Standard_RollUpToMaster
--GO

--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO



create procedure job_Standard_RollUpToMaster
	@ProcessCode varchar(50),
	@SourceFileID int,
	@MailPermissionOverRide bit = 'false',
	@FaxPermissionOverRide bit = 'false',
	@PhonePermissionOverRide bit = 'false',
	@OtherProductsPermissionOverRide bit = 'false',
	@ThirdPartyPermissionOverRide bit = 'false',
	@EmailRenewPermissionOverRide bit = 'false',
	@TextPermissionOverRide bit = 'false',
	@updateEmail bit = 'true',
	@updatePhone bit = 'true',
	@updateFax bit = 'true',
	@updateMobile bit = 'true'
AS
BEGIN

	SET NOCOUNT ON
	
	Print('Job_Standard_RollupToMaster - this stored proc code is obsolete as it is handled in job_datamatching')

	--if(@MailPermissionOverRide) = 'true'
	--	Begin
	--		-- Update MailPermission to 0 if any of the subordinate records have a 0
	--		update sf
	--			set MailPermission = 0
	--			from SubscriberFinal sf
	--				inner join (select distinct igrp_no
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and MailPermission = 0 and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on sf.IGrp_No = iSt.IGrp_No
	--			where sf.IGrp_Rank = 'M' and sf.MailPermission = 1 and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	---- Any record that isMailable = 1, set MailPermission to 0
	--update SubscriberFinal
	--set isMailable = 1
	--where ((CountryID not in (1,2) and isnull([address],'')!='' ) or (CountryID in (1,2) and IsLatLonValid = 1)) and  ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--if(@FaxPermissionOverRide) = 'true'
	--	begin
	--		-- Set FaxPermission to 0 if any S records are 0
	--		update sf
	--			set FaxPermission = 0
	--			from SubscriberFinal sf
	--				inner join (select igrp_no
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and FaxPermission = 0 and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on sf.IGrp_No = iSt.IGrp_No
	--			where sf.IGrp_Rank = 'M' and sf.FaxPermission = 1 and sf.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@PhonePermissionOverRide) = 'true'
	--	begin
	--		-- Set PhonePermission to 0 if any subordinate records are 0	
	--		update sf
	--			set PhonePermission = 0
	--			from SubscriberFinal sf
	--				inner join (select igrp_no
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and PhonePermission = 0 and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on sf.IGrp_No = iSt.IGrp_No
	--			where sf.IGrp_Rank = 'M' and sf.PhonePermission = 1 and sf.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@OtherProductsPermissionOverRide) = 'true'
	--	begin
	--		-- Set OtherProductsPermission to 0 if any subordinate records are 0	
	--		update sf
	--			set OtherProductsPermission = 0
	--			from SubscriberFinal sf
	--				inner join (select igrp_no
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and OtherProductsPermission = 0 and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on sf.IGrp_No = iSt.IGrp_No
	--			where sf.IGrp_Rank = 'M' and sf.OtherProductsPermission = 1 and sf.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@ThirdPartyPermissionOverRide) = 'true'
	--	begin
	--		-- Set ThirdPartyPermission to 0 if any subordinate records are 0
	--		update sf
	--			set ThirdPartyPermission = 0
	--			from SubscriberFinal sf
	--				inner join (select igrp_no
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and ThirdPartyPermission = 0 and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on sf.IGrp_No = iSt.IGrp_No
	--			where sf.IGrp_Rank = 'M' and sf.ThirdPartyPermission = 1 and sf.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@EmailRenewPermissionOverRide) = 'true'
	--	begin
	--		-- Set EmailRenewPermission to 0 if any subordinate records are 0	
	--		update sf
	--			set EmailRenewPermission = 0
	--			from SubscriberFinal sf
	--				inner join (select igrp_no
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and EmailRenewPermission = 0 and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on sf.IGrp_No = iSt.IGrp_No
	--			where sf.IGrp_Rank = 'M' and sf.EmailRenewPermission = 1 and sf.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@TextPermissionOverRide) = 'true'
	--	begin
	--		-- Set TextPermission to 0 if any subordinate records are 0	
	--		update sf
	--			set TextPermission = 0
	--			from SubscriberFinal sf
	--				inner join (select igrp_no
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and TextPermission = 0 and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on sf.IGrp_No = iSt.IGrp_No
	--			where sf.IGrp_Rank = 'M' and sf.TextPermission = 1 and sf.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@updateEmail) = 'true'
	--	begin
	--	-- Updates Email where master record is null and subordinate Email is not null
	--		update SF
	--			set Email = iSt.Email
	--			from SubscriberFinal SF
	--				inner join (select igrp_no,Email
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and ISNULL(Email,'')!='' and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on SF.IGrp_No = iSt.IGrp_No
	--			where SF.IGrp_Rank = 'M' and ISNULL(SF.Email,'')='' and SF.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@updatePhone) = 'true'
	--	begin
	--		-- Updates Phone where master record is null and subordinate Phone is not null	
	--		update SF
	--			set Phone = iSt.Phone
	--			from SubscriberFinal SF
	--				inner join (select igrp_no,Phone
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and ISNULL(Phone,'')!='' and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on SF.IGrp_No = iSt.IGrp_No
	--			where SF.IGrp_Rank = 'M' and ISNULL(SF.Phone,'')='' and SF.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@updateFax) = 'true'
	--	begin
	--		-- Updates Fax where master record is null and subordinate Fax is not null	
	--		update SF
	--			set Fax = iSt.Fax
	--			from SubscriberFinal SF
	--				inner join (select igrp_no,Fax
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and ISNULL(Fax,'')!='' and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on SF.IGrp_No = iSt.IGrp_No
	--			where SF.IGrp_Rank = 'M' and ISNULL(SF.Fax,'')='' and SF.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
	--if(@updateMobile) = 'true'
	--	begin
	--		-- Updates Mobile where master record is null and subordinate Mobile is not null	
	--		update SF
	--			set Mobile = iSt.Mobile
	--			from SubscriberFinal SF
	--				inner join (select igrp_no,Mobile
	--							from SubscriberFinal
	--							where IGrp_Rank = 'S' and ISNULL(Mobile,'')!='' and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId) as iSt
	--				on SF.IGrp_No = iSt.IGrp_No
	--			where SF.IGrp_Rank = 'M' and ISNULL(SF.Mobile,'')='' and SF.ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--		-- script fix per Sunil
	--		--update sf
	--		--set MailPermission = 'true'
	--		--from SubscriberFinal sf
	--		--where sf.IsLatLonValid = 'true' and sf.MailPermission = 'false' and ProcessCode = @ProcessCode and SourceFileId = @SourceFileId
	--	end
END
