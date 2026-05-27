declare @fromDate datetime,@endDate datetime,@deviceName nvarchar(50)
set @fromDate='2013-03-24';
set @endDate='2014-03-28';
set @deviceName='c001'


declare cursor1 cursor for  SELECT  createdat,[RequestStateCommandReplyDataLog_State]
  from dbo.ReceivedDataLogs 
  where devicename=@deviceName
  and CreatedAt between @fromDate and @endDate
  order by CreatedAt asc
  
declare @table1 table(
	deviceName nvarchar(50) not null,
	fromStatus int not null,
	fromDate datetime not null,
	toStatus int null,
	toDate datetime null,
	seconds int null
)

OPEN cursor1;

declare @status int,@date datetime2,@lastStatus int,@lastDate datetime2,@insert bit

FETCH NEXT FROM cursor1 into @date,@status
WHILE @@FETCH_STATUS = 0
BEGIN
	if (@lastDate is null and @lastStatus is null) or  @insert=1
	begin
		insert into @table1 (deviceName,fromStatus,fromDate,toStatus,toDate,seconds) values(@deviceName,@status,@date,null,null,null);	
		set @insert=0;	
		set @lastStatus=@status;
		if @lastDate is null
		begin
			set @lastDate=@date;
		end;
	end;
	else
	begin	
		if	@status<>@lastStatus
		begin
			update @table1 set fromDate=@lastDate,toStatus=@status,toDate=@date,seconds=DATEDIFF(second,fromDate,@date) where deviceName=@deviceName and toStatus is null and toDate is null and fromStatus=@lastStatus
			set @insert=1;			
				
			set @lastStatus=@status;
			set @lastDate=@date;
		end;
		else
		begin
			set @insert=0;
		end;
	end;
	
    FETCH NEXT FROM cursor1 into @date,@status
END;

CLOSE cursor1;
DEALLOCATE cursor1;

select * from @table1 where toStatus is not null