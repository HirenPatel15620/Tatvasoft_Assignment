Create Table Products(
ProductID int Primary Key identity(1,1),
ProductName varchar(40) not null,
SupplierID int not null,
CategoryID int not null,
QuantityPerUnit varchar(40) not null,
UnitPrice decimal(10,4) not null,
UnitsInStock smallint not null,
UnitsOnOrder smallint,
ReorderLevel smallint not null,
Discontinued bit not null
)


select * from Products

--1
select ProductID,ProductName,UnitPrice from Products 
where UnitPrice<20.00
--2
select ProductID,ProductName,UnitPrice from Products 
where UnitPrice<25.00 and UnitPrice>15.00
--3
select ProductName,UnitPrice from Products 
where UnitPrice > (select AVG(UnitPrice) from Products)
--4
select top 10 UnitPrice,ProductName from Products
order by UnitPrice desc
--5
select Discontinued,count(Discontinued) as ProductCount from Products
group by Discontinued
--6
select ProductName,UnitsInStock,UnitsOnOrder from Products 
where UnitsInStock<UnitsOnOrder