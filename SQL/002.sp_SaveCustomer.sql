if object_id('sp_SaveCustomer') is not null drop proc sp_SaveCustomer
go
create proc sp_SaveCustomer
	@Mode int=0,
	@HasError bit out,
	@Xml xml=''
as
/*
declare	   
	@Mode int=0,
	@HasError int=0, 
	@Xml xml=''
--*/
begin
	set nocount on
	if @Mode is null set @Mode=0
	if object_id('tempdb..#MS') is not null drop table #MS
	begin print 'Get data'		 		
		declare @iXml int
		exec sp_xml_preparedocument @iXml out, @Xml
		select Code, [Name], [Address], TaxCode, Phone, Remark
		into #MS
		from openxml(@iXml,'//MS',2)
		with (
			Code varchar(200), [Name] nvarchar(2000), [Address] nvarchar(2000),
			TaxCode nvarchar(2000), Phone nvarchar(2000), Remark nvarchar(2000)
		)[MS]
		--select * from #MS --return	---for debug
	end
	begin try
		begin tran CustTran	
			if @Mode=2 begin
				delete PR_Customer where Code in (select Code from #MS)
				goto EndStore
			end			
			if exists(select 1 from #MS t inner join PR_Customer c on c.Code=t.Code) set @Mode=1 else set @Mode=0
			if @Mode=0 begin
				insert into PR_Customer(Code,[Name],[Address],TaxCode,Phone,Remark,InputTime)
				select Code,[Name],[Address],TaxCode,Phone,Remark,getdate()
				from #MS
			end
			if @Mode=1 begin
				update m
				set m.[Name]=t.[Name], m.[Address]=t.[Address], m.TaxCode=t.TaxCode, m.Phone=t.Phone
					, m.Remark=t.Remark, m.ModifyTime=getdate()
				from PR_Customer m inner join #MS t on t.Code=m.Code
				where m.[Name]<>t.[Name] or m.[Address]<>t.[Address]
					or m.TaxCode<>t.TaxCode or m.Phone<>t.Phone
					or m.Remark<>t.Remark
			end
			EndStore:
			print 'Save customer success.'
			set @HasError=0
		commit tran CustTran
	end try
	begin catch
		print 'Save customer has error!'
		set @HasError=1
	end catch 
	if object_id('tempdb..#MS') is not null drop table #MS
end