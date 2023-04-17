--exec sp_LoadStatistics '20230101','20230430'

if object_id('sp_LoadStatistics') is not null drop proc sp_LoadStatistics
go
create proc sp_LoadStatistics
	@FDate datetime='',
	@TDate datetime='',
	@Customer varchar(200)=''
as
/*
declare	   	   
	@FDate datetime='20230101',
	@TDate datetime='20231231',
	@Customer varchar(200)=''
--*/
begin
	set nocount on
	if isnull(@Customer,'')='' set @Customer='%'

	if object_id('tempdb..#bhd') is not null drop table #bhd
	select s.*, c.Code, c.[Name], c.[Address], c.TaxCode, c.Phone
	into #bhd
	from PR_Statistics s left join PR_Customer c on c.Code=s.Customer
	where RequestTime between @FDate and @TDate and s.Customer like @Customer

	select [Level]=case when stt is null then 0 when isnull(GroupCol,'')='' then -1 else 1 end, stt=row_number()over(order by GroupCol, stt)
		, GroupCol, CustomerCode, CustomerName, ChargeCharacters, RequestTime
		, IpAddress, UserAgent, Country, Region, City, ISP
	from (
		select stt=0, GroupCol=''
			, CustomerCode='Total', CustomerName=''
			, ChargeCharacters=sum(isnull(t.ChargeCharacters,0)), RequestTime=null
			, IpAddress='', UserAgent='', Country='', Region='', City='', ISP=''
		from #bhd t	
		having count(t.Customer)>0
		union all
		select stt=cast(null as int), GroupCol=t.Customer
			, CustomerCode=t.Customer, CustomerName=max(t.[Name])
			, ChargeCharacters=sum(isnull(t.ChargeCharacters,0)), RequestTime=null
			, IpAddress='', UserAgent='', Country='', Region='', City='', ISP=''
		from #bhd t
		group by t.Customer
		union all
		select stt=row_number()over(order by t.MainID), GroupCol=t.Customer
			, CustomerCode=t.Customer, CustomerName=t.[Name]
			, t.ChargeCharacters, t.RequestTime
			, t.IpAddress, t.UserAgent, t.Country, t.Region, t.City, t.ISP
		from #bhd t
	) result
	--where stt is not null and stt>0 ---for debug
	order by GroupCol, stt, [Level]
	if object_id('tempdb..#bhd') is not null drop table #bhd
end
