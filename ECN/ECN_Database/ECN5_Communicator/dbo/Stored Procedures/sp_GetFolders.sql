CREATE proc [dbo].[sp_GetFolders]
(
			@customerID Int,
		 	@userID int,
			@FolderType varchar(10)
)
as
Begin
	declare @Folder Table 
	(
		FolderID int,
		FN  varchar(100),
		ParentID int,
		IsSystem bit,
		levelno int
	)
		
	/*if  (exists (select groupID from usergroups where userID = @userID) and @FolderType ='GRP')
	Begin
	
		declare @i int
		select @i = 0
	
		insert into @folder 
		SELECT distinct f.FolderID, f.FolderName, f.parentID, f.SystemFolder, 1
		FROM 	
					[FOLDER] f left outer join groups g on f.folderID = g.folderID join 
					usergroups u on u.groupID = g.groupID
		WHERE 
					f.CustomerID = @customerID AND FolderType = @FolderType and userID = @userID
	
		while @@rowcount > 0
		begin
		     select @i = @i + 1
		
			 insert @folder
			SELECT distinct f.FolderID, f.FolderName, f.parentID, f.SystemFolder, @i+1
			FROM 	   
						[FOLDER] f join 
						@folder f1 on F.folderID = f1.parentID
			WHERE  
						F.CustomerID = @customerID AND F.FolderType =@FolderType and f1.levelno= @i and
						not exists (select folderID from @folder where folderID = f.FolderID)
					 
	
		end
	
	End
	else
	Begin*/
		insert into @Folder
		SELECT FolderID, FolderName, parentID, IsSystem, 0
		FROM [FOLDER] WHERE CustomerID = @customerID  AND FolderType = @FolderType
	--End
	
	select  FolderID, CASE WHEN IsSystem = 1 THEN '<Font color=#FF0000>'+FN+'</font>'  ELSE FN END as FolderName, ParentID  from @Folder
	order by parentID, FN

End
