if object_id('sp_SaveStatistics') is not null drop proc sp_SaveStatistics
go
create proc sp_SaveStatistics
	@HasError bit out,
	@Xml xml=''
as
/*
declare	   
	@HasError int=0, 
	@Xml xml=''
--*/
begin
	set nocount on
	if object_id('tempdb..#MS') is not null drop table #MS
	begin print 'Get data'
		declare @iXml int
		exec sp_xml_preparedocument @iXml out, @Xml
		select
			ChargeCharacters=try_cast(ChargeCharacters as int)
			--, RequestTime=try_cast(RequestTime as datetime2)
			, Customer, IpAddress, UserAgent, Country, Region, City, ISP, Remark
		into #MS
		from openxml(@iXml,'//MS',2)
		with (
			ChargeCharacters int, RequestTime datetime2
			, Customer varchar(200), IpAddress varchar(200), UserAgent nvarchar(2000)
			, Country nvarchar(2000), Region nvarchar(2000), City nvarchar(2000), ISP nvarchar(2000)
			, Remark nvarchar(2000)
		)[MS]
		--select * from #MS --return	---for debug
	end
	begin try
		begin tran StatisticsTran		
			if not exists(select 1 from #MS t inner join PR_Customer c on c.Code=t.Customer) begin
				insert into PR_Customer(Code,[Name],Remark,InputTime)
				select Customer,Customer,'Auto inserted by ws',getdate()
				from #MS
			end	
			insert into PR_Statistics(ChargeCharacters, RequestTime
				, Customer, IpAddress, UserAgent, Country, Region, City, ISP, Remark)
			select ChargeCharacters, getdate()
				, Customer, IpAddress, UserAgent, Country, Region, City, ISP, Remark
			from #MS
			where isnull(Customer,'')<>''  
			print N'Save Statistics success.'
			set @HasError=0
		commit tran StatisticsTran
	end try
	begin catch
		print N'Save Statistics has error!'
		set @HasError=1
	end catch 
	if object_id('tempdb..#MS') is not null drop table #MS
end