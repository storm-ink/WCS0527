declare @fromDate datetime,@endDate datetime,@deviceName nvarchar(50)
set @fromDate='2014-03-24';
set @endDate='2014-03-25';
set @deviceName='c001';
 
with t (CreatedAt, rState, rownumber) 
as 
(
  SELECT  createdat, [RequestStateCommandReplyDataLog_State],ROW_NUMBER() over ( order by createdat asc)
  from dbo.ReceivedDataLogs 
  where devicename=@deviceName
  and CreatedAt between @fromDate and @endDate
)
select t1.CreatedAt time1, t2.CreatedAt time2,t1.rState state1,   t2.rState state2, DATEDIFF(second, t1.CreatedAt, t2.CreatedAt) s, t1.rownumber n1, t2.rownumber n2 
from t t1
inner join t t2 on t1.rownumber = t2.rownumber - 1
where t1.rownumber % 2 = 1;
