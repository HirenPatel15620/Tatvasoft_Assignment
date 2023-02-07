create table department(
dept_id int not null primary key identity(1,1),
dept_name varchar(20) not null
)

insert into department
values('HR')
insert into department 
values('Technical')
insert into department
values('Network')
insert into department
values('Marketing')

select * from department


create table Employee(
emp_id int not null primary key identity(1,1),
dept_id int foreign key references department(dept_id),
mngr_id int not null,
emp_name varchar(20) not null,
salary int not null
)

insert into Employee
values(1,12,'Vivek',20000)
insert into Employee
values(2,1,'Jigar',10000)
insert into Employee
values(1,12,'Jaydeep',40000)
insert into Employee
values(3,4,'Mayur',10000)
insert into Employee
values(4,8,'Darshan',50000)
insert into Employee
values(2,14,'Hiren',20000)
insert into Employee
values(4,8,'Kishan',15000)
insert into Employee
values(4,5,'Dhrumil',5000)

select * from Employee



--1
Select d.dept_id,d.dept_name as department_name ,max(e.salary) as max_salary
From Employee as e
Join Department as d On e.dept_id=d.dept_id
Group by d.dept_name,d.dept_id 


--2
SELECT d.dept_name, COUNT (e.emp_id) AS employees 
FROM Employee as e
JOIN Department as d ON e.dept_id= d.dept_id
GROUP BY dept_name
Having COUNT (e.emp_id)<3


--3
SELECT d.dept_name, COUNT (e.emp_id) AS employees
FROM Employee as e
Right JOIN Department as d ON e.dept_id= d.dept_id
GROUP BY dept_name;


--4
SELECT d.dept_name,SUM(e.salary)AS total_salary
FROM Employee as e
Right JOIN Department as d
ON e.dept_id= d.dept_id
GROUP BY dept_name;