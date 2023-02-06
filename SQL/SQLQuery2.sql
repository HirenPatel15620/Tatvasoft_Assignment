-- %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

--for smaple database 

-- *****************************************************************************************************


create table salesman(
Salesman_ID int primary key,
Name varchar(30),
City varchar(15),
Commission float
);

create table customer(
Customer_ID int primary key,
Cust_Name varchar(30),
City varchar(15),
Grade int,
Salesman_ID int
);

create table orders(
Ord_NO int primary key,
Purch_Amt float,
Ord_Date date,
Customer_ID int,
Salesman_ID int,
foreign key(Customer_ID) references customer(customer_id),
foreign key(salesman_ID) references salesman(salesman_id)
);

insert into salesman values(
5001,'James Hoog','New York',0.15),
(5002,'Nail Knite','Paris',0.13),
(5005,'Pit Alex','London',0.11),
(5006,'Mc Lyon','Paris',0.14),
(5003,'Lauson Hen','San Jose',0.12),
(5007,'Paul Adam','Rome',0.13);

insert into customer values(3002,'Nick Rimando','New York',100,5001),
(3005,'Graham Zusi','California',200,5002),
(3001,'Brad Guzan','London',100,5005),
(3004,'Fabian Johns','Paris',300,5006),
(3007,'Brad Davis','New York',200,5001),
(3009,'Geoff Camero','Berlin',100,5003),
(3008,'Julian Green','London',300,5002),
(3003,'Jozy Altidor','Moncow',200,5007);

insert into orders values(70001,150.5,'2012-10-05',3005,5002),
(70009,270.65,'2012-09-10',3001,5005),
(70002,65.26,'2012-10-05',3002,5001),
(70004,110.5,'2012-08-17',3009,5003),
(70007,948.5,'2012-09-10',3005,5002),
(70005,2400.6,'2012-07-27',3007,5001),
(70008,5760.0,'2012-09-10',3002,5001),
(70010,1983.43,'2012-10-10',3004,5006),
(70003,2480.4,'2012-10-10',3009,5003),
(70012,250.45,'2012-06-27',3008,5002),
(70011,75.29,'2012-08-17',3003,5007),
(70013,3045.6,'2012-04-25',3002,5001);






--1
select salesman.name as "salesname",customer.Cust_Name , salesman.City from salesman,customer
where salesman.city=customer.city



--2
select orders.Ord_NO ,orders.Purch_Amt ,customer.Cust_Name,customer.City  from orders,customer
where orders.Customer_ID=Customer.Customer_ID and orders.Purch_Amt between 500 and 2000

--3
 select customer.Cust_Name as"costomerName",customer.City,salesman.Name as"salesman",salesman.Commission from salesman,customer
 where salesman.Salesman_ID=customer.Salesman_ID



 --4
 select customer.Cust_Name, customer.City,salesman.Commission from salesman,customer
 where salesman.Commission>0.12 and customer.Salesman_ID=salesman.Salesman_ID

 --5
  select customer.Cust_Name, customer.City,salesman.Name as "salesman",salesman.City, salesman.Commission from salesman,customer
 where salesman.Commission>0.12 and customer.Salesman_ID=salesman.Salesman_ID and customer.city!=salesman.City

--6
select orders.Ord_NO,orders.Ord_Date, orders.Purch_Amt,customer.Cust_Name,customer.Grade,salesman.Name as "salesman",salesman.Commission from orders INNER JOIN customer 
ON customer.customer_id=orders.customer_id 
INNER JOIN salesman  
ON salesman.salesman_id=customer.salesman_id;


--7
--attention required
 select * from orders
left outer join customer
ON orders.Customer_ID=customer.Customer_ID
left outer join salesman
on orders.Salesman_ID=salesman.Salesman_ID;

--8
select customer.Cust_Name,customer.City, customer.Grade,salesman.Name as "salesman",salesman.City from customer inner join salesman on salesman.Salesman_ID=customer.Salesman_ID order by customer.Customer_ID ASC 

--9

select customer.Cust_Name,customer.City,customer.Grade,salesman.name as "salesman",salesman.City  from customer inner join salesman on salesman.Salesman_ID=customer.Salesman_ID
where customer.Grade<300

--10


SELECT a.cust_name,a.city, b.ord_no,
b.ord_date,b.purch_amt AS "Order Amount" 
FROM customer a 
LEFT OUTER JOIN orders b 
ON a.customer_id=b.customer_id 
order by b.ord_date;


--11
SELECT customer.cust_name,customer.city, orders.ord_no,
orders.ord_date,orders.purch_amt, 
salesman.name as "salesman",salesman.commission 
FROM customer 
LEFT OUTER JOIN orders  
ON customer.customer_id=orders.customer_id 
LEFT OUTER JOIN salesman 
ON salesman.salesman_id=orders.salesman_id;


--12
SELECT customer.cust_name,customer.city,customer.grade, 
salesman.name AS "Salesman", salesman.city 
FROM customer  
RIGHT OUTER JOIN salesman  
ON salesman.salesman_id=customer.salesman_id 
ORDER BY salesman.salesman_id;


--13
select salesman.Name as "salesman",customer.Cust_Name,customer.City,customer.Grade,orders.Ord_NO,orders.Ord_Date,orders.Purch_Amt from customer 
right outer join salesman on customer.Salesman_ID=salesman.Salesman_ID 
right outer join  orders on orders.Customer_ID=customer.Customer_ID

--14

SELECT a.cust_name,a.city,a.grade, 
b.name AS "Salesman", 
c.ord_no, c.ord_date, c.purch_amt 
FROM customer a 
RIGHT OUTER JOIN salesman b 
ON b.salesman_id=a.salesman_id 
LEFT OUTER JOIN orders c 
ON c.customer_id=a.customer_id 
WHERE c.purch_amt>=2000 
AND a.grade IS NOT NULL;



--15
SELECT a.cust_name,a.city,a.grade, 
b.name AS "Salesman", 
c.ord_no, c.ord_date, c.purch_amt 
FROM customer a 
RIGHT OUTER JOIN salesman b 
ON b.salesman_id=a.salesman_id 
LEFT OUTER JOIN orders c 
ON c.customer_id=a.customer_id 
WHERE c.purch_amt>=2000 
AND a.grade IS NOT NULL;

--16
SELECT a.cust_name,a.city, b.ord_no,
b.ord_date,b.purch_amt AS "Order Amount" 
FROM customer a 
FULL OUTER JOIN orders b 
ON a.customer_id=b.customer_id 
WHERE a.grade IS NOT NULL;

--17
SELECT * 
FROM salesman a 
CROSS JOIN customer b;

--18

SELECT * 
FROM salesman a 
CROSS JOIN customer b 
WHERE a.city IS NOT NULL;

--19

SELECT * 
FROM salesman a 
CROSS JOIN  customer b 
WHERE a.city IS NOT NULL 
AND b.grade IS NOT NULL;


--20
SELECT * 
FROM salesman a 
CROSS JOIN customer b 
WHERE a.city IS NOT NULL 
AND b.grade IS NOT NULL 
AND  a.city<>b.city;

