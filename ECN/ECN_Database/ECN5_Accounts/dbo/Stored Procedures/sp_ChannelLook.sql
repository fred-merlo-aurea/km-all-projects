CREATE procedure [dbo].[sp_ChannelLook]    
(     
 @channelID int, 
 @CustomerID varchar(2000),   
 @fromdt varchar(10),    
 @todt varchar(10),
 @testblast varchar(1)    
)    
as    
Begin    

	Set nocount on    
    
--declare 
--            @channelID int = 131, 
--            @CustomerID varchar(2000) = '',   
--            @fromdt varchar(10) = '1/1/2016',    
--            @todt varchar(10) = '3/31/2016',
--            @testblast varchar(1) = 'N'   

   declare @CustIDs table
   (
		CustomerID int
   )

   if len(@CustomerID) > 0
   BEGIN
        INSERT INTO @CustIDs
        SELECT convert(int,items)
        FROM fn_Split( @CustomerID, ',')
   END
   ELSE
   BEGIN
		INSERT INTO @CustIDs
		SELECT CustomerID 
		from Customer c with(nolock) 
		where c.BaseChannelID = @channelID
   END
   
   declare @TrigBlasts table(BlastID int)
   INSERT INTO @TrigBlasts
   Select BlastID
   FROM ECN5_COMMUNICATOR..Blast b with(nolock)
   JOIN @CustIDs c on b.CustomerID = c.CustomerID  
   WHERE b.BlastType in ('Layout','NoOpen') and b.StatusCode in ('Sent', 'Deleted')
   AND cast(b.SendTime as date) >= @fromdt
   
   
    declare @tbl    TABLE
       (  
        basechannelID int ,   
        basechannelName varchar(100),  
        customerID int,  
        CustomerName varchar(100),  
        
        JanCount int,  
        FebCount int,  
        MarCount int,  
        AprCount int,  
        MayCount int ,  
        JunCount int ,  
        JulCount int ,  
        AugCount int ,  
        SepCount int ,  
        OctCount int ,  
        NovCount int ,  
        DecCount int    
       ) 
       
		insert into @tbl
      select  [BaseChannel].BaseChannelID,    
            [BaseChannel].BaseChannelName,  
            [Customer].CustomerID,          
            [Customer].CustomerName,     
            convert(int,sum(case when month(sendtime) = 1 then sendtotal else 0 end)) as JanCount,    
            convert(int,sum(case when month(sendtime) = 2 then sendtotal else 0 end)) as FebCount,    
            convert(int,sum(case when month(sendtime) = 3 then sendtotal else 0 end)) as MarCount,    
            convert(int,sum(case when month(sendtime) = 4 then sendtotal else 0 end)) as AprCount,    
            convert(int,sum(case when month(sendtime) = 5 then sendtotal else 0 end)) as MayCount,    
            convert(int,sum(case when month(sendtime) = 6 then sendtotal else 0 end)) as JunCount,    
            convert(int,sum(case when month(sendtime) = 7 then sendtotal else 0 end)) as JulCount,    
            convert(int,sum(case when month(sendtime) = 8 then sendtotal else 0 end)) as AugCount,    
            convert(int,sum(case when month(sendtime) = 9 then sendtotal else 0 end)) as SepCount,    
            convert(int,sum(case when month(sendtime) = 10 then sendtotal else 0 end)) as OctCount,    
            convert(int,sum(case when month(sendtime) = 11 then sendtotal else 0 end)) as NovCount,    
            convert(int,sum(case when month(sendtime) = 12 then sendtotal else 0 end)) as DecCount    
      From     
         [BaseChannel] 
         join [Customer] with(nolock) on [BaseChannel].BaseChannelID = [Customer].BaseChannelID 
         join @CustIDs c on [Customer].CustomerID = c.CustomerID
         join [ECN5_COMMUNICATOR].[DBO].[BLAST] ComB with(nolock) on ComB.CustomerID = c.CustomerID    
      where  
			ComB.TestBlast = case when @testblast = 'Y' then 'Y' else Comb.TestBlast end
			AND CAST(SendTime as date) between @fromdt and @todt
			and testblast = @testblast and StatusCode in ('sent','deleted') and BlastType not in ('Layout','NoOpen')
      Group by [BaseChannel].BaseChannelID,    
        [BaseChannel].BaseChannelName,    
         [Customer].CustomerID,             
         [Customer].CustomerName 

      UNION ALL
      
      select  [BaseChannel].BaseChannelID,    
            [BaseChannel].BaseChannelName,  
            [Customer].CustomerID,          
            [Customer].CustomerName,     
            case when month(bas.sendtime) = 1 then count(bas.SendID) else 0 end as JanCount,    
            case when month(bas.sendtime) = 2 then count(bas.SendID) else 0 end as FebCount,    
            case when month(bas.sendtime) = 3 then count(bas.SendID) else 0 end as MarCount,    
            case when month(bas.sendtime) = 4 then count(bas.SendID) else 0 end as AprCount,    
            case when month(bas.sendtime) = 5 then count(bas.SendID) else 0 end as MayCount,    
            case when month(bas.sendtime) = 6 then count(bas.SendID) else 0 end as JunCount,    
            case when month(bas.sendtime) = 7 then count(bas.SendID) else 0 end as JulCount,    
            case when month(bas.sendtime) = 8 then count(bas.SendID) else 0 end as AugCount,    
            case when month(bas.sendtime) = 9 then count(bas.SendID) else 0 end as SepCount,    
            case when month(bas.sendtime) = 10 then count(bas.SendID)else 0 end as OctCount,    
            case when month(bas.sendtime) = 11 then count(bas.SendID) else 0 end as NovCount,    
            case when month(bas.sendtime) = 12 then count(bas.SendID) else 0 end as DecCount    
      From     
         [BaseChannel] 
         join [Customer] with(nolock) on [BaseChannel].BaseChannelID = [Customer].BaseChannelID 
         join [ECN5_COMMUNICATOR].[DBO].[BLAST] ComB with(nolock) on ComB.CustomerID = [Customer].CustomerID 
         JOIN @TrigBlasts t on ComB.BlastID = t.BlastID  
         join [ECN_ACTIVITY].[DBO].[BLASTACTIVITYSENDS] bas with(nolock) on t.BlastID = bas.BlastID  
      where cast(bas.SendTime as date) between @fromdt and @todt
		AND ComB.TestBlast = case when @testblast = 'Y' then 'Y' else 'N' end
      Group by [BaseChannel].BaseChannelID,    
         [BaseChannel].BaseChannelName,    
         [Customer].CustomerID,             
         [Customer].CustomerName,
         bas.SendTime
         
         order by BaseChannelName, CustomerName  
      
     select BaseChannelID,    
         BaseChannelName,    
         CustomerID,             
         CustomerName    ,
         SUM(JanCount) as JanCount,
            sum(FebCount) as FebCount,    
            sum(MarCount) as MarCount,    
            sum(AprCount) as AprCount,    
            sum(MayCount) as MayCount,    
            sum(JunCount) as JunCount,    
            sum(JulCount) as JulCount,    
            sum(AugCount) as AugCount,    
            sum(SepCount) as SepCount,    
            sum(OctCount) as OctCount,    
            sum(NovCount) as NovCount,    
            sum(DecCount) as DecCount   
     from @tbl
     Group by  BaseChannelID,    
         BaseChannelName,    
         CustomerID,             
         CustomerName  
      order by BaseChannelName, CustomerName  

    
End
