CREATE PROCEDURE job_Stamats_CopyProfilesToGroup
AS
Begin
	--PubId=162 / Design Flash (JF) / Source Group 169237
	
		EXEC sp_copyProfilesToGroup 169237, 164677, 0
		EXEC sp_copyProfilesToGroup 169237, 157800, 0
		EXEC sp_copyProfilesToGroup 169237, 157797, 0
		
	--PubId=166 / Webinar Promotions (JF) / Source Group 169345

		EXEC sp_copyProfilesToGroup 169345, 168580, 0
	
	--PubId=167 
	
		--Facility Systems Solutions (JF) / Source Group 169376

		EXEC sp_copyProfilesToGroup 169376, 164680, 0
		EXEC sp_copyProfilesToGroup 169376, 157773, 0
		EXEC sp_copyProfilesToGroup 169376, 157769, 0

		--Greener Facilities (JF) / Source Group 169379
		
		EXEC sp_copyProfilesToGroup 169379, 164681, 0
		EXEC sp_copyProfilesToGroup 169379, 157775, 0
		EXEC sp_copyProfilesToGroup 169379, 157771, 0

		--Roofing Results (JF) / Source Group 169380
		
		EXEC sp_copyProfilesToGroup 169380, 164682, 0
		EXEC sp_copyProfilesToGroup 169380, 157776, 0
		EXEC sp_copyProfilesToGroup 169380, 157772, 0
		
		--Energy Manager (JF) / Source Group 169383
		
		EXEC sp_copyProfilesToGroup 169383, 164679, 0
		EXEC sp_copyProfilesToGroup 169383, 157774, 0
		EXEC sp_copyProfilesToGroup 169383, 157770 , 0

		--Marketing Promotions (JF) / Source Group 169385
		
		EXEC sp_copyProfilesToGroup 169385, 157766, 0
		EXEC sp_copyProfilesToGroup 169385, 157767, 0

		--Surveys (JF) / Source Group 169388
		
		EXEC sp_copyProfilesToGroup 169388, 157777, 0
		EXEC sp_copyProfilesToGroup 169388, 157778, 0

		--Digital Alerts (JF)/ Source Group 169389
		
		EXEC sp_copyProfilesToGroup 169389, 157764, 0
		EXEC sp_copyProfilesToGroup 169389, 157765, 0

End

