create table EngineLog
(
	EngineLogId int identity(1,1),
	ClientId int not null,
	Engine varchar(50) not null, 
	CurrentStatus varchar(max) null,
	LastRefreshDate date null,
	LastRefreshTime time(7) null, 
	IsRunning bit null, 
	LastRunningCheckDate date null,
	LastRunningCheckTime time(7) null,
	DateUpdated DateTime not null,
	CONSTRAINT [PK_EngineLog_ClientId_Engine] PRIMARY KEY CLUSTERED (ClientId, Engine ASC),  
)
