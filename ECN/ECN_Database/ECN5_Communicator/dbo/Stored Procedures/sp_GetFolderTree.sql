Create PROCEDURE [dbo].[sp_GetFolderTree]  
(
	@customerID Int,
	@userID int,
	@FolderType varchar(10) 
) as 
Begin
	declare @space varchar(500)

	set @space = '&nbsp;'

	declare @Folder Table  (
		FolderID int,
		FolderName  varchar(800),
		FolderDescription varchar(500),
		ParentID int,
		FolderType char(10),
		IsSystem bit,
		levelno int,
		sort varchar(100),
		DateCreated datetime,
		Items int
	)
		
declare @i int
select @i = 1

if @FolderType = 'GRP'  
	Begin  
		insert into @folder 
		SELECT f.FolderID, @space+'<sub><img src=''/ecn.images/images/L.gif''></sub>&nbsp;<sub><img src=''/ecn.images/images/Lg_folder_Yel.gif''></sub>&nbsp;&nbsp;'+f.FolderName, f.FolderDescription, f.parentID, @FolderType as FolderType, f.IsSystem, 2, f.FolderID, f.CreatedDate, COUNT(g.groupID)
		FROM 	
			[FOLDER] f LEFT OUTER JOIN groups g ON f.folderID = g.folderID
		WHERE 
			f.CustomerID = @customerID AND FolderType = @FolderType AND parentID = 0
		GROUP BY f.FolderID, f.FolderName, f.FolderDescription, f.parentID, f.IsSystem, f.FolderID, f.CreatedDate

		while @@rowcount > 0
		begin
		     select @i = @i + 1
			set @space = @space + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'
			insert @folder
			SELECT f.FolderID, @space + '<sub><img src=''/ecn.images/images/L.gif''></sub>&nbsp;<sub><img src=''/ecn.images/images/Sm_folder_Yel.gif''></sub>&nbsp;'+ f.FolderName,f.FolderDescription, f.parentID, @FolderType as FolderType, f.IsSystem, @i+1, Convert(varchar,f1.sort)+'-'+Convert(varchar,f.FolderID), f.CreatedDate, COUNT(g.groupID)
			FROM 	   
				[FOLDER] f join @folder f1 on F1.folderID = f.parentID  LEFT OUTER JOIN groups g ON f.folderID = g.folderID
			WHERE  
				F.CustomerID = @customerID AND F.FolderType =@FolderType and f1.levelno= @i and
				not exists (select folderID from @folder where folderID = f.FolderID)
			GROUP BY f.FolderID, f.FolderName,f.FolderDescription, f.parentID, f.IsSystem, Convert(varchar,f1.sort)+'-'+Convert(varchar,f.FolderID), f.CreatedDate
		end
	END
else if @FolderType = 'CNT'  
	Begin  
		insert into @folder 
		SELECT f.FolderID, @space+'<sub><img src=''/ecn.images/images/L.gif''></sub>&nbsp;<sub><img src=''/ecn.images/images/Lg_folder_Yel.gif''></sub>&nbsp;&nbsp;'+f.FolderName, f.FolderDescription, f.parentID, @FolderType as FolderType, f.IsSystem, 2, f.FolderID, f.CreatedDate, (COUNT(distinct c.contentID) + COUNT(distinct l.LayoutID))
		FROM 	
			[FOLDER] f LEFT OUTER JOIN Content c ON f.folderID = c.folderID LEFT OUTER JOIN [LAYOUT] l on f.FolderID = l.FolderID 
		WHERE 
			f.CustomerID = @customerID AND FolderType = @FolderType AND parentID = 0
		GROUP BY f.FolderID, f.FolderName, f.FolderDescription, f.parentID, f.IsSystem, f.FolderID, f.CreatedDate

		while @@rowcount > 0
		begin
		     select @i = @i + 1
			set @space = @space + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'
			insert @folder
			SELECT f.FolderID, @space +'<sub><img src=''/ecn.images/images/L.gif''></sub>&nbsp;<sub><img src=''/ecn.images/images/Sm_folder_Yel.gif''></sub>&nbsp;'+ f.FolderName,f.FolderDescription, f.parentID, @FolderType as FolderType, f.IsSystem, @i+1, Convert(varchar,f1.sort)+'-'+Convert(varchar,f.FolderID), f.CreatedDate,(COUNT(distinct c.contentID) + COUNT(distinct l.LayoutID))
			FROM 	   
				[FOLDER] f join @folder f1 on F1.folderID = f.parentID  LEFT OUTER JOIN Content c ON f.folderID = c.folderID LEFT OUTER JOIN [LAYOUT] l on f.FolderID = l.FolderID 
			WHERE  
				F.CustomerID = @customerID AND F.FolderType =@FolderType and f1.levelno= @i and
				not exists (select folderID from @folder where folderID = f.FolderID)
			GROUP BY f.FolderID, f.FolderName,f.FolderDescription, f.parentID, f.IsSystem, Convert(varchar,f1.sort)+'-'+Convert(varchar,f.FolderID), f.CreatedDate
		end
	END

	select FolderID,FolderName ,FolderDescription,ParentID,FolderType, IsSystem,levelno,sort,DateCreated ,Items from @folder 
	UNION
	select 0 as 'FolderID', '&nbsp;<Font color=#FF0000><b>ROOT FOLDER</b></font>' AS 'FolderName', '<b>ROOT FOLDER</b>' AS 'FolderDescription', 0 AS 'ParentID', @FolderType AS 'FolderType', 1, 1,'0', null as 'DateCreated', ''
	order by sort, levelno
End
